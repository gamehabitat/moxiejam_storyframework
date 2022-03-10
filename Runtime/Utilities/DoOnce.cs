using System;
using UnityEngine;
using UnityEngine.Events;

namespace StoryFramework.Utilities
{
    /// <summary>
    /// Executes a event once when the component starts.
    /// </summary>
    [AddComponentMenu("MoxieJam/StoryFramework/Utilities/Do Once")]
    [RequireComponent(typeof(PersistentObject))]
    public class DoOnce : MonoBehaviour, IPersistentComponent
    {
        public const string HaveDoneActionId = "DoOnce_HaveDoneAction";

        /// <summary>
        /// The event to do once.
        /// </summary>
        [SerializeField]
        UnityEvent onStart;

        /// <summary>
        /// Have the event executed once?
        /// </summary>
        GameStateValue<bool> haveDoneAction { get; set; } = new();

        void Start()
        {
            onStart.Invoke();
        }

        public void LoadPersistentData(GameSaveData saveData)
        {
            haveDoneAction = saveData.GetState(this, HaveDoneActionId, false);
        }
    }
}