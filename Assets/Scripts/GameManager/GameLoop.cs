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
    public UnityAction OnCarCollision;

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

    private void OnCarCollisionHandler()
    {
        Debug.Log("GameOver");
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
            int nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
            DOVirtual.DelayedCall(1.3f, () =>
            {
                if (nextLevel < SceneManager.sceneCountInBuildSettings)
                {
                    SceneManager.LoadScene(nextLevel);
                }
                else
                {
                    Debug.Log("No Next Level");
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
