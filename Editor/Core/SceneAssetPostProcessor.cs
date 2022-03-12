using System;
using UnityEditor;

namespace StoryFramework.Editor.Core
{
    public class SceneAssetPostProcessor : AssetPostprocessor
    {
        public delegate void AssetDatabaseScenesModified();

        public static event AssetDatabaseScenesModified OnAssetDatabaseScenesModified;
        
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            //Debug.Log($"OnPostprocessAllAssets(\n{string.Join(", ", importedAssets)}\n{string.Join(", ", deletedAssets)}\n{string.Join(", ", movedAssets)}\n{string.Join(", ", movedFromAssetPaths)})");
            const string sceneFileExtension = ".unity";
            if (Array.Exists(importedAssets, x => x.EndsWith(sceneFileExtension)) ||
                Array.Exists(deletedAssets, x => x.EndsWith(sceneFileExtension)) ||
                Array.Exists(movedAssets, x => x.EndsWith(sceneFileExtension)) ||
                Array.Exists(movedFromAssetPaths, x => x.EndsWith(sceneFileExtension)))
            {
                OnAssetDatabaseScenesModified?.Invoke();
            }
        }
    }
}