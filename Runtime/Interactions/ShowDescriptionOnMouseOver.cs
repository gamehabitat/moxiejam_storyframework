using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StoryFramework
{
    /// <summary>
    /// Shows a descriptive text when mouse is over the object.
    /// </summary>
    /// <remarks>
    /// Criteria for this event is implementation dependent. For example see StandAloneInputModule.
    /// </remarks>
    [AddComponentMenu("MoxieJam/StoryFramework/Interactions/Show Description On Mouse Over")]
    public class ShowDescriptionOnMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        /// <summary>
        /// Description text to show.
        /// </summary>
        [SerializeField]
        string description;

        void Start()
        {
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Dialogue.ActiveDialogueText = description; 
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Dialogue.ActiveDialogueText = string.Empty;
        }
    }
}