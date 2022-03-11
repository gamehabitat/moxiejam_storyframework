using System;
using TMPro;
using UnityEngine;

namespace StoryFramework
{
    [AddComponentMenu("MoxieJam/StoryFramework/UI/In-game UI")]
    public class IngameUI : MonoBehaviour
    {
        [SerializeField]
        CursorHandler cursorHandler;

        [SerializeField]
        InventoryView inventoryView;

        [SerializeField]
        Dialogue dialogue;

        public CursorHandler CursorHandler => cursorHandler;

        void Awake()
        {
            Dialogue.ActiveDialogue = dialogue;
        }
        
    }
}