using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

/** NOTES:
 * https://docs.unity3d.com/ScriptReference/PackageManager.BuildUtilities.RegisterShouldIncludeInBuildCallback.html
 * https://docs.unity3d.com/Manual/RunningEditorCodeOnLaunch.html
 * https://forum.unity.com/threads/samples-in-packages-manual-setup.623080/
 */

namespace StoryFramework
{
    [AddComponentMenu("MoxieJam/StoryFramework/Core/Game")]
    public class Game : MonoBehaviour
    {
        public delegate void SceneLoadEvent(string sceneName);

        public delegate void BeginLoadSceneEvent(string sceneName);

        [SerializeField]
        GameSettings settings;

        [SerializeField]
        Audio.AudioManager audioManager;

        public static Game Instance;

        public GameSettings Settings => settings;
        public Audio.AudioManager AudioManager => audioManager;
        public GameSaveData SaveData { get; private set; }
        public static event BeginLoadSceneEvent OnBeginLoadScene;

        IngameUI ingameUI;

        public IngameUI IngameUi
        {
            get
            {
                if (!ingameUI)
                {
                    Debug.LogError("No active in-game UI set. Please make sure you do not try to access the in-game UI before it have awaken.");
                }

                return ingameUI;
            }

            set => ingameUI = value;
        }


#if false
        public static void Initialize(GameSettings settings, AudioManager audioManager)
        {
            Assert.IsNull(Instance, "Game is already initialized, please make sure to not initialize Game twice.");
            GameObject persistentGameData = new GameObject("Game", typeof(Game));
            DontDestroyOnLoad(persistentGameData);
            Instance = persistentGameData.GetComponent<Game>();

            if (settings != null)
            {
                Instance.Settings = settings;
            }
            else
            {
                /*var gameSettingsAssets = AssetDatabase.FindAssets($"t:{nameof(GameSettings)}");
                if (gameSettingsAssets.Length > 0)
                {
                    var gameSettingsPath = AssetDatabase.GUIDToAssetPath(gameSettingsAssets[0]);
                    Instance.Settings = AssetDatabase.LoadAssetAtPath<GameSettings>(gameSettingsPath);
                }
                else
                {
                    Debug.Log("No game settings found, please create a GameSettings file.");
                }*/
            }

            Instance.AudioManager = audioManager;
        }
        
        public static void Destroy()
        {
            // Make sure we have created a instance of Game before trying to destroy it.
            Assert.IsNotNull(Instance, "No Game instance exist, please make sure you have initialized Game before calling this.");
            if (Instance)
            {
                Destroy(Instance);
            }

            Instance = null;
        }
#endif

        void Awake()
        {
            // Make sure we only have one instance of Game at any moment.
            Assert.IsNull(Instance, "Game is already initialized, please make sure to not initialize Game twice.");
            Assert.IsNotNull(settings, "No settings file specified in game. Please assign a settings file to this object.");
            Assert.IsNotNull(audioManager, "No audio manager specified in game. Please create and assign a audio manager to this object.");

            // Listen to scene manager events.
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            SceneManager.activeSceneChanged += OnActiveSceneChanged;

            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        void Start()
        {
            StartCoroutine(LoadStartMenu());
        }

        void OnDestroy()
        {
            if (SaveData != null)
            {
                DestroySaveData(SaveData);
                SaveData = null;
            }

            // Stop listening to the scene manager.
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        }

        /// <summary>
        /// Starts a new game session by creating save data and loading the first in-game scene.
        /// </summary>
        /// <param name="sceneRef">Scene to load.</param>
        public static void StartNewGame(SceneRef sceneRef)
        {
            Instance.SaveData = Instance.CreateSaveData();
            LoadScene(sceneRef.sceneName, false);
        }

        /// <summary>
        /// Quit the game and return back to main menu.
        /// </summary>
        public static void QuitToMenu()
        {
            Instance.DestroySaveData(Instance.SaveData);
            Instance.SaveData = null;
            
            Instance.AudioManager.StopMusic();

            Destroy(Instance);
            Instance = null;
            SceneManager.LoadScene(0);
        }

        /// <summary>
        /// Quit the game.
        /// </summary>
        public static void Quit()
        {
            Application.Quit();
        }

        public static void LoadScene(SceneRef sceneRef, bool additive, SceneLoadEvent onSceneLoaded = null)
        {
            Instance.StartCoroutine(Instance.LoadSceneAsync(sceneRef.sceneName, additive, onSceneLoaded));
        }

        public static void LoadScene(string sceneName, bool additive, SceneLoadEvent onSceneLoaded = null)
        {
            Instance.StartCoroutine(Instance.LoadSceneAsync(sceneName, additive, onSceneLoaded));
        }

        private IEnumerator LoadStartMenu()
        {
            // Wait a frame to allow game to settle.
            yield return new WaitForEndOfFrame();

            // Make sure there's a start scene.
            if ((settings == null) || (string.IsNullOrEmpty(settings.StartScene.sceneName)))
            {
                Debug.LogError("No start scene specified in the game settings. Please set the start scene.");
                yield break;
            }

            // Load the start scene.
            yield return LoadSceneAsync(settings.StartScene.sceneName, false, null);
        }

        private IEnumerator LoadSceneAsync(string sceneName, bool additive, SceneLoadEvent onSceneLoaded)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError("Scene name is empty, did you forget to specify a scene?");
                yield break;
            }

