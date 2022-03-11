using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace StoryFramework
{
    /// <summary>
    /// A UI element to help with rendering the inventory inside the UI.
    /// Uses InventoryItemSlot to render the items. 
    /// </summary>
    [AddComponentMenu("MoxieJam/StoryFramework/UI/Inventory View")]
    public class InventoryView : MonoBehaviour
    {
        [SerializeField]
        IngameUI inGameUI;
        
        [SerializeField]
        Transform contentRoot;

        [SerializeField]
        GameObject inventoryItemPrefab;

        Inventory inventory;

        void Start()
        {
            inventory = Game.Instance ? Game.Instance.SaveData?.Inventory : null;
            if (inventory != null)
            {
                UpdateView();
                inventory.OnUpdated += UpdateView;
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
        /// Iterates through all the current inventory items and update the inventory item slots.
        /// </summary>
        void UpdateView()
        {
            int index = 0;

            if (inventory == null)
            {
                for (index = 0; index < contentRoot.childCount; ++index)
                {
                    var child = contentRoot.GetChild(index);
                    child.gameObject.SetActive(false);
                }
                return;
            }

            // Update slot view.
            index = 0;
            foreach (var item in inventory.Items)
            {
                // If we don't have enough slot to cover this item, add one more slot.
                if (contentRoot.childCount <= index)
                {
                    // Create a new inventory item slot.
                    Instantiate(inventoryItemPrefab, contentRoot, false);
                }

                // Get the slot.
                var child = contentRoot.GetChild(index);
                if (!child.TryGetComponent<InventoryItemSlot>(out var slot))
                {
                    Debug.LogError($"Inventory slot {index.ToString()} is missing a InventoryItemSlot component");
                    slot = child.gameObject.AddComponent<InventoryItemSlot>();
                }

                // Set the slot as active.
                slot.Initialize(inGameUI, item.Item, item.Amount);
                slot.gameObject.SetActive(true);

                index++;
            }

            // Disable any unused slots.
            for (; index < contentRoot.childCount; ++index)
            {
                var child = contentRoot.GetChild(index);
                child.gameObject.SetActive(false);
            }
        }
    }
}