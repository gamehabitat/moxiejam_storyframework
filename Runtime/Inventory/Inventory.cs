using System;
using System.Collections.Generic;
using UnityEngine;

namespace StoryFramework
{
    [Serializable]
    public class Inventory
    {
        public delegate void InventoryUpdatedDelegate();
        public delegate void InventoryItemDelegate(InventoryItem item);
        
        /// <summary>
        /// A record of a item in the inventory.
        /// Contains a reference to the item and meta data such as amount of the item in the inventory.
        /// </summary>
        [Serializable]
        public class ItemRecord
        {
            [SerializeField]
            public InventoryItem Item;
            [SerializeField]
            public int Amount;
            [SerializeField]
            public bool Enabled;
        }

        [SerializeField]
        List<ItemRecord> items = new List<ItemRecord>();

        /// <summary>
        /// Active items in the inventory.
        /// </summary>
        public IEnumerable<ItemRecord> Items => items;
        
        /// <summary>
        /// Number of item records in inventory.
        /// </summary>
        public int Count => items.Count;
        
        /// <summary>
        /// Called when the inventory is updated (item added, removed, enabled, disabled, sorted).
        /// </summary>
        public event InventoryUpdatedDelegate OnUpdated;

        /// <summary>
        /// Called when a item is added.
        /// </summary>
        public event InventoryItemDelegate OnItemAdded;

        /// <summary>
        /// Called when a item is removed.
        /// </summary>
        public event InventoryItemDelegate OnItemRemoved;

        /// <summary>
        /// Called when a item is added.
        /// </summary>
        public event InventoryItemDelegate OnItemEnabled;

        /// <summary>
        /// Called when a item is added.
        /// </summary>
        public event InventoryItemDelegate OnItemDisabled;

        /// <summary>
        /// Clears the inventory.
        /// </summary>
        public void Clear()
        {
            items.Clear();
            OnUpdated?.Invoke();
        }

        /// <summary>
        /// Add am item to the inventory,
        /// </summary>
        /// <param name="item">Item to add.</param>
        public void Add(InventoryItem item)
        {
            var inventoryItem = items.Find(x => x.Item == item);
            if (inventoryItem == null)
            {
                inventoryItem = new ItemRecord() { Item = item, Amount = 1, Enabled = true }; 
                items.Add(inventoryItem);
            }
            else
            {
                inventoryItem.Amount++;
            }

            OnItemAdded?.Invoke(inventoryItem.Item);
            OnUpdated?.Invoke();
        }

        /// <summary>
        /// Removes an item from the inventory.
        /// </summary>
        /// <param name="item">Item to remove.</param>
        /// <param name="amount">If we have more than one of the item, remove this amount of it.</param>
        public void Remove(InventoryItem item, int amount)
        {
            // TODO: Remove item.
            var inventoryItem = items.Find(x => x.Item == item);
            if (inventoryItem != null)
            {
                inventoryItem.Amount -= amount;
                if (inventoryItem.Amount == 0)
                {
                    items.Remove(inventoryItem);
                }

                OnItemRemoved?.Invoke(inventoryItem.Item);
                OnUpdated?.Invoke();
            }
        }

        public void Enable(InventoryItem item)
        {
            // TODO:
            OnItemEnabled?.Invoke(item);
        }

        public void Disable(InventoryItem item)
        {
            // TODO:
            OnItemDisabled?.Invoke(item);
        }

        /// <summary>
        /// Test if the inventory contains the specified item.
        /// </summary>
        public bool Contains(InventoryItem item)
        {
            return items.Exists(x => x.Item == item);
        }

        /// <summary>
        /// Looks through the inventory for a item and returns the inventories record of the item (item and metadata such as amount in the inventory).
        /// </summary>
        public ItemRecord Find(InventoryItem item)
        {
            return items.Find(x => x.Item == item);
        }
    }
}