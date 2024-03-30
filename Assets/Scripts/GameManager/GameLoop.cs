using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GameLoop : MonoBehaviour
{
    private static GameLoop instance;
    public static GameLoop Instance { get { return instance; } }

    [HideInInspector] public List<Route> readyRoutes = new();
    private int totalRoutes;

    public UnityAction<Route> OnCarEntersPark;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        totalRoutes = transform.GetComponentsInChildren<Route>().Length;
        OnCarEntersPark += OnCarEntersParkHandler;
    }

    private void OnCarEntersParkHandler(Route route)
    {
        route.car.StopDancingAnim();
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
