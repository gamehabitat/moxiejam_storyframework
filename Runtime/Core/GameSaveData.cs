using System;
using UnityEngine;
using System.Collections.Generic;

namespace StoryFramework
{
    public delegate void GameStateValueModifiedDelegate();

    public interface IGameStateValue
    {
        string Identifier { get; }
    }

    [Serializable]
    public class GameStateValue<TValue> : IGameStateValue where TValue : IEquatable<TValue>
    {
        public string Identifier { get; private set; }

        [SerializeField]
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
    
    [Serializable]
    public class GameStateValueBool : GameStateValue<bool> {}
    [Serializable]
    public class GameStateValueInt : GameStateValue<int> {}
    [Serializable]
    public class GameStateValueFloat : GameStateValue<float> {}
    [Serializable]
    public class GameStateValueString : GameStateValue<string> {}

    /// <summary>
    /// Holds the currently active games persistent state.
    /// </summary>
    public class GameSaveData : IDisposable
    {
        /// <summary>
        /// Persistent object states.
        /// </summary>
        Dictionary<string, IGameStateValue> gameObjectStates = new();
        Dictionary<string, GameStateValue<bool>> gameObjectStateBools = new();
        Dictionary<string, GameStateValue<int>> gameObjectStateInts = new();
        Dictionary<string, GameStateValue<float>> gameObjectStateFloats = new();
        Dictionary<string, GameStateValue<string>> gameObjectStateStrings = new();

        // Dispose flag.
        bool isDisposed = false;

        /// <summary>
        /// Inventory data.
        /// </summary>
        public Inventory Inventory { get; private set; } = new();

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
            if (isDisposed)
            {
                return;
            }

            if (disposing)
            {
                Game.OnBeginLoadScene -= OnBeginLoadScene;

                Inventory.Clear();
                Inventory = null;
            }

            isDisposed = true;
        }

        void OnBeginLoadScene(string sceneName)
        {
            Save();
        }

        public void Save()
        {
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

            var states = GetStateStorage<TValue>();
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
        public void SetGlobalState<TValue>(string id, string property, TValue value) where TValue : IEquatable<TValue>
        {
            if (!string.IsNullOrEmpty(property))
            {
                id = SanitizeId($"{id}[{property}]");
            }
            else
            {
                id = SanitizeId(id);
            }

            var states = GetStateStorage<TValue>();
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
        /// Takes a IPersistentComponent and try to retrieve its identifier.
        /// </summary>
        /// <param name="dataContainer">Target to find identifier for</param>
        /// <param name="property">Property on target that is accessed</param>
        /// <param name="id">The targets identifier</param>
        /// <returns>True if a identifier was found</returns>
        bool TryGetId<T>(T dataContainer, string property, out string id) where T : MonoBehaviour, IPersistentComponent
        {
            if (!dataContainer.TryGetComponent<PersistentObject>(out var dataId))
            {
                id = string.Empty;
                Debug.LogError($"No persistent object found on the game object {dataContainer.name}. Please add a PersistentObject to it.");
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

            id = SanitizeId($"{id}[{property}]");

            return true;
        }

        Dictionary<string, GameStateValue<TValue>> GetStateStorage<TValue>() where TValue : IEquatable<TValue>
        {
            if (typeof(TValue) == typeof(bool))
            {
                return gameObjectStateBools as Dictionary<string, GameStateValue<TValue>>;
            }
            else if (typeof(TValue) == typeof(int))
            {
                return gameObjectStateInts as Dictionary<string, GameStateValue<TValue>>;
            }
            else if (typeof(TValue) == typeof(float))
            {
                return gameObjectStateFloats as Dictionary<string, GameStateValue<TValue>>;
            }
            else if (typeof(TValue) == typeof(string))
            {
                return gameObjectStateStrings as Dictionary<string, GameStateValue<TValue>>;
            }

            return null;
        }

        public bool ContainsState<TValue>(string id) where TValue : IEquatable<TValue>
        {
            id = SanitizeId(id);
            var states = GetStateStorage<TValue>();
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
            var states = GetStateStorage<TValue>();
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
            }

            return states[id];
        }

        static string SanitizeId(string id)
        {
            return id.ToLower();
        }
    }
}