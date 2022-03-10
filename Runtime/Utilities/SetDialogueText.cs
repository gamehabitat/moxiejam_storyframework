using System;
using UnityEngine;

namespace StoryFramework.Utilities
{
    /// <summary>
    /// Utility for setting dialogue texts.
    /// </summary>
    [AddComponentMenu("MoxieJam/StoryFramework/Utilities/Set Dialogue Text")]
    public class SetDialogueText : MonoBehaviour
    {
        /// <summary>
        /// Wether to set the text on start..
        /// </summary>
        [SerializeField]
        bool setOnStart;

        /// <summary>
        /// Text to display on start.
        /// </summary>
        [SerializeField]
        string text;

        void Start()
        {
            if (setOnStart)
            {
                SetText(text);
            }
        }

        public void SetText(string dialogueText)
        {
            Dialogue.ActiveDialogue.SetText(dialogueText);
        }
    }
}