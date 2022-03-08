using System;
using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace StoryFramework
{
    public delegate void GameStateValueModifiedDelegate();

    public interface IGameStateValue
    {
        string Identifier { get; }
    }

    public class GameStateValue<TValue> : IGameStateValue where TValue : IEquatable<TValue>
    {
        public string Identifier { get; private set; }

        TValue currentValue;

        public TValue Value
        {
            get => currentValue;
            set
            {
                currentValue = value;
                OnValueModified?.Invoke();
            }
        }

        public event GameStateValueModifiedDelegate OnValueModified;

        public GameStateValue()
        {
            Identifier = string.Empty;
            currentValue = default;
        }

        public GameStateValue(string identifier, TValue defaultValue)
        {
            Identifier = identifier;
            currentValue = defaultValue;
        }

        public static implicit operator TValue(GameStateValue<TValue> gameStateValue) => gameStateValue.currentValue;
    }

    /// <summary>
    /// Holds the currently active games persistent state.
    /// </summary>
    public class GameSaveData : IDisposable
    {
        public delegate void StateBoolUpdatedDelegate(string id);

        public delegate void StateIntUpdatedDelegate(string id);

        public delegate void StateFloatUpdatedDelegate(string id);

        public delegate void StateStringUpdatedDelegate(string id);

        /// <summary>
        /// Persistent object states.
        /// </summary>
        Dictionary<string, bool> gameObjectStateBools = new Dictionary<string, bool>();

        Dictionary<string, int> gameObjectStateInts = new Dictionary<string, int>();
        Dictionary<string, float> gameObjectStateFloats = new Dictionary<string, float>();
        Dictionary<string, string> gameObjectStateStrings = new Dictionary<string, string>();
        Dictionary<string, IGameStateValue> gameObjectStates = new Dictionary<string, IGameStateValue>();
        Dictionary<string, GameStateValue<bool>> gameObjectStateBools2 = new Dictionary<string, GameStateValue<bool>>();
        Dictionary<string, GameStateValue<int>> gameObjectStateInts2 = new Dictionary<string, GameStateValue<int>>();
        Dictionary<string, GameStateValue<float>> gameObjectStateFloats2 = new Dictionary<string, GameStateValue<float>>();
        Dictionary<string, GameStateValue<string>> gameObjectStateStrings2 = new Dictionary<string, GameStateValue<string>>();

        // Currently active persistent components.
        List<IPersistentComponent> peristentComponentsInScene = new List<IPersistentComponent>(100);

        /// <summary>
        /// Inventory data.
        /// </summary>
        public Inventory Inventory { get; set; } = new Inventory();

        public event StateBoolUpdatedDelegate OnStateBoolUpdated;
        public event StateIntUpdatedDelegate OnStateIntUpdated;
        public event StateFloatUpdatedDelegate OnStateFloatUpdated;
        public event StateStringUpdatedDelegate OnStateStringUpdated;

        bool m_IsDisposed = false;

        /// <summary>
        /// Constructs a new save data.
        /// </summary>
        public GameSaveData()
        {
            Game.OnBeginLoadScene += OnBeginLoadScene;
        }

        /// <summary>
        /// Cleans up the save data resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (m_IsDisposed)
            {
                return;
            }

            if (disposing)
            {
                Game.OnBeginLoadScene -= OnBeginLoadScene;

                peristentComponentsInScene.Clear();

                Inventory.Clear();
                Inventory = null;
            }

            m_IsDisposed = true;
        }

        void OnBeginLoadScene(string sceneName)
        {
            Save();

            // Currently when this is called we exprect the previous scene to be unloaded at the same time.
            // TODO: Handle unload, additive scenes and DontDestroyOnLoad()
            peristentComponentsInScene.Clear();
        }

        public void Save()
        {
            foreach (var persistentComponent in peristentComponentsInScene)
            {
                //persistentComponent.SavePersistentData(this);
            }
        }

        Dictionary<string, UnityEvent<GameSaveData>> stateChangedListeners = new Dictionary<string, UnityEvent<GameSaveData>>();

        public void AddStateChangedListener(string id, string property, UnityAction<GameSaveData> onStateChanged)
        {
            id = SanitizeId($"{id}[{property}]");
            if (!stateChangedListeners.ContainsKey(id))
            {
                stateChangedListeners.Add(id, new UnityEvent<GameSaveData>());
            }

            stateChangedListeners[id].AddListener(onStateChanged);
        }

        public void RemoveStateChangedListener(string id, string property, UnityAction<GameSaveData> onStateChanged)
        {
            if (!stateChangedListeners.ContainsKey(id))
            {
                return;
            }

            stateChangedListeners[id].RemoveListener(onStateChanged);
        }

        public void AddStateChangedListener<T>(T dataContainer, string property, UnityAction<GameSaveData> onStateChanged) where T : MonoBehaviour, IPersistentComponent
        {
            if (!TryGetId(dataContainer, property, out var id))
            {
                return;
            }

            if (!stateChangedListeners.ContainsKey(id))
            {
                stateChangedListeners.Add(id, new UnityEvent<GameSaveData>());
            }

            stateChangedListeners[id].AddListener(onStateChanged);
        }

        public void RemoveStateChangedListener<T>(T dataContainer, string property, UnityAction<GameSaveData> onStateChanged) where T : MonoBehaviour, IPersistentComponent
        {
            if (!TryGetId(dataContainer, property, out var id))
            {
                return;
            }

            if (!stateChangedListeners.ContainsKey(id))
            {
                return;
            }

            stateChangedListeners[id].RemoveListener(onStateChanged);
        }

        /// <summary>
        /// Gets a global state. Useful for cross-scene states.
        /// </summary>
        /// <param name="id">State identifier</param>
        /// <returns>Current state value</returns>
        public bool GetGlobalStateBool(string id)
        {
            return GetGlobalState<bool>(id, string.Empty);
        }

        /// <summary>
        /// Gets a global state. Useful for cross-scene states.
        /// </summary>
        /// <param name="id">State identifier</param>
        /// <returns>Current state value</returns>
        public int GetGlobalStateInt(string id)
        {
            return GetGlobalState<int>(id, string.Empty);
        }

        /// <summary>
        /// Gets a global state. Useful for cross-scene states.
        /// </summary>
        /// <param name="id">State identifier</param>
        /// <returns>Current state value</returns>
        public float GetGlobalStateFloat(string id)
        {
            return GetGlobalState<float>(id, string.Empty);
        }

        /// <summary>
        /// Gets a global state. Useful for cross-scene states.
        /// </summary>
        /// <param name="id">State identifier</param>
        /// <returns>Current state value</returns>
        public string GetGlobalStateString(string id)
        {
            return GetGlobalState<string>(id, string.Empty);
        }

        /// <summary>
        /// Sets a global state. Useful for cross-scene states.
        /// </summary>
        /// <param name="id">State identifier</param>
        /// <param name="value">State value</param>
        public void SetGlobalState(string id, bool value)
        {
            SetGlobalState<bool>(id, string.Empty, value);
        }

        /// <summary>
        /// Sets a global state. Useful for cross-scene states.
        /// </summary>
        /// <param name="id">State identifier</param>
        /// <param name="value">State value</param>
        public void SetGlobalState(string id, int value)
        {
            SetGlobalState<int>(id, string.Empty, value);
        }

        /// <summary>
        /// Sets a global state. Useful for cross-scene states.
        /// </summary>
        /// <param name="id">State identifier</param>
        /// <param name="value">State value</param>
        public void SetGlobalState(string id, float value)
        {
            SetGlobalState<float>(id, string.Empty, value);
        }

        /// <summary>
        /// Sets a global state. Useful for cross-scene states.
        /// </summary>
        /// <param name="id">State identifier</param>
        /// <param name="value">State value</param>
        public void SetGlobalState(string id, string value)
        {
            SetGlobalState<string>(id, string.Empty, value);
        }

        /// <summary>
        /// Gets a global state. Useful for cross-scene states.
        /// </summary>
        /// <param name="id">State identifier</param>
        /// <param name="property">Property on object with id</param>
        /// <returns>Current state value</returns>
        public bool GetGlobalStateBool(string id, string property)
        {
            return GetGlobalState<bool>(id, property);
        }

        /// <summary>
        /// Gets a global state. Useful for cross-scene states.
        /// </summary>
        /// <param name="id">State identifier</param>
        /// <param name="property">Property on object with id</param>
        /// <returns>Current state value</returns>
        public int GetGlobalStateInt(string id, string property)
        {
            return GetGlobalState<int>(id, property);
        }

        /// <summary>
        /// Gets a global state. Useful for cross-scene states.
        /// </summary>
        /// <param name="id">State identifier</param>
        /// <param name="property">Property on object with id</param>
        /// <returns>Current state value</returns>
        public float GetGlobalStateFloat(string id, string property)
        {
            return GetGlobalState<float>(id, property);
        }

        /// <summary>
        /// Gets a global state. Useful for cross-scene states.
        /// </summary>
        /// <param name="id">State identifier</param>
        /// <param name="property">Property on object with id</param>
        /// <returns>Current state value</returns>
        public string GetGlobalStateString(string id, string property)
        {
            return GetGlobalState<string>(id, property);
        }

        /// <summary>
        /// Sets a global state. Useful for cross-scene states.
        /// </summary>
        /// <param name="id">State identifier</param>
        /// <param name="property">Property on object with id</param>
        /// <param name="value">State value</param>
        public void SetGlobalState(string id, string property, bool value)
        {
            SetGlobalState<bool>(id, property, value);
        }

        /// <summary>
        /// Sets a global state. Useful for cross-scene states.
        /// </summary>
        /// <param name="id">State identifier</param>
        /// <param name="property">Property on object with id</param>
        /// <param name="value">State value</param>
        public void SetGlobalState(string id, string property, int value)
        {
            SetGlobalState<int>(id, property, value);
        }

        /// <summary>
        /// Sets a global state. Useful for cross-scene states.
        /// </summary>
        /// <param name="id">State identifier</param>
        /// <param name="property">Property on object with id</param>
        /// <param name="value">State value</param>
        public void SetGlobalState(string id, string property, float value)
        {
            SetGlobalState<float>(id, property, value);
        }

        /// <summary>
        /// Sets a global state. Useful for cross-scene states.
        /// </summary>
        /// <param name="id">State identifier</param>
        /// <param name="property">Property on object with id</param>
        /// <param name="value">State value</param>
        public void SetGlobalState(string id, string property, string value)
        {
            SetGlobalState<string>(id, property, value);
        }

        /// <summary>
        /// Gets a global state. Useful for cross-scene states.
        /// </summary>
        /// <param name="id">State identifier</param>
        /// <param name="property">Property on object with id</param>
        /// <param name="createIfNeeded">Create the state if it doesn't exist.</param>
        /// <param name="defaultValue">Default value if state is crated.</param>
        /// <returns>Current state value</returns>
        public GameStateValue<TValue> GetGlobalState<TValue>(string id, string property, bool createIfNeeded = false, TValue defaultValue = default) where TValue : IEquatable<TValue>
        {
            if (!string.IsNullOrEmpty(property))
            {
                id = SanitizeId($"{id}[{property}]");
            }
            else
            {
                id = SanitizeId(id);
            }

            var states = GetStateStorage2<TValue>();
            if (states == null)
            {
                Debug.LogError($"Unsupported storage type {typeof(TValue).Name} in container.");
                return default;
            }

            if (!ContainsState<TValue>(id))
            {
                if (!createIfNeeded)
                {
                    return default;
                }
                
                states.Add(id, new GameStateValue<TValue>(property, defaultValue));
            }

            return states[id];
        }

        /// <summary>
        /// Sets a global state. Useful for cross-scene states.
        /// </summary>
        /// <param name="id">State identifier</param>
        /// <param name="property">Property on object with id</param>
        /// <param name="value">State value</param>
        void SetGlobalState<TValue>(string id, string property, TValue value) where TValue : IEquatable<TValue>
        {
            if (!string.IsNullOrEmpty(property))
            {
                id = SanitizeId($"{id}[{property}]");
            }
            else
            {
                id = SanitizeId(id);
            }

            var states = GetStateStorage2<TValue>();
            if (states == null)
            {
                Debug.LogError($"Unsupported storage type {typeof(TValue).Name} in container.");
                return;
            }

            if (!ContainsState<TValue>(id))
            {
                states.Add(id, new GameStateValue<TValue>(property, value));
            }
            else
            {
                states[id].Value = value;
            }
        }

        /*/// <summary>
        /// Retrieves the value state of a data container.
        /// </summary>
        /// <param name="dataContainer">Owner of the data</param>
        /// <param name="key">Identifier for the value</param>
        /// <param name="defaultValue">Default value if none exist yet.</param>
        /// <returns>Current state</returns>
        public TValue GetState<T, TValue>(T dataContainer, string key, TValue defaultValue) where T : MonoBehaviour, IPersistentComponent
        {
            if (!TryGetId(dataContainer, key, out var id))
            {
                return defaultValue;
            }

            return EvaluateState(id, defaultValue, false);
        }*/

        /// <summary>
        /// Retrieves the value state of a data container.
        /// </summary>
        /// <param name="dataContainer">Owner of the data</param>
        /// <param name="property">Identifier for the value</param>
        /// <param name="defaultValue">Default value if none exist yet.</param>
        /// <returns>Current state</returns>
        public GameStateValue<TValue> GetState<T, TValue>(T dataContainer, string property, TValue defaultValue) where T : MonoBehaviour, IPersistentComponent where TValue : IEquatable<TValue>
        {
            if (!TryGetId(dataContainer, property, out var id))
            {
                return new GameStateValue<TValue>(id, defaultValue);
            }

            return EvaluateState(id, defaultValue, false);
        }

        /// <summary>
        /// Stores the value state of a data container.
        /// </summary>
        /// <param name="dataContainer">Owner of the data</param>
        /// <param name="key">Identifier for the value</param>
        /// <param name="value">Value to set</param>
        public void SetState<T, TValue>(T dataContainer, string key, TValue value) where T : MonoBehaviour, IPersistentComponent where TValue : IEquatable<TValue>
        {
            if (TryGetId(dataContainer, key, out var id))
            {
                EvaluateState(id, value, true);
            }
        }

        /// <summary>
        /// Takes a game object and returns a id for use with the persistent state.
        /// </summary>
        /// <param name="gameObject">Target object to get id</param>
        /// <param name="key">A extra key for use with multiple value states.</param>
        /// <returns>The identifier of the game object</returns>
        [Obsolete("This method is obsolete and will be removed.")]
        string GetId(GameObject gameObject, string key)
        {
            if (gameObject.TryGetComponent<PersistentObject>(out var persistentObject))
            {
                return persistentObject.Identifier;
            }

            if (gameObject.TryGetComponent<GuidComponent>(out var guidComponent))
            {
                return guidComponent.GetGuid().ToString("D").ToLower();
            }

            Debug.LogWarning($"{gameObject.name} is missing a GUID. Please add a GuidComponent or a PersistentObject. Generating a temporary id...");
            return $"{gameObject.scene.name}_{gameObject.name}[{gameObject.transform.hierarchyCount.ToString()}][{key}]".ToLower();
        }

        bool TryGetId<T>(T dataContainer, string key, out string id) where T : MonoBehaviour, IPersistentComponent
        {
            if (!dataContainer.TryGetComponent<PersistentObject>(out var dataId))
            {
                id = string.Empty;
                Debug.LogError($"No persistent object found on the game object {dataContainer.name}. Please add a PeristentObject to it.");
                return false;
            }

            id = dataId.Identifier;
            if (!dataId.HasCustomIdentifier)
            {
                // Append component index to identifier.
                var persistentComponents = dataId.GetComponents<IPersistentComponent>();
                int dataIndex = Array.FindIndex(persistentComponents, x => x == dataContainer);
                if (dataIndex < 0)
                {
                    Debug.LogError($"Data container not registered to persistent object in {dataContainer.name}. Please add it.");
                    return false;
                }

                id += $"[{dataIndex.ToString()}]";
            }

            id = SanitizeId($"{id}[{key}]");

            // Add dataContainer to tracked objects.
            if (!peristentComponentsInScene.Contains(dataContainer))
            {
                peristentComponentsInScene.Add(dataContainer);
            }

            return true;
        }

        Dictionary<string, T> GetStateStorage<T>()
        {
            if (typeof(T) == typeof(bool))
            {
                return gameObjectStateBools as Dictionary<string, T>;
            }
            else if (typeof(T) == typeof(int))
            {
                return gameObjectStateBools as Dictionary<string, T>;
            }
            else if (typeof(T) == typeof(float))
            {
                return gameObjectStateBools as Dictionary<string, T>;
            }
            else if (typeof(T) == typeof(string))
            {
                return gameObjectStateBools as Dictionary<string, T>;
            }

            return null;
        }

        Dictionary<string, GameStateValue<TValue>> GetStateStorage2<TValue>() where TValue : IEquatable<TValue>
        {
            if (typeof(TValue) == typeof(bool))
            {
                return gameObjectStateBools2 as Dictionary<string, GameStateValue<TValue>>;
            }
            else if (typeof(TValue) == typeof(int))
            {
                return gameObjectStateBools2 as Dictionary<string, GameStateValue<TValue>>;
            }
            else if (typeof(TValue) == typeof(float))
            {
                return gameObjectStateBools2 as Dictionary<string, GameStateValue<TValue>>;
            }
            else if (typeof(TValue) == typeof(string))
            {
                return gameObjectStateBools2 as Dictionary<string, GameStateValue<TValue>>;
            }

            return null;
        }

        public void InvokeStateUpdated<TValue>(string id)
        {
            if (typeof(TValue) == typeof(bool))
            {
                OnStateBoolUpdated?.Invoke(id);
            }
            else if (typeof(TValue) == typeof(int))
            {
                OnStateIntUpdated?.Invoke(id);
            }
            else if (typeof(TValue) == typeof(float))
            {
                OnStateFloatUpdated?.Invoke(id);
            }
            else if (typeof(TValue) == typeof(string))
            {
                OnStateStringUpdated?.Invoke(id);
            }
        }

        public bool ContainsState<TValue>(string id) where TValue : IEquatable<TValue>
        {
            id = SanitizeId(id);
            var states = GetStateStorage2<TValue>();
            if (states == null)
            {
                //throw new Exception); 
                Debug.LogError($"Unsupported storage type {typeof(TValue).Name} in container.");
                return false;
            }

            return states.ContainsKey(id);
        }

        GameStateValue<TValue> EvaluateState<TValue>(string id, TValue currentValue, bool setState) where TValue : IEquatable<TValue>
        {
            id = SanitizeId(id);
            var states = GetStateStorage2<TValue>();
            if (states == null)
            {
                //throw new Exception); 
                Debug.LogError($"Unsupported storage type {typeof(TValue).Name} in container.");
                return new GameStateValue<TValue>(id, currentValue);
            }

            if (!ContainsState<TValue>(id))
            {
                states.Add(id, new GameStateValue<TValue>(id, currentValue));
            }
            else if (setState)
            {
                states[id].Value = currentValue;
                InvokeStateUpdated<TValue>(id);
            }

            return states[id];
        }

        static string SanitizeId(string id)
        {
            return id.ToLower();
        }
    }
}