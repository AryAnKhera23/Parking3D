using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameLoop : MonoBehaviour
{
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
        Debug.Log("GameOver");
        route.park.collider.enabled = false;
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        DOVirtual.DelayedCall(2f, () =>
        {
            SceneManager.LoadScene(currentLevel);
        });

    }

    private void OnCarEntersParkHandler(Route route)
    {
        route.car.StopDancingAnimation();
        successfulParks++;

        if(successfulParks == totalRoutes)
        {
            
            LevelManager.Instance?.MarkCurrentLevelComplete();
            int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
            int nextLevelIndex = currentLevelIndex + 1;

            
            DOVirtual.DelayedCall(1.3f, () =>
            {
                if (nextLevelIndex < SceneManager.sceneCountInBuildSettings)
                {
                    SceneManager.LoadScene(nextLevelIndex);
                }
                else
                {
                    SceneManager.LoadScene(0);
                }
            });
            
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
}
