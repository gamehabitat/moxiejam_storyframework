using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace StoryFramework.Utilities
{
    [AddComponentMenu("MoxieJam/StoryFramework/UI/ScrollRect Utility")]
    public class ScrollRectUtility : MonoBehaviour
    {
        [SerializeField]
        ScrollRect scrollRect;

        [SerializeField]
        GameObject scrollUpButton;

        [SerializeField]
        GameObject scrollDownButton;

        [SerializeField]
        GameObject scrollLeftButton;

        [SerializeField]
        GameObject scrollRightButton;

        [SerializeField]
        float scrollAmount = 0.33f;

        void Start()
        {
            Assert.IsNotNull(scrollRect, "ScrollRect is not set. Please make sure to set the ScrollRect setting to a valid object.");
        }

        public void ScrollUp()
        {
            scrollRect.verticalNormalizedPosition = Mathf.Clamp01(scrollRect.verticalNormalizedPosition + scrollAmount);
        }

        public void ScrollDown()
        {
            scrollRect.verticalNormalizedPosition = Mathf.Clamp01(scrollRect.verticalNormalizedPosition - scrollAmount); 
        }

        public void ScrollLeft()
        {
            scrollRect.horizontalNormalizedPosition = Mathf.Clamp01(scrollRect.horizontalNormalizedPosition - scrollAmount);
        }

        public void ScrollRight()
        {
            scrollRect.horizontalNormalizedPosition = Mathf.Clamp01(scrollRect.horizontalNormalizedPosition + scrollAmount); 
        }
    }
}