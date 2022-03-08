using System;
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
        void LoadPersistentData(GameSaveData saveData);
        void SavePersistentData(GameSaveData saveData);
    }
    
    /// <summary>
    /// Adds persistence to the object.
    /// The component will save the Active state of the game object and apply it on loading of the scene. 
    /// </summary>
    [AddComponentMenu("MoxieJam/StoryFramework/Persistent Object")]
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

        GameStateValue<bool> IsActive = new GameStateValue<bool>();

        protected override void OnDestroy()
        {
            IsActive.OnValueModified -= OnActiveStateChanged;
            Game.OnBeginLoadScene -= OnBeginLoadScene;
            base.OnDestroy();
        }

        void OnEnable()
        {
            IsActive.Value = true;
        }

        void OnDisable()
        {
            IsActive.Value = false;
        }

        void OnActiveStateChanged()
        {
            gameObject.SetActive(IsActive);
        }

        void OnBeginLoadScene(string scenename)
        {
            // Hack to ignore disable on unload of scene.
            IsActive.OnValueModified -= OnActiveStateChanged;
            IsActive = new GameStateValue<bool>();
        }

        public void LoadPersistentData(GameSaveData saveData)
        {
            IsActive = saveData.GetState(this, IsActiveStateId, activeOnStart);
            gameObject.SetActive(IsActive);
            IsActive.OnValueModified += OnActiveStateChanged;
            Game.OnBeginLoadScene += OnBeginLoadScene;
        }

        public void SavePersistentData(GameSaveData saveData)
        {
            saveData.SetState(this, IsActiveStateId, gameObject.activeSelf);
        }
    }
}