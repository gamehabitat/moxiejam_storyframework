using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using static StoryFramework.Utilities.GameStateUtilities;

namespace StoryFramework
{
    /// <summary>
    /// Adds functionality for locking/unlocking this object.
    /// A lockable object have a locked state and events for locking/unlocking and using.
    /// </summary>
    [AddComponentMenu("MoxieJam/StoryFramework/Objects/Lockable Object")]
    [RequireComponent(typeof(PersistentObject))]
    public class LockableObject : MonoBehaviour, IPersistentComponent
    {
        public const string LockedStateId = "LockableObject_IsLocked";
        
        [FormerlySerializedAs("isLockedOnStart")]
        [SerializeField]
        bool isLocked = true;

        [SerializeField]
        UnityEvent onStartLocked;

        [SerializeField]
        UnityEvent onStartUnlocked;

        [SerializeField]
        UnityEvent onLocked;

        [SerializeField]
        UnityEvent onUnlocked;

        [SerializeField]
        UnityEvent onUseLocked;

        [SerializeField]
        UnityEvent onUseUnlocked;

        /// <summary>
        /// Is object locked or not?
        /// </summary>
        public GameState IsLockedState => new() { Identifier = GetIdentifier(this, LockedStateId), Value = new(isLockedValue) };
        bool isLockedValue;

        /// <summary>
        /// Available states on this persistent object.
        /// </summary>
        public GameStateIdentifier[] GameStates => new[] { IsLockedState.Identifier };

        public void Start()
        {
            if (IsLockedState)
            {
                onStartLocked.Invoke();
            }
            else
            {
                onStartUnlocked.Invoke();
            }

            StateManager.Global.OnStateChanged += OnStateChanged;
        }

        void OnDestroy()
        {
            StateManager.Global.OnStateChanged -= OnStateChanged;
        }

        /// <summary>
        /// Try to use the locked object.
        /// Will call the <see cref="onUseLocked">Use Locked</see> event if object is locked. 
        /// Otherwise it will call the <see cref="onUseUnlocked">Use Unlocked</see> event if object is unlocked. 
        /// </summary>
        public void Use()
        {
            if (IsLockedState)
            {
                onUseLocked?.Invoke();
            }
            else
            {
                onUseUnlocked?.Invoke();
            }
        }

        /// <summary>
        /// Sets the object as locked.
        /// </summary>
        public void Lock()
        {
            if (!IsLockedState)
            {
                StateManager.Global.SetState(IsLockedState.Identifier, new(true));
            }
        }

        /// <summary>
        /// Sets the object as unlocked.
        /// </summary>
        public void Unlock()
        {
            //if (IsLocked.Value)
            if (isLocked)
            {
                StateManager.Global.SetState(IsLockedState.Identifier, new(false));
            }
        }

        void OnStateChanged(in GameState state)
        {
            if (state.Identifier.Equals(IsLockedState.Identifier))
            {
                if (state)
                {
                    onLocked?.Invoke();
                }
                else
                {
                    onUnlocked?.Invoke();
                }
            }
        }

        public void LoadPersistentData(GameSaveData saveData)
        {
            isLockedValue = StateManager.Global.GetOrCreate(IsLockedState.Identifier, new(isLocked));
        }
    }
}