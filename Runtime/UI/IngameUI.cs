using System;
using TMPro;
using UnityEngine;

namespace StoryFramework
{
    public class IngameUI : MonoBehaviour
    {
        [SerializeField]
        CursorHandler cursorHandler;

        [SerializeField]
        InventoryView inventoryView;

        [SerializeField]
        Dialogue dialogue;

        public CursorHandler CursorHandler => cursorHandler;

        void Start()
        {
            Dialogue.ActiveDialouge = dialogue;
        }
        
    }
}