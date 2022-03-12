using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StoryFramework.Editor
{
    [InitializeOnLoad]
    public class GameEditor
    {
        static GameEditor()
        {
            // Make sure the play scene is the first file in the build list.
            SceneAsset startScene = AssetDatabase.LoadMainAssetAtPath(SceneUtility.GetScenePathByBuildIndex(0)) as SceneAsset;
            EditorSceneManager.playModeStartScene = startScene;

            // React to build settings change.
            EditorBuildSettings.sceneListChanged += OnSceneListChanged;
            
            // React to build scene load and unload.
            EditorSceneManager.sceneLoaded += OnSceneLoaded;
            EditorSceneManager.sceneUnloaded += OnSceneUnloaded;

            SetupSortingLayers();
        }

        static void SetupSortingLayers()
        {
            CreateSortingLayer("Background", 0);
            CreateSortingLayer("Foreground");
            CreateSortingLayer("UI");
            CreateSortingLayer("Cursor");
        }

        /// <summary>
        /// Adds a sorting layer at index in sorting layers list.
        /// </summary>
        /// <param name="layerName">Name of the layer</param>
        /// <param name="index">Where in the sorting to put it, or -1 to add to end.</param>
        static void CreateSortingLayer(string layerName, int index = -1)
        {
            var serializedObject = new SerializedObject(AssetDatabase.LoadMainAssetAtPath("ProjectSettings/TagManager.asset"));
            var sortingLayers = serializedObject.FindProperty("m_SortingLayers");
            for (int i = 0; i < sortingLayers.arraySize; i++)
            {
                if (sortingLayers.GetArrayElementAtIndex(i).FindPropertyRelative("name").stringValue.Equals(layerName, StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }
            }

            // Make sure index is valid.
            if (index < 0)
            {
                index = sortingLayers.arraySize;
            }
            index = Mathf.Min(index, sortingLayers.arraySize);

            // Add layer.
            sortingLayers.InsertArrayElementAtIndex(index);
            var newLayer = sortingLayers.GetArrayElementAtIndex(index);
            newLayer.FindPropertyRelative("name").stringValue = layerName;
            newLayer.FindPropertyRelative("uniqueID").intValue = layerName.GetHashCode(); /* some unique number */
            serializedObject.ApplyModifiedProperties();
        }

        static void OnSceneListChanged()
        {
            // Make sure the play scene is the first file in the build list.
            SceneAsset startScene = AssetDatabase.LoadMainAssetAtPath(SceneUtility.GetScenePathByBuildIndex(0)) as SceneAsset;
            EditorSceneManager.playModeStartScene = startScene;
        }

        static void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
        }

        static void OnSceneUnloaded(Scene scene)
        {
        }
    }

    /*[InitializeOnLoad]
    public class ObjectDatabaseEditor : AssetModificationProcessor
    {
        static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions options)
        {
            Debug.Log($"deleting {assetPath}");
            return AssetDeleteResult.DidNotDelete;
        }

        static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
        {
            Debug.Log($"moving {sourcePath} to {destinationPath}");
            return AssetMoveResult.DidNotMove;
        }
    }*/
}