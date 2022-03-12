using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace StoryFramework.Utilities
{
    /// <summary>
    /// A utility for showing different texts each time.
    /// </summary>
    [AddComponentMenu("MoxieJam/StoryFramework/Utilities/Dialogue Text List")]
    [RequireComponent(typeof(PersistentObject))]
    public class DialogueTextList : MonoBehaviour, IPersistentComponent
    {
        public const string CurrentTextIndexStateId = "DialogueTextList_CurrentTextIndex";
        public const string AdvanceToNextOnShowTextStateId = "DialogueTextList_AdvanceToNextOnShowText";
        public const string LoopTextsStateId = "DialogueTextList_LoopTexts";

        /// <summary>
        /// The texts to show.
        /// </summary>
        [SerializeField]
        string[] texts;

        /// <summary>
        /// If set to true, it will advance the current text index to the next text, and if looping is enabled it will wrap around to the first text on the end.
        /// </summary>
        [SerializeField]
        bool advanceToNextOnShowText = true;

        /// <summary>
        /// If we want to loop back to first text when we've reached the end. 
        /// </summary>
        [SerializeField]
        bool loopTexts = true;

        /// <summary>
        /// Event that happen when last text have been showed.
        /// </summary>
        [SerializeField]
        UnityEvent onLastTextWasShown;

        private GameStateValue<int> currentTextIndex = new();
        private GameStateValue<bool> advanceToNextOnShowTextValue = new();
        private GameStateValue<bool> loopTextsValue = new(); 

        private void Start()
        {
        }

        /// <summary>
        /// Reset current text index back to the first text in the list.
        /// </summary>
        public void ResetToFirstText()
        {
            currentTextIndex.Value = 0;
        }

        /// <summary>
        /// Advance the current text index to the next text, and if looping is enabled it will wrap around to the first text on the end.
        /// </summary>
        public void AdvanceToNextText()
        {
            currentTextIndex.Value = currentTextIndex + 1;

            if (currentTextIndex == texts.Length)
            {
                onLastTextWasShown.Invoke();

                // Wrap around
                if (loopTexts)
                {
                    ResetToFirstText();
                }
            }
        }

        /// <summary>
        /// Shows the current text in the dialogue.
        /// </summary>
        public void ShowText()
        {
            // Show the text.
            if (texts.Length > currentTextIndex)
            {
                Dialogue.ActiveDialogueText = texts[currentTextIndex];
            }

            // Advance the curren text index
            if (advanceToNextOnShowText)
            {
                AdvanceToNextText();
            }
        }

        /// <summary>
        /// Shows a specific text from the list.
        /// </summary>
        /// <param name="index">The index of the text</param>
        public void ShowTextIndex(int index)
        {
            if (texts.Length > index)
            {
                Dialogue.ActiveDialogueText = texts[index];
            }
        }

        /// <summary>
        /// Shows a random text from the text list.
        /// </summary>
        public void ShowRandomText()
        {
            if (texts.Length > 0)
            {
                Dialogue.ActiveDialogueText = texts[Random.Range(0, texts.Length)];
            }
        }

        public void LoadPersistentData(GameSaveData saveData)
        {
            currentTextIndex = saveData.GetState(this, CurrentTextIndexStateId, 0);
            advanceToNextOnShowTextValue = saveData.GetState(this, AdvanceToNextOnShowTextStateId, advanceToNextOnShowText);
            loopTextsValue = saveData.GetState(this, CurrentTextIndexStateId, loopTexts);
        }
    }
}