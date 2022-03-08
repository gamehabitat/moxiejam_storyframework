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
        string id;

        [SerializeField]
        bool boolValue;

        [SerializeField]
        int intValue;

        [SerializeField]
        float floatValue;

        [SerializeField]
        string stringValue;

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
            if ((!string.IsNullOrEmpty(id)) && Game.Instance && (Game.Instance.SaveData != null))
            {
                switch (stateType)
                {
                case Types.Bool:
                    Game.Instance.SaveData.SetGlobalState(id, boolValue);
                    break;
                case Types.Int:
                    Game.Instance.SaveData.SetGlobalState(id, intValue);
                    break;
                case Types.Float:
                    Game.Instance.SaveData.SetGlobalState(id, floatValue);
                    break;
                case Types.String:
                    Game.Instance.SaveData.SetGlobalState(id, stringValue);
                    break;
                }
            }
        }
    }
}