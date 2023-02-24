using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static StoryFramework.Utilities.GameStateUtilities;

namespace StoryFramework
{
    /// <summary>
    /// A composite component, adding multiple functionality for standard scenario.
    /// Handles basic interaction of clicking, hoovering, picking up and displaying description.
    /// </summary>
    [AddComponentMenu("MoxieJam/StoryFramework/Objects/Interactable Item")]
    [RequireComponent(typeof(PersistentObject))]
    public class InteractableItem : MonoBehaviour, IPersistentComponent, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public const string PickedUpStateId = "InteractableItem_IsPickedUp";

        [SerializeField]
        InventoryItem item;

        [SerializeField]
        string customDescription;

        [SerializeField]
        bool showDescriptionOnClick;

        [SerializeField]
        bool showDescriptionOnMouseOver;

        [SerializeField]
        bool pickUpOnClick;

        [SerializeField]
        UnityEvent<InventoryItem> onMouseEnter;

        [SerializeField]
        UnityEvent<InventoryItem> onMouseExit;

        [SerializeField]
        UnityEvent<InventoryItem> onMouseClick;

        [SerializeField]
        UnityEvent<InventoryItem> onPickUp;

        public GameState IsPickedUpState => new() { Identifier = GetIdentifier(this, PickedUpStateId), Value = new(isPickedUp) };
        bool isPickedUp;

        /// <summary>
        /// Available states on this persistent object.
        /// </summary>
        public GameStateIdentifier[] GameStates => new[] { IsPickedUpState.Identifier };

        void Start()
        {
            gameObject.SetActive(!IsPickedUpState);
            StateManager.Global.OnStateChanged += OnStateChanged;
        }

        private void OnDestroy()
        {
            StateManager.Global.OnStateChanged -= OnStateChanged;
        }

        /// <summary>
        /// Handel hoover start.
        /// Depending o settings, it can show description or just call a <see cref="onMouseEnter">mouse enter event</see>. 
        /// </summary>
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (showDescriptionOnMouseOver)
            {
                ShowDescription();
            }

            onMouseEnter.Invoke(item);
        }

        /// <summary>
        /// Handel hoover exit.
        /// Depending o settings, it can clear description or just call a <see cref="onMouseExit">mouse exit event</see>. 
        /// </summary>
        public void OnPointerExit(PointerEventData eventData)
        {
            if (showDescriptionOnMouseOver)
            {
                ClearDescription();
            }

            onMouseExit.Invoke(item);
        }

        /// <summary>
        /// Handel pointer clicking.
        /// Depending o settings, it can show description, pick up an item or just call a <see cref="onMouseClick">mouse click event</see>. 
        /// </summary>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (showDescriptionOnClick)
            {
                ShowDescription();
            }

            if (pickUpOnClick)
            {
                PickUp();
            }

            onMouseClick.Invoke(item);
        }

        /// <summary>
        /// Enables displaying of the items description.
        /// </summary>
        public void EnableDescription(bool enabled)
        {
            showDescriptionOnClick = enabled;
        }

        /// <summary>
        /// Enables picking up the item.
        /// </summary>
        public void EnablePickUp(bool enabled)
        {
            pickUpOnClick = enabled;
        }

        /// <summary>
        /// Display the description in the dialogue.
        /// </summary>
        public void ShowDescription()
        {
            if (!string.IsNullOrEmpty(customDescription))
            {
                Dialogue.ActiveDialogueText = customDescription; 
            }
            else
            {
                Dialogue.ActiveDialogueText = item.Description; 
            }
        }

        /// <summary>
        /// Clear the description from the dialogue.
        /// </summary>
        void ClearDescription()
        {
            Dialogue.ActiveDialogueText = string.Empty; 
        }

        /// <summary>
        /// Picks up the item (add it to the inventory).
        /// Will call <see cref="onPickUp">Pick Up event</see>.
        /// </summary>
        public void PickUp()
        {
            StateManager.Global.SetState(IsPickedUpState.Identifier, new(true));
            onPickUp.Invoke(item);
            Game.Instance.SaveData.Inventory.Add(item);
            gameObject.SetActive(false);
        }

        void OnStateChanged(in GameState state)
        {
            if (state.Identifier.Equals(IsPickedUpState.Identifier))
            {
            }
        }

        public void LoadPersistentData(GameSaveData saveData)
        {
            isPickedUp = saveData.GetState(this, PickedUpStateId, false);
            if (isPickedUp)
            {
                gameObject.SetActive(false);
            }
        }
    }
}