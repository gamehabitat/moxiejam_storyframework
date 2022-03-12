using UnityEngine;
using UnityEngine.Events;

namespace StoryFramework.Utilities
{
    /// <summary>
    /// Executes a event if it receive a specified item.
    /// </summary>
    [AddComponentMenu("MoxieJam/StoryFramework/Utilities/Do If Item Is")]
    public class DoIfItemIs : MonoBehaviour
    {
        /// <summary>
        /// The required item.
        /// </summary>
        [SerializeField]
        InventoryItem requiredItem;

        /// <summary>
        /// The event you want to do if it's the correct item.
        /// </summary>
        [SerializeField]
        UnityEvent<InventoryItem> eventToDoIfAccepted;

        /// <summary>
        /// The event you want to do if it's the wrong item.
        /// </summary>
        [SerializeField]
        UnityEvent<InventoryItem> eventToDoIfRejected;

        /// <summary>
        /// Try doing the event.
        /// </summary>
        /// <param name="item">Item to test against.</param>
        public void TryDo(InventoryItem item)
        {
            if (requiredItem == item)
            {
                eventToDoIfAccepted.Invoke(item);
            }
            else
            {
                eventToDoIfRejected.Invoke(item);
            }
        }
    }
}