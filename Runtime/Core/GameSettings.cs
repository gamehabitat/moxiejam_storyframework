using UnityEngine;

namespace StoryFramework
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "MoxieJam/StoryFramework/GameSettings")]
    public class GameSettings : ScriptableObject
    {
        [SerializeField]
        public SceneRef StartScene;
    }
}