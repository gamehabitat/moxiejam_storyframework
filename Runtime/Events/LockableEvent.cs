using UnityEngine;
using UnityEngine.Events;

namespace StoryFramework.Events
{
    /// <summary>
    /// Event listener for a lockable object.
    /// </summary>
    [AddComponentMenu("MoxieJam/StoryFramework/Events/Lockable Event")]
    public class LockableEvent : MonoBehaviour
    {
        /// <summary>
        /// Called when the lock locked.
        /// </summary>
        [SerializeField]
        UnityEvent onLocked;

        /// <summary>
        /// Called when the lock is successfully unlocked.
        /// </summary>
        [SerializeField]
        UnityEvent onUnlocked;

        [SerializeField]
        LockableObject targetLock;

        void Start()
        {
        }
        
        void OnDestroy()
        {
        }

        void OnEnable()
        {
        }

        void OnDisable()
        {
        }

        void OnItemAdded(LockableObject lockableObject)
        {
            onLocked.Invoke();
        }

        void OnItemRemoved(LockableObject lockableObject)
        {
            onUnlocked.Invoke();
        }
    }
}