using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace StoryFramework
{
    /// <summary>
    /// Invoke a event with the specified item when mouse clicks a object.
    /// </summary>
    /// <remarks>
    /// Criteria for this event is implementation dependent. For example see StandAloneInputModule.
    /// </remarks>
    [AddComponentMenu("MoxieJam/StoryFramework/Click Item Event")]
    public class ClickItem : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        InventoryItem item;

        [SerializeField]
        UnityEvent<InventoryItem> onClick;

        public void OnPointerClick(PointerEventData eventData)
        {
            onClick.Invoke(item);
        }
    }
}