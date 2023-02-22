using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace StoryFramework
{
    public class GameSettings : ScriptableObject
    {
        public const string SettingsPath = "Assets/MoxieJamStorySettings.asset";

        public static GameSettings Instance;

        [SerializeField]
        public SceneRef StartScene;

        [SerializeField]
        public GameState[] GlobalGameStates;
        
        /*/// <summary>
        /// Static Function to load the TMP Settings file.
        /// </summary>
        /// <returns></returns>
        public static TMP_Settings LoadDefaultSettings()
        {
            if (s_Instance == null)
            {
                // Load settings from TMP_Settings file
                TMP_Settings settings = Resources.Load<TMP_Settings>("TMP Settings");
                if (settings != null)
                    s_Instance = settings;
            }

            return s_Instance;
        }*/

        void OnValidate()
        {
            if (GlobalGameStates != null)
            {
                /*foreach (var state in GlobalGameStates)
                {
                    var matchingStates = Array.FindAll(GlobalGameStates,
                        x =>
                            x.Identifier.Equals(state.Identifier, StringComparison.OrdinalIgnoreCase) &&
                            x.Property.Equals(state.Property, StringComparison.OrdinalIgnoreCase));
                    if (matchingStates.Length > 1)
                    {
                        Debug.LogError($"Multiple states with the same identifier {state.Identifier} and " +
                                       $"property {state.Property} is not allowed. Please change the name of one of " +
                                       $"the items.");
                    }
                }*/
            }
        }

        private void OnEnable()
        {
            Instance = this;
        }

#if UNITY_EDITOR
        public static GameSettings GetOrCreateSettings()
        {
            var settings = AssetDatabase.LoadAssetAtPath<GameSettings>(SettingsPath);
            if (!settings)
            {
                settings = CreateInstance<GameSettings>();
                AssetDatabase.CreateAsset(settings, SettingsPath);
                AssetDatabase.SaveAssets();
                
                // Add the settings assets to the preloaded player assets.
                var preloadedAssets = new List<UnityEngine.Object>(PlayerSettings.GetPreloadedAssets());
                if (!preloadedAssets.Contains(settings))
                {
                    preloadedAssets.Add(settings);
                    PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
                    AssetDatabase.SaveAssets();
                }
            }
            return settings;
        }
        
        public static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetOrCreateSettings());
        }
#endif
    }
}
