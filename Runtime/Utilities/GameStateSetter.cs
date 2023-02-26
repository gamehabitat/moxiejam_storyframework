using System;
using UnityEngine;

namespace StoryFramework.Utilities
{
    /// <summary>
    /// A utility for setting a global game state.
    /// </summary>
    [AddComponentMenu("MoxieJam/StoryFramework/Utilities/Game State Setter")]
    public class GameStateSetter : MonoBehaviour
    {
        public enum Types
        {
            Bool,
            Int,
            Float,
            String
        }

        [SerializeField]
        bool setStateOnStart;

        [SerializeField]
        Types stateType;

        [SerializeField]
        string identifier;

        [SerializeField]
        string property;

        [SerializeField]
        bool boolValue;

        [SerializeField]
        int intValue;

        [SerializeField]
        float floatValue;

        [SerializeField]
        string stringValue;

        [SerializeField, GameStateRef]
        GameStateIdentifier gameState;

        void Start()
        {
            if (setStateOnStart)
            {
                SetState();
            }
        }

        /// <summary>
        /// Sets the save state value.
        /// </summary>
        public void SetState()
        {
            if (gameState.IsValid())
            {
                switch (stateType)
                {
                case Types.Bool:
                    StateManager.Global.SetState(in gameState, boolValue);
                    break;
                case Types.Int:
                    StateManager.Global.SetState(in gameState, intValue);
                    break;
                case Types.Float:
                    StateManager.Global.SetState(in gameState, floatValue);
                    break;
                case Types.String:
                    StateManager.Global.SetState(in gameState, stringValue);
                    break;
                }
            }
            
            if ((!string.IsNullOrEmpty(identifier)) && Game.Instance && (Game.Instance.SaveData != null))
            {
                switch (stateType)
                {
                case Types.Bool:
                    Game.Instance.SaveData.SetGlobalState<bool>(identifier, property, boolValue);
                    break;
                case Types.Int:
                    Game.Instance.SaveData.SetGlobalState<int>(identifier, property, intValue);
                    break;
                case Types.Float:
                    Game.Instance.SaveData.SetGlobalState<float>(identifier, property, floatValue);
                    break;
                case Types.String:
                    Game.Instance.SaveData.SetGlobalState<string>(identifier, property, stringValue);
                    break;
                }
            }
        }
    }
}