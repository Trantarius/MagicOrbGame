using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager singletonInstance;
    void Awake()
    {
        if (singletonInstance == null)
        {
            singletonInstance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    void OnEnable()
    {
        EventBus.onLevelCompleted += LoadLevel;
        EventBus.onLevelFailed += RestartLevel;
        EventBus.onGameFailed += RestartGame;
        EventBus.onGameRestarted += RestartGame;
        EventBus.onGameStarted += StartGame;
    }

    void OnDisable()
    {
        EventBus.onLevelCompleted -= LoadLevel;
        EventBus.onLevelFailed -= RestartLevel;
        EventBus.onGameFailed -= RestartGame;
        EventBus.onGameRestarted -= RestartGame;
        EventBus.onGameStarted -= StartGame;
    }

    public void LoadLevel()
    {
        var nextSceneIndex = (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings;
        StartCoroutine(LoadLevelDelayed(nextSceneIndex));
    }

    private IEnumerator LoadLevelDelayed(int nextSceneIndex)
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(nextSceneIndex);
    }

    public void RestartLevel()
    {
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(LoadLevelDelayed(currentSceneIndex));
    }

    public void RestartGame()
    {
         SceneManager.LoadScene(0);
    }

    public void StartGame()
    {
         SceneManager.LoadScene(1);
    }

    public void Update()
    {   // Reset game on "r" press
        if (Input.GetKeyDown("r"))
        { //If you press R
            var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            StartCoroutine(LoadLevelDelayed(currentSceneIndex)); //Load scene called Game
        }
    }
}
