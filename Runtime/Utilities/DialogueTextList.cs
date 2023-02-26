using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;
using static StoryFramework.Utilities.GameStateUtilities;

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

        /// <summary>
        /// Available states on this persistent object.
        /// </summary>
        public GameStateProperty[] GameStateProperties => new[]
        {
            new GameStateProperty(CurrentTextIndexStateId, GameStateTypes.IntegerNumber),
            new GameStateProperty(AdvanceToNextOnShowTextStateId, GameStateTypes.BooleanFlag),
            new GameStateProperty(LoopTextsStateId, GameStateTypes.BooleanFlag)
        };

        GameState CurrentTextIndexState => new(GetIdentifier(this, in GameStateProperties[0]), 0);
        GameState AdvanceToNextOnShowTextValueState => new(GetIdentifier(this, in GameStateProperties[1]), advanceToNextOnShowText);
        GameState LoopTextsValueState => new(GetIdentifier(this, in GameStateProperties[2]), loopTexts);

        private void Start()
        {
        }

        /// <summary>
        /// Reset current text index back to the first text in the list.
        /// </summary>
        public void ResetToFirstText()
        {
            StateManager.Global.SetState(CurrentTextIndexState.Identifier, 0);
        }

        /// <summary>
        /// Advance the current text index to the next text, and if looping is enabled it will wrap around to the first text on the end.
        /// </summary>
        public void AdvanceToNextText()
        {
            CurrentTextIndexState.SetValue(CurrentTextIndexState.IntegerValue + 1);
            StateManager.Global.SetState(CurrentTextIndexState.Identifier, CurrentTextIndexState.IntegerValue);

            if (CurrentTextIndexState.IntegerValue == texts.Length)
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
            if (texts.Length > CurrentTextIndexState.IntegerValue)
            {
                Dialogue.ActiveDialogueText = texts[CurrentTextIndexState.IntegerValue];
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
        }
    }
}