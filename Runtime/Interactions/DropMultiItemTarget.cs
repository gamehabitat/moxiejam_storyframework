using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace StoryFramework
{
    /// <summary>
    /// A drop item target accepting multiple items.
    /// </summary>
    [AddComponentMenu("MoxieJam/StoryFramework/Interactions/Drop Multi Item Target")]
    public class DropMultiItemTarget : MonoBehaviour, IDropItemTarget
    {
        /// <summary>
        /// If we should remove the item from inventory when dropping.
        /// </summary>
        [SerializeField]
        bool removeItemFromInventory = true;

        /// <summary>
        /// If the target requires all items to be dropped
        /// If this is set the event onDroppedAllItems will invoked when all items have been dropped.
        /// </summary>
        [SerializeField]
        bool needsAllItems = true;

        /// <summary>
        /// Requited items for this drop target.
        /// </summary>
        [SerializeField]
        InventoryItem[] requiredItems;

        [SerializeField]
        UnityEvent<InventoryItem> onItemAccepted;

        [SerializeField]
        UnityEvent onDroppedAllItems;

        [SerializeField]
        UnityEvent<InventoryItem> onItemRejected;

        // A list of items that have been dropped so far.
        private List<InventoryItem> itemsDropped = new(10);

        /// <summary>
        /// Clears the list of dropped items.
        /// </summary>
        public void ClearDroppedItems()
        {
            itemsDropped.Clear();
        }
        
        void OnMouseEnter()
        {
            if (Game.Instance.IngameUi.CursorHandler)
            {
                Game.Instance.IngameUi.CursorHandler.SetDropTarget(this);
            }
        }

        void OnMouseExit()
        {
            if (Game.Instance.IngameUi.CursorHandler)
            {
                Game.Instance.IngameUi.CursorHandler.ClearDropTarget(this);
            }
        }

        ///<inheritdoc />
        public bool TryDropItem(InventoryItem item)
        {
            var itemRecord = Game.Instance.SaveData.Inventory.Find(item);

            // If this drop interaction require a certain item, check that this is the correct item.
            bool itemAccepted = false;
            if ((requiredItems.Length == 0) || (itemRecord == null))
            {
                itemAccepted = true;
            }
            else if (needsAllItems)
            {
                itemAccepted = ((!itemsDropped.Contains(item)) && System.Array.Exists(requiredItems, x => x == item));
            }
            else
            {
                itemAccepted = System.Array.Exists(requiredItems, x => x == item);
            }

            if (itemAccepted)
            {
                if (needsAllItems)
                {
                    itemsDropped.Add(item);
                }
                if (removeItemFromInventory)
                {
                    Game.Instance.SaveData.Inventory.Remove(item, 1);
                }
                onItemAccepted.Invoke(item);
                if (itemsDropped.Count == requiredItems.Length)
                {
                    onDroppedAllItems.Invoke();
                }
            }
            else
            {
                onItemRejected.Invoke(itemRecord.Item);
            }

            return itemAccepted;
        }
    }
}