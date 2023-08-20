﻿using System.Collections;
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
    }

    void OnDisable()
    {
        EventBus.onLevelCompleted -= LoadLevel;
        EventBus.onLevelFailed -= RestartLevel;
    }

    public void LoadLevel()
    {
        var nextSceneIndex = (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCount;
        StartCoroutine(LoadLevel(nextSceneIndex));
    }

    private IEnumerator LoadLevel(int nextSceneIndex)
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(nextSceneIndex);
    }

    public void RestartLevel()
    {
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(LoadLevel(currentSceneIndex));
    }
}
