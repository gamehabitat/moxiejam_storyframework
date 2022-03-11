using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace StoryFramework.Events
{
    /// <summary>
    /// Invoke a event with the specified item when mouse enter or exit a object.
    /// </summary>
    /// <remarks>
    /// Criteria for this event is implementation dependent. For example see StandAloneInputModule.
    /// </remarks>
    [AddComponentMenu("MoxieJam/StoryFramework/Events/Mouse Over Item")]
    public class MouseOverItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        InventoryItem item;

        [SerializeField]
        UnityEvent<InventoryItem> onMouseEnter;

        [SerializeField]
        UnityEvent<InventoryItem> onMouseExit;

        public void OnPointerEnter(PointerEventData eventData)
        {
            onMouseEnter.Invoke(item);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            onMouseExit.Invoke(item);
        }
    }
}