using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    public static LevelManager Instance { get { return instance; } }

    [SerializeField] public string[] Levels;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (GetLevelStatus(Levels[1]) == LevelStatus.Locked)
        {
            SetLevelStatus(Levels[1], LevelStatus.Unlocked);
        }
    }

    public LevelStatus GetLevelStatus(string level)
    {
        LevelStatus levelStatus = (LevelStatus)PlayerPrefs.GetInt(level, 0);
        return levelStatus;
    }

    public void SetLevelStatus(string level, LevelStatus levelStatus)
    {
        PlayerPrefs.SetInt(level, (int)levelStatus);
    }

    internal void MarkCurrentLevelComplete()
    {
        int currentSceneIndex = GetCurrentSceneIndex();
        int nextSceneIndex = GetNextSceneIndex();

        if (SoundManager.Instance != null)
            SoundManager.Instance.Play(Sounds.LevelComplete);
        SetLevelStatus(Levels[currentSceneIndex], LevelStatus.Completed);

        if (nextSceneIndex < Levels.Length)
        {
            SetLevelStatus(Levels[nextSceneIndex], LevelStatus.Unlocked);
        }
    }

    public int GetNextSceneIndex()
    {
        return GetCurrentSceneIndex() + 1;
    }

    public int GetCurrentSceneIndex()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        int currentSceneIndex = Array.FindIndex(Levels, level => level == currentScene.name);
        return currentSceneIndex;
    }

    public void LoadLevel(string level)
    {
        if (GetCurrentSceneIndex() <= Levels.Length)
        {
            SceneManager.LoadScene(level);
        }
    }
}
