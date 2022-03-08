using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace StoryFramework
{
    /// <summary>
    /// A representation of an item in the game.
    /// </summary>
    [CreateAssetMenu(fileName = "Item", menuName = "MoxieJam/StoryFramework/Item")]
    public class InventoryItem : ScriptableObject
    {
        /// <summary>
        /// The name of the item.
        /// </summary>
        [SerializeField]
        public string Name;

        /// <summary>
        /// This text is displayed in the dialogue text field.
        /// </summary>
        [SerializeField]
        public string Description;

        /// <summary>
        /// Image to display in the inventory view.
        /// </summary>
        [SerializeField]
        public Sprite Icon;
    }
}