using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StoryFramework
{
    /// <summary>
    /// Shows a descriptive text when mouse clicks the object.
    /// </summary>
    /// <remarks>
    /// Criteria for this event is implementation dependent. For example see StandAloneInputModule.
    /// </remarks>
    [AddComponentMenu("MoxieJam/StoryFramework/Interactions/Show Description On Mouse Click")]
    public class ShowDescriptionOnClick : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        string description;

        void Start()
        {
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Dialogue.ActiveDialogue.SetText(description);
        }
    }
}