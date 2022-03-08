using UnityEngine;
using StoryFramework;

public class StartScene : MonoBehaviour
{
    [SerializeField]
    SceneRef firstInGameScene;

    [SerializeField]
    GameObject inGameUiPrefab;

    public void StartGame()
    {
        Game.StartNewGame(firstInGameScene);
    }

    public void QuitGame()
    {
        Game.Quit();
    }
}
