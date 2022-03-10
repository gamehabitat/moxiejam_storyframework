using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace StoryFramework.Utilities
{
    /// <summary>
    /// Utility for modifying the dialogue.
    /// </summary>
    [AddComponentMenu("MoxieJam/StoryFramework/Utilities/Dialogue Utility")]
    public class DialogueUtility : MonoBehaviour
    {
        /// <summary>
        /// Sets the specified dialogue as the currently active dialogue that handles text.
        /// </summary>
        /// <param name="dialogue">Dialogue to use.</param>
        public void SetActiveDialogue(Dialogue dialogue)
        {
            Assert.IsNotNull(dialogue, "The specified dialogue is invalid. Please specify a valid dialogue.");
            Dialogue.ActiveDialogue = dialogue;
        }

        /// <summary>
        /// Clears the currently active dialogue.
        /// </summary>
        public void ClearDialogue()
        {
            Assert.IsNotNull(Dialogue.ActiveDialogue, "Currently there's no active dialogue. Please assign a active dialogue.");
            Dialogue.ActiveDialogue.Clear();
        }

        /// <summary>
        /// Set the text of the currently active dialogue.
        /// </summary>
        public void SetDialogue(string text)
        {
            Assert.IsNotNull(Dialogue.ActiveDialogue, "Currently there's no active dialogue. Please assign a active dialogue.");
            Dialogue.ActiveDialogue.SetText(text);
        }

        /// <summary>
        /// Appends text to the currently active dialogue.
        /// </summary>
        public void AddToDialogue(string text)
        {
            Assert.IsNotNull(Dialogue.ActiveDialogue, "Currently there's no active dialogue. Please assign a active dialogue.");
            Dialogue.ActiveDialogue.AppendText(text);
        }
    }
}