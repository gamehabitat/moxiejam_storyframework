using System;
using UnityEngine;

namespace StoryFramework
{
    [Serializable]
    public struct SceneRef
    {
        [SerializeField]
        private string sceneGUID;

        [SerializeField]
        public string sceneName;
    }
}