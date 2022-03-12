using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace StoryFramework
{
    /// <summary>
    /// A drop item target accepting a single item.
    /// </summary>
    [AddComponentMenu("MoxieJam/StoryFramework/Interactions/Drop Item Target")]
    public class DropItemTarget : MonoBehaviour, IDropItemTarget
    {
        /// <summary>
        /// If we should remove the item from inventory when dropping.
        /// </summary>
        [SerializeField]
        bool removeItemFromInventory = true;

        /// <summary>
        /// Requited item for this drop target.
        /// </summary>
        [FormerlySerializedAs("ExprectedItem")]
        [SerializeField]
        InventoryItem requiredItem;

        /// <summary>
        /// Requited amount of the item for this drop target.
        /// </summary>
        [FormerlySerializedAs("AmountNeeded")]
        [SerializeField]
        int requiredAmount = 1;

        [SerializeField]
        UnityEvent<InventoryItem> onItemAccepted;

        [SerializeField]
        UnityEvent<InventoryItem> onItemRejected;
        
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
            bool itemAccepted = (!requiredItem) ||
                                (itemRecord == null) ||
                                ((item == requiredItem) && (itemRecord.Amount >= requiredAmount));

            if (itemAccepted)
            {
                if (removeItemFromInventory)
                {
                    Game.Instance.SaveData.Inventory.Remove(item, requiredAmount);
                }
                onItemAccepted.Invoke(item);
            }
            else
            {
                onItemRejected.Invoke(itemRecord.Item);
            }

            return itemAccepted;
        }
    }
}