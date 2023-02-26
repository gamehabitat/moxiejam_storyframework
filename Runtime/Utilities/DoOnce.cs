using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using static StoryFramework.Utilities.GameStateUtilities;

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
        /// Available states on this persistent object.
        /// </summary>
        public GameStateProperty[] GameStateProperties => new[]
        {
            new GameStateProperty(HaveDoneActionId, GameStateTypes.BooleanFlag)
        };

        /// <summary>
        /// Have the event executed once?
        /// </summary>
        public GameState HaveDoneActionState => new(GetIdentifier(this, in GameStateProperties[0]), false);

        void Start()
        {
            Do();
            StateManager.Global.OnStateChanged += OnStateChanged;
        }

        void OnDestroy()
        {
            StateManager.Global.OnStateChanged -= OnStateChanged;
        }

        /// <summary>
        /// Do the event.
        /// </summary>
        public void Do()
        {
            if (!HaveDoneActionState)
            {
                StateManager.Global.SetState(HaveDoneActionState.Identifier, true);
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

        void OnStateChanged(in GameState state)
        {
            if (state.Identifier.Equals(HaveDoneActionState.Identifier))
            {
            }
        }

        public void LoadPersistentData(GameSaveData saveData)
        {
        }
    }
}