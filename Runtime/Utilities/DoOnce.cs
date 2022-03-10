using System;
using System.Collections;
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
        UnityEvent eventToDo;

        /// <summary>
        /// Have the event executed once?
        /// </summary>
        GameStateValue<bool> haveDoneAction { get; set; } = new();

        void Start()
        {
            Do();
        }

        /// <summary>
        /// Do the event.
        /// </summary>
        public void Do()
        {
            if (!haveDoneAction)
            {
                haveDoneAction.Value = true;
                StartCoroutine(DoEvent());
            }
        }

        /// <summary>
        /// The actual code that will trigger the event.
        /// </summary>
        IEnumerator DoEvent()
        {
            yield return new WaitForEndOfFrame();
            eventToDo.Invoke();
        }

        public void LoadPersistentData(GameSaveData saveData)
        {
            haveDoneAction = saveData.GetState(this, HaveDoneActionId, false);
        }
    }
}