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
            if ((!string.IsNullOrEmpty(id)) && Game.Instance && (Game.Instance.SaveData != null))
            {
                switch (value)
                {
                case LockStates.Locked:
                    Game.Instance.SaveData.SetGlobalState<bool>(id, LockableObject.LockedStateId, true);
                    break;
                case LockStates.Unlocked:
                    Game.Instance.SaveData.SetGlobalState<bool>(id, LockableObject.LockedStateId, false);
                    break;
                }
            }
        }

        /// <summary>
        /// Set the state to locked.
        /// </summary>
        public void Lock()
        {
            if ((!string.IsNullOrEmpty(id)) && Game.Instance && (Game.Instance.SaveData != null))
            {
                var lockState = Game.Instance.SaveData.GetGlobalState<bool>(id, LockableObject.LockedStateId);
                if (lockState != null)
                {
                    lockState.Value = true;
                }
            }
        }

        /// <summary>
        /// Set the state to locked.
        /// </summary>
        public void Unlock()
        {
            if ((!string.IsNullOrEmpty(id)) && Game.Instance && (Game.Instance.SaveData != null))
            {
                var lockState = Game.Instance.SaveData.GetGlobalState<bool>(id, LockableObject.LockedStateId);
                if (lockState != null)
                {
                    lockState.Value = false;
                }
            }
        }
    }
}