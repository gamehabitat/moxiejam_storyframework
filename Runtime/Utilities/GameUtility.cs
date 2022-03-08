using System;
using UnityEngine;
using UnityEngine.Events;

namespace StoryFramework.Utilities
{
    /// <summary>
    /// A general utility for controlling the game.
    /// </summary>
    [AddComponentMenu("MoxieJam/StoryFramework/Utilities/Game Utility")]
    public class GameUtility : MonoBehaviour
    {
        /// <summary>
        /// Called when a scene begin to load.
        /// </summary>
        [SerializeField]
        UnityEvent onBeginLoadScene;

        void Start()
        {
            Game.OnBeginLoadScene += OnBeginLoadScene;
        }

        void OnDestroy()
        {
            Game.OnBeginLoadScene -= OnBeginLoadScene;
        }

        void OnBeginLoadScene(string scenename)
        {
            onBeginLoadScene?.Invoke();
        }

        /// <summary>
        /// Quits the current gaming session and return to main menu.
        /// </summary>
        public void QuitToMenu()
        {
            Game.QuitToMenu();
        }

        /// <summary>
        /// Quits the game.
        /// </summary>
        public void QuitGame()
        {
            Game.Quit();
        }
    }
}