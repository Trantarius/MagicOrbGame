using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public HealthBar healthBar;

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
    }

    void OnDisable()
    {
        EventBus.onLevelCompleted -= LoadLevel;
        EventBus.onLevelFailed -= RestartLevel;
        EventBus.onGameFailed -= RestartGame;
    }

    public void LoadLevel()
    {
        var nextSceneIndex = (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings;
        if(nextSceneIndex == 0)
        {
            healthBar.RestoreHealth();
        }
        StartCoroutine(LoadLevel(nextSceneIndex));
    }

    private IEnumerator LoadLevel(int nextSceneIndex)
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(nextSceneIndex);
    }

    public void RestartLevel()
    {
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(LoadLevel(currentSceneIndex));
    }

    public void RestartGame()
    {
        healthBar.RestoreHealth();
        StartCoroutine(LoadLevel(0));
    }
}
