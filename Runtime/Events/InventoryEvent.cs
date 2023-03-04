using System;
using UnityEngine;
using UnityEngine.Events;

namespace StoryFramework.Events
{
    /// <summary>
    /// Event listener for the inventory system.
    /// </summary>
    [AddComponentMenu("MoxieJam/StoryFramework/Events/Inventory Event")]
    public class InventoryEvent : MonoBehaviour
    {
        /// <summary>
        /// Called whenever the inventory is updated.
        /// </summary>
        [SerializeField]
        UnityEvent onUpdated;

        /// <summary>
        /// Called when a item is added (or the amount is increased) to the inventory.
        /// </summary>
        [SerializeField]
        UnityEvent<InventoryItem> onItemAdded;

        /// <summary>
        /// Called when a item is removed (or the amount is decreased) from the inventory.
        /// </summary>
        [SerializeField]
        UnityEvent<InventoryItem> onItemRemoved;

        /// <summary>
        /// Called when a item in the inventory is enabled again (e.g. not being dragged). 
        /// </summary>
        [SerializeField]
        UnityEvent onItemEnabled;

        /// <summary>
        /// Called when a item in the inventory is disabled (e.g. being dragged). 
        /// </summary>
        [SerializeField]
        UnityEvent onItemDisabled;
        
        // Find the inventory.
        Inventory inventory => Game.Instance ? Game.Instance.SaveData?.Inventory : null;

        void Start()
        {
        }
        
        void OnDestroy()
        {
        }

        void OnEnable()
        {
            inventory = Game.Instance ? Game.Instance.SaveData?.Inventory : null;
            if (inventory != null)
            {
                inventory.OnUpdated += OnUpdated;
                inventory.OnItemAdded += OnItemAdded;
                inventory.OnItemRemoved += OnItemRemoved;
                inventory.OnItemEnabled += OnItemEnabled;
                inventory.OnItemDisabled += OnItemDisabled;
            }
        }

        void OnDisable()
        {
            inventory = Game.Instance ? Game.Instance.SaveData?.Inventory : null;
            if (inventory != null)
            {
                inventory.OnUpdated -= OnUpdated;
                inventory.OnItemAdded -= OnItemAdded;
                inventory.OnItemRemoved -= OnItemRemoved;
                inventory.OnItemEnabled -= OnItemEnabled;
                inventory.OnItemDisabled -= OnItemDisabled;
            }
        }

        void OnUpdated()
        {
            onUpdated.Invoke();
        }

        void OnItemAdded(InventoryItem item)
        {
            onItemAdded.Invoke(item);
        }

        void OnItemRemoved(InventoryItem item)
        {
            onItemRemoved.Invoke(item);
        }

        void OnItemEnabled(InventoryItem item)
        {
            onItemEnabled.Invoke();
        }

        void OnItemDisabled(InventoryItem item)
        {
            onItemDisabled.Invoke();
        }
    }
}