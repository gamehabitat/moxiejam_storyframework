using UnityEngine;
using static StoryFramework.Utilities.GameStateUtilities;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
//using UnityEditor.Experimental.SceneManagement;
#endif

namespace StoryFramework
{
    public interface IPersistentComponent
    {
        public GameStateProperty[] GameStateProperties { get; }
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
        bool useCustomIdentifier;

        [SerializeField]
        [EnableIf(nameof(useCustomIdentifier))]
        string customIdentifier;

        [SerializeField]
        [EnableIf(nameof(useCustomIdentifier))]
        [GameStateType("Active On Start", GameStateTypes.BooleanFlag)]
        //[CustomLabel("Custom Identifier 2")]
        GameState isActiveState;

        [SerializeField]
        [EnableIf(nameof(useCustomIdentifier))]
        string customIdentifierTest;

        /// <summary>
        /// Available states on this persistent object.
        /// </summary>
        public GameStateProperty[] GameStateProperties => new[]
        {
            new GameStateProperty(IsActiveStateId, GameStateTypes.BooleanFlag)
        };

        public bool HasCustomIdentifier => !string.IsNullOrEmpty(customIdentifier);
        public bool NewHasCustomIdentifier => useCustomIdentifier && (!string.IsNullOrEmpty(isActiveState.Identifier.Identifier));
        private string NewIdentifier => NewHasCustomIdentifier ? isActiveState.Identifier.Identifier : GetGuid().ToString("D");
        public string Identifier => HasCustomIdentifier
            ? customIdentifier
            : NewIdentifier;

        GameState IsActiveState => StateManager.Global.GetOrCreate(GetIdentifier(this, in GameStateProperties[0]), activeOnStart);

        protected override void OnDestroy()
        {
            StateManager.Global.OnStateChanged -= OnStateChanged;
            Game.OnBeginLoadScene -= OnBeginLoadScene;
            base.OnDestroy();
        }

        void OnEnable()
        {
            IsActiveState.SetValue(true);
        }

        void OnDisable()
        {
            IsActiveState.SetValue(false);
        }

        void OnBeginLoadScene(string sceneName)
        {
            // Hack to ignore disable on unload of scene.
            StateManager.Global.OnStateChanged -= OnStateChanged;
        }
        
        void OnStateChanged(in GameState state)
        {
            if (state.Identifier.Equals(IsActiveState.Identifier))
            {
                gameObject.SetActive(state);
            }
        }
        
        public void LoadPersistentData(GameSaveData saveData)
        {
            gameObject.SetActive(IsActiveState.BooleanValue);
            StateManager.Global.OnStateChanged += OnStateChanged;
            Game.OnBeginLoadScene += OnBeginLoadScene;
        }
    }
}