using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private LineDrawer lineDrawer;
    [SerializeField] private GameLoop gameLoop;

    [Space]
    [SerializeField] private CanvasGroup availableLineCanvasGroup;
    [SerializeField] private  Image availableLineFill;
    private bool isAvailableLineUIActive = false;

    [Space]
    [SerializeField] private Image fadePanel;
    [SerializeField] private float fadeDuration;

    [Space]
    [SerializeField] public GameObject pausePanel;
    [SerializeField] public Button pauseButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button returnToMenu;

    [Space]
    private SceneReloader sceneReloader;
    [SerializeField] public GameObject gameOverPanel;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button reloadButton;

    [Space]
    [SerializeField] public GameObject levelCompletePanel;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button mainMenuButton;

    private LevelManager levelManager;


    private Route activeRoute;
    private void Start()
    {
        levelManager = LevelManager.Instance;
        fadePanel.DOFade(0f, fadeDuration).From(1f);
        sceneReloader = GetComponent<SceneReloader>();
        availableLineCanvasGroup.alpha = 0f;

        lineDrawer.OnBeginDraw += OnBeginDrawHandler;
        lineDrawer.OnDraw += OnDrawHandler;
        lineDrawer.OnEndDraw += OnEndDrawHandler;
        AddButtonListeners();
        

    }

    private void AddButtonListeners()
    {
        pauseButton.onClick.AddListener(Pause);
        resumeButton.onClick.AddListener(Resume);
        returnToMenu.onClick.AddListener(LoadMenuScene);

        menuButton.onClick.AddListener(LoadMenuScene);
        reloadButton.onClick.AddListener(sceneReloader.ReloadLevel);

        nextLevelButton.onClick.AddListener(LoadNextLevel);
        mainMenuButton.onClick.AddListener(LoadMenuScene);
    }

    private void LoadNextLevel()
    {
        if(levelManager != null)
        {
            int currentSceneIndex = levelManager.GetCurrentSceneIndex();
            levelManager.LoadLevel(levelManager.Levels[currentSceneIndex + 1]);
        }
    }

    private void LoadMenuScene()
    {
        Time.timeScale = 1f;
        if(levelManager != null)
            levelManager.LoadLevel("MenuScene");
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    private void Pause()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }

    private void OnEndDrawHandler()
    {
        if (isAvailableLineUIActive)
        {
            isAvailableLineUIActive = false;
            activeRoute = null;

            availableLineCanvasGroup.DOFade(0f, 0.3f).From(1f);
        }
    }

    private void OnDrawHandler()
    {
        if (isAvailableLineUIActive)
        {
            float maxLineLength = activeRoute.maxLineLength;
            float lineLength = activeRoute.line.length;

            availableLineFill.fillAmount = lineLength / maxLineLength;
        }
    }

    private void OnBeginDrawHandler(Route route)
    {
        activeRoute = route;
        availableLineFill.color = activeRoute.carColor;
        availableLineFill.fillAmount = 1f;
        availableLineCanvasGroup.DOFade(1f, .3f).From(0f);
        isAvailableLineUIActive = true;
    }
}
