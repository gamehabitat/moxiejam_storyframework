using UnityEngine;

namespace StoryFramework.Utilities
{
    /// <summary>
    /// A utility for setting a active state on persistent objects.
    /// </summary>
    [AddComponentMenu("MoxieJam/StoryFramework/Utilities/Active State Setter")]
    public class ActiveStateSetter : MonoBehaviour
    {
        [SerializeField]
        bool setStateOnStart;

        [SerializeField]
        string id;

        [SerializeField]
        bool isActive;
        
        [SerializeField, GameStateRef(GameStateTypes.BooleanFlag, PersistentObject.IsActiveStateId)]
        GameStateIdentifier gameState;

        void Start()
        {
            if (setStateOnStart)
            {
                SetActive(isActive);
            }
        }

        /// <summary>
        /// Sets the active state value.
        /// </summary>
        public void SetActive(bool value)
        {
            if (gameState.IsValid())
            {
                StateManager.Global.SetState(in gameState, value);
            }

            if ((!string.IsNullOrEmpty(id)) && Game.Instance && (Game.Instance.SaveData != null))
            {
                Game.Instance.SaveData.SetGlobalState<bool>(id, PersistentObject.IsActiveStateId, value);
            }
        }
    }
}