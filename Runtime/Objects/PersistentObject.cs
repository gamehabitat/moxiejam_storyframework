using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
//using UnityEditor.Experimental.SceneManagement;
#endif

namespace StoryFramework
{
    public interface IPersistentComponent
    {
        GameStateIdentifier[] GameStates { get; }
        void LoadPersistentData(GameSaveData saveData);
    }
    
    /// <summary>
    /// Adds persistence to the object.
    /// The component will save the Active state of the game object and apply it on loading of the scene. 
    /// </summary>
    [AddComponentMenu("MoxieJam/StoryFramework/Objects/Persistent Object")]
    [DisallowMultipleComponent]
    public class PersistentObject : GuidComponent, IPersistentComponent
    {
        public const string IsActiveStateId = "PersistentObject_IsActive";

        /// <summary>
        /// Whether to have this game object active when loading the scene.
        /// </summary>
        [SerializeField]
        bool activeOnStart = true;

        [SerializeField]
        string customIdentifier;

        public bool HasCustomIdentifier => !string.IsNullOrEmpty(customIdentifier);
        public string Identifier => HasCustomIdentifier ? customIdentifier : GetGuid().ToString("D");

        GameState IsActiveState => new()
        {
            Identifier = new(Identifier, IsActiveStateId),
            Value = StateManager.Global.GetOrCreate(new(Identifier, IsActiveStateId), new(activeOnStart)).Value
        };
        bool isActive;

        /// <summary>
        /// Available states on this persistent object.
        /// </summary>
        public GameStateIdentifier[] GameStates => new[] { IsActiveState.Identifier };

        protected override void OnDestroy()
        {
            StateManager.Global.OnStateChanged -= OnStateChanged;
            Game.OnBeginLoadScene -= OnBeginLoadScene;
            base.OnDestroy();
        }

        void OnEnable()
        {
            isActive = true;
        }

        void OnDisable()
        {
            isActive = false;
        }

        void OnBeginLoadScene(string sceneName)
        {
            // Hack to ignore disable on unload of scene.
            StateManager.Global.OnStateChanged -= OnStateChanged;
            isActive = default;
        }
        
        void OnStateChanged(in GameState state)
        {
            if (state.Identifier.Equals(IsActiveState.Identifier))
            {
                isActive = state;
                gameObject.SetActive(state);
            }
        }
        
        public void LoadPersistentData(GameSaveData saveData)
        {
            var isActiveState = StateManager.Global.GetOrCreate(IsActiveState.Identifier, new(activeOnStart));
            gameObject.SetActive(isActiveState);
            StateManager.Global.OnStateChanged += OnStateChanged;
            Game.OnBeginLoadScene += OnBeginLoadScene;
        }
    }
}