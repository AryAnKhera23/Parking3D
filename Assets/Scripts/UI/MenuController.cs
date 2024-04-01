using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] Button exitButton;
    [SerializeField] GameObject levelSelections;
    private void Awake()
    {
        playButton.onClick.AddListener(Play);
        exitButton.onClick.AddListener(QuitApplication);
    }

    private void Play()
    {
        levelSelections.SetActive(true);
        SoundManager.Instance.Play(Sounds.ButtonClick);
    }

    private void QuitApplication()
    {
        Application.Quit();
    }
}
