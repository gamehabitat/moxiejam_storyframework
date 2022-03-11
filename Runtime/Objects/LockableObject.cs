using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

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
        [NonSerialized]
        public GameStateValue<bool> IsLocked = new GameStateValue<bool>();

        public void Start()
        {
            if (IsLocked)
            {
                onStartLocked.Invoke();
            }
            else
            {
                onStartUnlocked.Invoke();
            }

            IsLocked.OnValueModified += OnLockedStateChanged;
        }

        void OnDestroy()
        {
            IsLocked.OnValueModified -= OnLockedStateChanged;
        }

        /// <summary>
        /// Try to use the locked object.
        /// Will call the <see cref="onUseLocked">Use Locked</see> event if object is locked. 
        /// Otherwise it will call the <see cref="onUseUnlocked">Use Unlocked</see> event if object is unlocked. 
        /// </summary>
        public void Use()
        {
            if (IsLocked)
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
            if (!IsLocked)
            {
                IsLocked.Value = true;
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
                IsLocked.Value = false;
            }
        }

        void OnLockedStateChanged()
        {
            if (IsLocked)
            {
                onLocked?.Invoke();
            }
            else
            {
                onUnlocked?.Invoke();
            }
        }
        
        public void LoadPersistentData(GameSaveData saveData)
        {
            IsLocked = saveData.GetState(this, LockedStateId, isLocked);
        }
    }
}