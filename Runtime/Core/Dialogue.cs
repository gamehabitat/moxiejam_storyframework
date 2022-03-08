using System;
using TMPro;
using UnityEngine;

namespace StoryFramework
{
    /// <summary>
    /// Dialogue handler.
    /// </summary>
    [AddComponentMenu("MoxieJam/StoryFramework/Dialogue")]
    public class Dialogue : MonoBehaviour
    {
        /// <summary>
        /// Currently active dialogue.
        /// </summary>
        public static Dialogue ActiveDialouge;

        /// <summary>
        /// Graphical text target.
        /// </summary>
        [SerializeField]
        TMP_Text dialogueText;
        
        /// <summary>
        /// Property for setting/getting current text on the dialogue.
        /// </summary>
        public string Text
        {
            get
            {
                if (dialogueText)
                {
                    return dialogueText.text;
                }

                return string.Empty;
            }

            set
            {
                if (dialogueText)
                {
                    dialogueText.text = value;
                }
            }
        }

        /// <summary>
        /// Clear the dialogue.
        /// </summary>
        public void Clear()
        {
            Text = string.Empty;
        }

        /// <summary>
        /// Sets the dialogue.
        /// </summary>
        /// <param name="text"></param>
        public void SetText(string text)
        {
            Text = text;
        }

        /// <summary>
        /// Append text to the dialogue.
        /// </summary>
        /// <param name="text"></param>
        public void AppendText(string text)
        {
            Text += text;
        }
    }
}