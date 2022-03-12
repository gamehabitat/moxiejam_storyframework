using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StoryFramework.Editor.Core
{
    public class GameEditorSceneBuildSettings : EditorWindow
    {
        private static readonly Type AppHandlerType = typeof(StoryFramework.Game); 
        
        [MenuItem("MoxieJam/StoryFramework/Add project scenes to build settings")]
        public static void InitWindow()
        {
            var window = GetWindow(typeof(GameEditorSceneBuildSettings));
            window.titleContent = new GUIContent("Add project scenes to build settings");
            window.ShowModal();
        }

        bool isReloadingAssembly = false;
        private bool refereshProjectSceneList = false;
        int startUpSceneIndex = 0;
        List<bool> addProjectScenes = new();
        List<EditorBuildSettingsScene> projectScenes = new();
        Vector2 projectSceneScrollPos = Vector2.zero;

        private void OnEnable()
        {
            refereshProjectSceneList = true;
            AssemblyReloadEvents.beforeAssemblyReload += OnBeforeAssemblyReload;
            AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;
            SceneAssetPostProcessor.OnAssetDatabaseScenesModified += RefreshProjectSceneList;
            EditorSceneManager.sceneSaved += OnSceneSaved;
        }
        
        private void OnDisable()
        {
            EditorSceneManager.sceneSaved -= OnSceneSaved;
            SceneAssetPostProcessor.OnAssetDatabaseScenesModified -= RefreshProjectSceneList;
            AssemblyReloadEvents.afterAssemblyReload -= OnAfterAssemblyReload;
            AssemblyReloadEvents.beforeAssemblyReload -= OnBeforeAssemblyReload;
        }

        private void OnSceneSaved(Scene scene)
        {
            refereshProjectSceneList = true;
        }

        private void OnBeforeAssemblyReload()
        {
            isReloadingAssembly = true;
        }

        private void OnAfterAssemblyReload()
        {
            isReloadingAssembly = false;
            refereshProjectSceneList = true;
        }

        void RefreshProjectSceneList()
        {
            refereshProjectSceneList = true;
        }

        void UpdateProjectSceneList()
        {
            if (EditorApplication.isPlaying || isReloadingAssembly)
            {
                return;
            }
 
            refereshProjectSceneList = false;
            startUpSceneIndex = -1;
            projectScenes.Clear();
            addProjectScenes.Clear();
            
            var buildScenes = EditorBuildSettings.scenes;
            List<string> loadedScenes = new();
            for (int i = 0; i < EditorSceneManager.sceneCount; ++i)
            {
                loadedScenes.Add(EditorSceneManager.GetSceneAt(i).path);
            }

            var sceneGuidsInProject = AssetDatabase.FindAssets("t:scene");
            foreach (var sceneGuidString in sceneGuidsInProject)
            {
                var sceneGuid = new GUID(sceneGuidString);
                var scenePath = AssetDatabase.GUIDToAssetPath(sceneGuid);
                
                // Find start up scene.
                int loadedSceneIndex = loadedScenes.FindIndex(x => x.Equals(scenePath, StringComparison.OrdinalIgnoreCase));
                var scene = loadedSceneIndex < 0 ? EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive) : EditorSceneManager.GetSceneAt(loadedSceneIndex);
                var rootGameObjects = scene.GetRootGameObjects();
                string multipleStartUpScenes = string.Empty;
                foreach (var o in rootGameObjects)
                {
                    var game = o.GetComponentInChildren(AppHandlerType);
                    if (game)
                    {
                        if (startUpSceneIndex >= 0)
                        {
                            if (string.IsNullOrEmpty(multipleStartUpScenes))
                            {
                                multipleStartUpScenes = $"Found multiple scenes containing the {AppHandlerType.Name} object:\n{projectScenes[startUpSceneIndex].path}";
                            }

                            multipleStartUpScenes += $"\n{scenePath}";  
                        }
                        startUpSceneIndex = projectScenes.Count;
                    }
                }

                if (!string.IsNullOrEmpty(multipleStartUpScenes))
                {
                    Debug.LogWarning(multipleStartUpScenes);
                }

                if (loadedSceneIndex < 0)
                {
                    EditorSceneManager.CloseScene(scene, true);
                }
                
                int buildIndex = Array.FindIndex(buildScenes, x => x.guid == sceneGuid);
                if (buildIndex < 0)
                {
                    projectScenes.Add(new EditorBuildSettingsScene(sceneGuid, true));
                    addProjectScenes.Add(false);
                }
                else
                {
                    projectScenes.Add(buildScenes[buildIndex]);
                    addProjectScenes.Add(true);
                }
            }

            if (startUpSceneIndex >= 0)
            {
                var startUpScene = projectScenes[startUpSceneIndex];
                var addStartUpScene = addProjectScenes[startUpSceneIndex];
                projectScenes.RemoveAt(startUpSceneIndex);
                addProjectScenes.RemoveAt(startUpSceneIndex);
                projectScenes.Insert(0, startUpScene);
                addProjectScenes.Insert(0, addStartUpScene);
                startUpSceneIndex = 0;
            }
        }

        void OnGUI()
        {
            if (refereshProjectSceneList)
            {
                UpdateProjectSceneList();
            }

            EditorGUILayout.LabelField("Scenes in project", EditorStyles.boldLabel);
            projectSceneScrollPos = EditorGUILayout.BeginScrollView(projectSceneScrollPos);
            EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(true));
            for (int i = 0; i < projectScenes.Count; ++i)
            {
                if (startUpSceneIndex == i)
                {
                    addProjectScenes[i] = EditorGUILayout.ToggleLeft($"(Start Up scene) {projectScenes[i].path}", addProjectScenes[i], EditorStyles.boldLabel);
                }
                else
                {
                    addProjectScenes[i] = EditorGUILayout.ToggleLeft(projectScenes[i].path, addProjectScenes[i]);
                }
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add selected scenes to build settings"))
            {
                List<EditorBuildSettingsScene> buildSettingsScenes = new List<EditorBuildSettingsScene>(projectScenes.Count);
                for(int i = 0; i < addProjectScenes.Count; ++i)
                {
                    if (addProjectScenes[i])
                    {
                        buildSettingsScenes.Add(projectScenes[i]);
                    }
                }
                EditorBuildSettings.scenes = buildSettingsScenes.ToArray();
            }

            if (GUILayout.Button("Open build settings"))
            {
                EditorWindow.GetWindow(Type.GetType("UnityEditor.BuildPlayerWindow,UnityEditor"));
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}