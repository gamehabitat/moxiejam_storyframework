using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace StoryFramework.Editor
{
    [CustomPropertyDrawer(typeof(SceneRef))]
    public class SceneRefEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.serializedObject.Update();
            var sceneGUIDProp = property.FindPropertyRelative("sceneGUID");
            var sceneNameProp = property.FindPropertyRelative("sceneName");
            
            var sceneAsset = EditorGUI.ObjectField(position, label, GetSceneObject(sceneGUIDProp.stringValue), typeof(SceneAsset), false) as SceneAsset;
            if (sceneAsset != null)
            {
                var sceneGuid = AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(sceneAsset));
                if (sceneGuid.ToString() != sceneGUIDProp.stringValue)
                {
                    sceneGUIDProp.stringValue = sceneGuid.ToString();
                }
                else
                {
                    var sceneBuildSettings = Array.Find(EditorBuildSettings.scenes, x => x.guid == sceneGuid);
                    if ((sceneBuildSettings != null) && (sceneNameProp.stringValue != sceneGUIDProp.stringValue))
                    {
                        sceneNameProp.stringValue = Path.GetFileNameWithoutExtension(sceneBuildSettings.path);
                    }
                }

            }
            else
            {
                sceneGUIDProp.stringValue = String.Empty;
                sceneNameProp.stringValue = String.Empty;
            }
            property.serializedObject.ApplyModifiedProperties();
        }
        
        SceneAsset GetSceneObject(string sceneAssetGuid)
        {
            if (string.IsNullOrEmpty(sceneAssetGuid))
            {
                return null;
            }

            foreach (var editorScene in EditorBuildSettings.scenes)
            {
                if (editorScene.guid.ToString().Equals(sceneAssetGuid))
                {
                    return AssetDatabase.LoadAssetAtPath(editorScene.path, typeof(SceneAsset)) as SceneAsset;
                }
            }

            var sceneAssetPath = AssetDatabase.GUIDToAssetPath(sceneAssetGuid);
            if (string.IsNullOrEmpty(sceneAssetPath))
            {
                Debug.LogWarning($"Scene with guid \"{sceneAssetGuid}\" cannot be found.");
                return null;
            }
            
            var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(sceneAssetPath);
            if (sceneAsset)
            {
                var sceneList = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
                sceneList.Add(new EditorBuildSettingsScene(sceneAssetPath, true));
                EditorBuildSettings.scenes = sceneList.ToArray();
            }
            else
            {
                Debug.LogWarning($"Failed to load scene \"{sceneAssetPath}\". Please make sure the file exist.");
            }

            return sceneAsset;
        }
    }

    [CustomPropertyDrawer(typeof(SceneRefAttribute))]
    public class SceneRefAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                var sceneAsset = EditorGUI.ObjectField(position, label, GetSceneObject(property.stringValue), typeof(SceneAsset), false) as SceneAsset;
                if (sceneAsset != null)
                {
                    var sceneGuid = AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(sceneAsset));
                    if (sceneGuid.ToString() != property.stringValue)
                    {
                        property.stringValue = sceneGuid.ToString();
                    }
                }
                else
                {
                    property.stringValue = String.Empty;
                }
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Use [SceneRef] with strings.");
            }
        }

        SceneAsset GetSceneObject(string sceneAssetGuid)
        {
            if (string.IsNullOrEmpty(sceneAssetGuid))
            {
                return null;
            }

            foreach (var editorScene in EditorBuildSettings.scenes)
            {
                if (editorScene.guid.ToString().Equals(sceneAssetGuid))
                {
                    return AssetDatabase.LoadAssetAtPath(editorScene.path, typeof(SceneAsset)) as SceneAsset;
                }
            }

            var sceneAssetPath = AssetDatabase.GUIDToAssetPath(sceneAssetGuid);
            if (string.IsNullOrEmpty(sceneAssetPath))
            {
                Debug.LogWarning($"Scene with guid \"{sceneAssetGuid}\" cannot be found.");
                return null;
            }
            
            var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(sceneAssetPath);
            if (sceneAsset)
            {
                var sceneList = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
                sceneList.Add(new EditorBuildSettingsScene(sceneAssetPath, true));
                EditorBuildSettings.scenes = sceneList.ToArray();
            }
            else
            {
                Debug.LogWarning($"Failed to load scene \"{sceneAssetPath}\". Please make sure the file exist.");
            }

            return sceneAsset;
        }
    }
}