            SaveCurrentSaveData();

            OnBeginLoadScene?.Invoke(sceneName);

            // Get current scene.
            var currentScene = SceneManager.GetActiveScene();
            string currentSceneName = SceneManager.GetActiveScene().path;
            
            // Get number of active scenes.
            int sceneCount = SceneManager.sceneCount;

            // Start loading the new scene.
            var sceneLoader = SceneManager.LoadSceneAsync(sceneName, additive ? LoadSceneMode.Additive : LoadSceneMode.Single);

            // Don't activate the scene automatically.
            sceneLoader.allowSceneActivation = false;

            // Wait for scene to load.
            while (!sceneLoader.isDone)
            {
                yield return new WaitForEndOfFrame();

                // When progress is above 0.9 the scene is more or less loaded according to Unity.
                if (sceneLoader.progress >= 0.9f)
                {
                    //Change the Text to show the Scene is ready
                    //m_Text.text = "Press the space bar to continue";
                    //Wait to you press the space key to activate the Scene
                    //if (Input.GetKeyDown(KeyCode.Space))
                    {
                        /*if (additive)
                        {
                            // Unload the old scene.
                            var sceneUnloader = SceneManager.UnloadSceneAsync(currentScene);
                            while ((sceneUnloader != null) && (!sceneUnloader.isDone))
                            {
                                yield return new WaitForEndOfFrame();
                            }
                        }*/

                        // Activate the loaded Scene.
                        sceneLoader.allowSceneActivation = true;
                    }
                }
            }

            // Tell the game the scene is loaded.
            onSceneLoaded?.Invoke(sceneName);

            // Get the loaded scene.
            var loadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
            while (!loadedScene.isLoaded)
            {
                yield return new WaitForEndOfFrame();
            }

            /*var rootObjects = loadedScene.GetRootGameObjects();
            foreach (var rootObject in rootObjects)
            {
                var persistentComponents = rootObject.GetComponentsInChildren<IPersistentComponent>();
                foreach (var persistentComponent in persistentComponents)
                {
                    //Debug.Log($"Loading data for {(persistentComponent as MonoBehaviour).name}");
                    persistentComponent.LoadPersistentData(SaveData);
                }
            }*/

            // Set the new scene to be the active scene.
            //SceneManager.SetActiveScene(currentScene);

            // Todo: Unload current scene.
            //SceneManager.UnloadSceneAsync()
        }

        void OnSceneLoaded(Scene loadedScene, LoadSceneMode loadSceneMode)
        {
            var rootObjects = loadedScene.GetRootGameObjects();
            foreach (var rootObject in rootObjects)
            {
                var persistentComponents = rootObject.GetComponentsInChildren<IPersistentComponent>();
                foreach (var persistentComponent in persistentComponents)
                {
                    //Debug.Log($"Loading data for {(persistentComponent as MonoBehaviour).name}");
                    persistentComponent.LoadPersistentData(SaveData);
                }
            }

            SceneManager.SetActiveScene(loadedScene);
        }

        void OnSceneUnloaded(Scene unloadedScene)
        {
        }

        void OnActiveSceneChanged(Scene previousScene, Scene newScene)
        {
        }

        protected virtual GameSaveData CreateSaveData()
        {
            return new GameSaveData();
        }

        protected virtual void SaveCurrentSaveData()
        {
            
        }

        protected virtual void DestroySaveData(GameSaveData saveData)
        {
            saveData.Dispose();
            saveData = null;
        }
    }
}