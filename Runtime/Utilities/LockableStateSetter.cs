using System;
using UnityEngine;

namespace StoryFramework.Utilities
{
    /// <summary>
    /// A utility for setting a locks state.
    /// </summary>
    [AddComponentMenu("MoxieJam/StoryFramework/Utilities/Lockable State Setter")]
    public class LockableStateSetter : MonoBehaviour
    {
        [Serializable]
        public enum LockStates
        {
            Locked,
            Unlocked
        }

        [SerializeField, GameStateRef(GameStateTypes.BooleanFlag, LockableObject.LockedStateId)]
        GameStateIdentifier lockStateRef;

        [SerializeField]
        bool setStateOnStart;

        [SerializeField]
        string id;

        [SerializeField]
        LockStates setValue;
        
        void Start()
        {
            if (setStateOnStart)
            {
                SetState(setValue);
            }
        }

        /// <summary>
        /// Sets the save state value.
        /// </summary>
        public void SetState(LockStates value)
        {
            if (lockStateRef.IsValid())
            {
                // TODO: Use property LockableObject.LockedStateId
                Game.Instance.SaveData.stateManager.SetState(in lockStateRef, new GameStateValue(value == LockStates.Locked));
            }
        }

        /// <summary>
        /// Set the state to locked.
        /// </summary>
        public void Lock()
        {
            SetLockState(true);
        }

        /// <summary>
        /// Set the state to locked.
        /// </summary>
        public void Unlock()
        {
            SetLockState(false);
        }

        void SetLockState(bool locked)
        {
            if (lockStateRef.IsValid() && StateManager.Global.Exists(in lockStateRef))
            {
                Game.Instance.SaveData.stateManager.SetState(in lockStateRef, new GameStateValue(locked));
            }
        }
    }
}