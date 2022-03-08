using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace StoryFramework
{
    public class CursorHandler : MonoBehaviour
    {
        public delegate void ItemDroppedEvent(InventoryItem item);

        [SerializeField]
        Texture2D defaultCursor;

        [SerializeField]
        Image dragIcon;

        Camera mainCamera;
        DropItemTarget currentDropTarget;

        public Texture2D Current { get; private set; }
        public InventoryItem ItemDragged { get; private set; }
        public event ItemDroppedEvent OnItemDropped;

        void Awake()
        {
            //Cursor.lockState = CursorLockMode.Confined;
            //SetCursor(defaultCursor);
        }

        void Start()
        {
            mainCamera = Camera.main;
            /*if (Cursor.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            {
                spriteRenderer.sortingLayerID = SortingLayer.NameToID("Cursor");
                spriteRenderer.sortingOrder = 1000;
            }*/
        }

        public void SetCursor(Texture2D newCursor)
        {
            SetCursor(newCursor, Vector2.zero);
        }

        public void SetCursor(Texture2D newCursor, Vector2 hotspot)
        {
            Current = newCursor;
            Cursor.SetCursor(newCursor, hotspot, CursorMode.Auto);
        }

        /// <summary>
        /// Start dragging a item.
        /// </summary>
        public void DragItem(InventoryItem item)
        {
            Assert.IsNotNull(item, "Tried dragging a invalid item. Please make sure you have valid item before calling DragItem.");
            if (item == null)
            {
                return;
            }

            if (ItemDragged)
            {
                StopDraggingItem();
            }

            ItemDragged = item;
            dragIcon.sprite = item.Icon;
            dragIcon.preserveAspect = true;
            dragIcon.enabled = true;
        }

        /// <summary>
        /// Stop dragging current item.
        /// </summary>
        public void StopDraggingItem()
        {
            if (ItemDragged)
            {
                dragIcon.sprite = null;
                dragIcon.enabled = false;
                if (currentDropTarget && currentDropTarget.TryDropItem(ItemDragged))
                {
                    Game.Instance.SaveData.Inventory.Enable(ItemDragged);
                }
                ClearDropTarget(currentDropTarget);

                OnItemDropped?.Invoke(ItemDragged);
                ItemDragged = null;
            }
        }

        /// <summary>
        /// Set active drop target.
        /// </summary>
        /// <param name="dropItemTargetTarget">Current target we can drop an item on.</param>
        public void SetDropTarget(DropItemTarget dropItemTargetTarget)
        {
            currentDropTarget = dropItemTargetTarget;
        }

        /// <summary>
        /// Clear the drop target if the specified target is the current target.
        /// </summary>
        /// <param name="dropItemTargetTarget">The drop target to clear.</param>
        public void ClearDropTarget(DropItemTarget dropItemTargetTarget)
        {
            if (currentDropTarget == dropItemTargetTarget)
            {
                currentDropTarget = null;
            }
        }

        void Update()
        {
            if (ItemDragged && Input.GetMouseButtonUp(0))
            {
                StopDraggingItem();
                return;
            }

            // Update dragging item position.
            var myTransform = transform;
            var mouseScreenPos = Input.mousePosition;
            var mouseToWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);
            mouseToWorldPos.z = myTransform.position.z;
            myTransform.position = mouseToWorldPos;
            //dragIcon.transform.position = mouseScreenPos;
        }
    }
}