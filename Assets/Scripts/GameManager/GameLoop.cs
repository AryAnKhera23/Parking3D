using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;



public class GameLoop : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    private static GameLoop instance;
    public static GameLoop Instance { get { return instance; } }

    [HideInInspector] public List<Route> readyRoutes = new();
    private int totalRoutes;
    private int successfulParks;
    public UnityAction<Route> OnCarEntersPark;
    public UnityAction<Route> OnCarCollision;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        successfulParks = 0;
        totalRoutes = transform.GetComponentsInChildren<Route>().Length;
        OnCarCollision += OnCarCollisionHandler;
        OnCarEntersPark += OnCarEntersParkHandler;
    }

    private void OnCarCollisionHandler(Route route)
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.Play(Sounds.CarCrash);
        route.park.collider.enabled = false;
        GameOver();
    }

    private void OnCarEntersParkHandler(Route route)
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.Play(Sounds.Park);
        route.car.StopDancingAnimation();
        successfulParks++;

        if(successfulParks == totalRoutes)
        {
            LevelManager.Instance?.MarkCurrentLevelComplete();
            LevelComplete();
        }
    }

    public void SetRouteReady(Route route)
    {
        readyRoutes.Add(route);

        if(readyRoutes.Count == totalRoutes )
        {
            MoveAllCars();
        }
    }

    private void MoveAllCars()
    {
        foreach(var route in readyRoutes)
        {
            route.car.MoveCar(route.linePoints);
        }
    }

    private void GameOver()
    {
        uiManager.gameOverPanel.SetActive(true);
    }

    private void LevelComplete()
    {
        uiManager.levelCompletePanel.SetActive(true);
    }
}
