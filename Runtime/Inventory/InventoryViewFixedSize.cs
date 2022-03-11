using System;
using UnityEngine;

namespace StoryFramework
{
    /// <summary>
    /// A version of inventory view with a fixed number of slots. 
    /// </summary>
    [AddComponentMenu("MoxieJam/StoryFramework/UI/Inventory Fixed Size")]
    public class InventoryViewFixedSize : MonoBehaviour
    {
        [SerializeField]
        IngameUI inGameUI;

        [SerializeField]
        InventoryItemSlot[] itemSlots;
        
        int currentIndex = 0;
        Inventory inventory;

        void Start()
        {
            inventory = Game.Instance ? Game.Instance.SaveData?.Inventory : null;
            if (inventory != null)
            {
                inventory.OnUpdated += UpdateView;
                UpdateView();
            }
        }

        void OnDestroy()
        {
            if (inventory != null)
            {
                inventory.OnUpdated -= UpdateView;
            }
        }

        /// <summary>
        /// Show previous item in inventory.
        /// </summary>
        public void ShowPreviousItem()
        {
            int newIndex = Mathf.Clamp(currentIndex - 1, 0, inventory.Count - itemSlots.Length);
            newIndex = Mathf.Max(newIndex, 0);
            if (newIndex != currentIndex)
            {
                currentIndex = newIndex;
                UpdateView();
            }
        }

        /// <summary>
        /// Show next item in inventory.
        /// </summary>
        public void ShowNextItem()
        {
            int newIndex = Mathf.Clamp(currentIndex + 1, 0, inventory.Count - itemSlots.Length);
            newIndex = Mathf.Max(newIndex, 0);
            if (newIndex != currentIndex)
            {
                currentIndex = newIndex;
                UpdateView();
            }
        }

        /// <summary>
        /// Iterates through all the current inventory items and update the inventory item slots.
        /// </summary>
        void UpdateView()
        {
            //currentIndex = Mathf.Min(currentIndex + itemSlots.Length, itemSlots.Length) - itemSlots.Length;
            if (inventory == null)
            {
                foreach (var itemSlot in itemSlots)
                {
                    itemSlot.enabled = false;
                }
                return;
            }
            
            // Update slot view.
            int index = -currentIndex;
            foreach (var item in inventory.Items)
            {
                if (index >= itemSlots.Length)
                {
                    break;
                }

                if (index >= 0)
                {
                    var itemSlot = itemSlots[index];
                    itemSlot.Initialize(inGameUI, item.Item, item.Amount);
                    itemSlot.enabled = true;
                }

                ++index;
            }

            while (index < itemSlots.Length)
            {
                var itemSlot = itemSlots[index];
                itemSlot.enabled = false;
                index++;
            }
        }
    }
}