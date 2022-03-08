using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEditor.SceneTemplate;
using UnityEngine;

namespace StoryFramework.Editor
{
    public class GameSceneTemplatePipeline : ISceneTemplatePipeline
    {
        public virtual bool IsValidTemplateForInstantiation(SceneTemplateAsset sceneTemplateAsset)
        {
            return true;
        }

        public virtual void BeforeTemplateInstantiation(SceneTemplateAsset sceneTemplateAsset, bool isAdditive, string sceneName)
        {

        }

        public virtual void AfterTemplateInstantiation(SceneTemplateAsset sceneTemplateAsset, Scene scene, bool isAdditive, string sceneName)
        {
            EditorSceneManager.SetActiveScene(scene);
            var root = scene.GetRootGameObjects();
            foreach (var gameObject in root)
            {
                if (gameObject.TryGetComponent<InGameScene>(out var inGameScene))
                {
                    Debug.Log(inGameScene.name);
                }
            }
        }
    }
}