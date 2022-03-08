using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace StoryFramework
{
    /// <summary>
    /// A graphical "slot" for a inventory item used by the inventory view to display the inventory items.
    /// </summary>
    public class InventoryItemSlot : Selectable, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField]
        Image icon;

        [SerializeField]
        TextMeshProUGUI amount;

        [SerializeField]
        UnityEvent onClick;

        [SerializeField]
        UnityEvent onBeginDrag;

        [SerializeField]
        UnityEvent onEndDrag;

        IngameUI inGameUI;
        InventoryItem Item;

        public void Initialize(IngameUI ui, InventoryItem item, int itemAmount)
        {
            Assert.IsNotNull(item, "Item is invalid. Please make sure you initialize the slot with a valid item.");
            Assert.IsNotNull(item, "Item is invalid. Please make sure you initialize the slot with a valid item.");

            inGameUI = ui;
            Item = item;
            icon.sprite = Item.Icon;
            icon.preserveAspect = true;
            icon.gameObject.SetActive(true);
            interactable = itemAmount > 0;
            if (amount)
            {
                amount.text = itemAmount.ToString();
                amount.gameObject.SetActive(itemAmount > 1);
            }
        }

        void SetDescription(string text)
        {
            Dialogue.ActiveDialouge.SetText(text);
        }

        void ClearDescription()
        {
            SetDescription(string.Empty);
        }

        void ShowDescription()
        {
            if (Item)
            {
                SetDescription(Item.Description);
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if (Item)
            {
                Game.Instance.SaveData.Inventory.OnItemEnabled += OnInventoryItemEnabled;
                Game.Instance.SaveData.Inventory.OnItemDisabled += OnInventoryItemDisabled;

                var itemRecord = Game.Instance.SaveData.Inventory.Find(Item);
                interactable = (itemRecord != null) && itemRecord.Amount > 0;
            }
        }

        protected override void OnDisable()
        {
            if (Item)
            {
                if (Game.Instance && (Game.Instance.SaveData != null))
                {
                    Game.Instance.SaveData.Inventory.OnItemEnabled -= OnInventoryItemEnabled;
                    Game.Instance.SaveData.Inventory.OnItemDisabled -= OnInventoryItemDisabled;
                }

                interactable = false;
                inGameUI = null;
                Item = null;
                icon.sprite = null;
                icon.gameObject.SetActive(false);

                if (amount)
                {
                    amount.gameObject.SetActive(false);
                    amount.text = string.Empty;
                }
            }

            base.OnDisable();
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            if (IsHighlighted())
            {
                ShowDescription();
            }
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            if (IsHighlighted())
            {
                ClearDescription();
            }

            base.OnPointerExit(eventData);
        }

        private void Press()
        {
            if (!IsActive() || !IsInteractable())
            {
                return;
            }

            onClick.Invoke();
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            Press();
        }

        bool MayDrag(PointerEventData eventData)
        {
            return IsActive() && IsInteractable() && eventData.button == PointerEventData.InputButton.Left;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!MayDrag(eventData))
            {
                return;
            }

            onBeginDrag.Invoke();

            if (Item != null)
            {
                ClearDescription();

                interactable = false;
                inGameUI.CursorHandler.OnItemDropped += OnItemDropped;
                inGameUI.CursorHandler.DragItem(Item);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!MayDrag(eventData))
            {
                return;
            }

            Debug.Log($"OnDrag {gameObject.name}");
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!MayDrag(eventData))
            {
                return;
            }

            onEndDrag.Invoke();
            Debug.Log($"OnEndDrag {gameObject.name}");
        }

        void OnItemDropped(InventoryItem item)
        {
            if ((Item == null) || (item != Item))
            {
                return;
            }

            var itemRecord = Game.Instance.SaveData.Inventory.Find(Item);
            interactable = (itemRecord != null) && itemRecord.Amount > 0;
            inGameUI.CursorHandler.OnItemDropped -= OnItemDropped;
        }

        void OnInventoryItemEnabled(InventoryItem item)
        {
            if ((Item == null) || (item != Item))
            {
                return;
            }

            var itemRecord = Game.Instance.SaveData.Inventory.Find(Item);
            interactable = (itemRecord != null) && itemRecord.Amount > 0;
            if (amount)
            {
                if (itemRecord != null)
                {
                    amount.text = itemRecord.Amount.ToString();
                    amount.gameObject.SetActive(itemRecord.Amount > 1);
                }
                else
                {
                    amount.text = string.Empty;
                    amount.gameObject.SetActive(false);
                }
            }
        }

        void OnInventoryItemDisabled(InventoryItem item)
        {
            if ((Item == null) || (item != Item))
            {
                return;
            }

            var itemRecord = Game.Instance.SaveData.Inventory.Find(Item);
            interactable = (itemRecord != null) && itemRecord.Amount > 0;
        }
    }
}