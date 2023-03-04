using StoryFramework;
using UnityEngine;
using UnityEngine.Events;

namespace StoryFramework.Utilities
{
    /// <summary>
    /// A script to check if we have some items in the inventory.
    /// </summary>
    [AddComponentMenu("MoxieJam/StoryFramework/Utilities/Do If Inventory Contains Items")]
	
	public class DoIfInventoryContainsItems : MonoBehaviour
	{
		/// <summary>
		/// The items required to exist in the inventory for the event to happen.
		/// </summary>
		[SerializeField]
		InventoryItem[] requiredItems;

		/// <summary>
		/// An event that is called once the inventory contain all items.
		/// </summary>
		[SerializeField]
		UnityEvent onHaveAllRequiredItems;
		
		// Find the inventory.
		Inventory inventory => Game.Instance ? Game.Instance.SaveData?.Inventory : null;

		// Called when script is activated.
		void Start()
		{
		}

		// Called when this script is activated.
		void OnEnable()
		{
			// Make sure we have found the inventory before doing anything else here.
			if (inventory != null)
			{
				// Listen for items added to inventory.
				inventory.OnItemAdded += OnItemAdded;
			}
		}

		// Called when this script is deactivated.
		void OnDisable()
		{
			// Make sure we have found the inventory before doing anything else here.
			if (inventory != null)
			{
				// The script is asleep so ignore listening to items added to inventory.
				inventory.OnItemAdded -= OnItemAdded;
			}
		}

		// Called every time a item is added to the inventory.
		void OnItemAdded(InventoryItem item)
		{
			// Make sure we have a list of items before doing anything else.
			if (requiredItems != null)
			{
				// Used to count the number of items we've found that match the required items.
				int numberOfRequiredItemsOwned = 0;
				
				// Go through all required items.
				for (int i = 0; i < requiredItems.Length; ++i)
				{
					// Is this required item in the inventory.
					if (inventory.Contains(item))
					{
						// Yay! We have one of the required items. Increase the counter.
						numberOfRequiredItemsOwned++;
					}
				}
				
				// Check if we have all the items by comparing found
				// requited items with the actual number of required items.
				if (numberOfRequiredItemsOwned == requiredItems.Length)
				{
					// Yay! We got them all. Now do something to celebrate.
					onHaveAllRequiredItems?.Invoke();
				}
			}
		}
	}
}