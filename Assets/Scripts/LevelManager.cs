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
    }

    void OnDisable()
    {
        EventBus.onLevelCompleted -= LoadLevel;
    }

    public void LoadLevel()
    {
        var nextSceneIndex = (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCount;
        StartCoroutine(loadLevel(nextSceneIndex));
    }

    private IEnumerator loadLevel(int nextSceneIndex)
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(nextSceneIndex);
    }
}
