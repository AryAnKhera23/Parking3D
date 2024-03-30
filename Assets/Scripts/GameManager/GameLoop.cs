using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    private static GameLoop instance;
    public static GameLoop Instance { get { return instance; } }

    [HideInInspector] public List<Route> readyRoutes = new();
    private int totalRoutes;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        totalRoutes = transform.GetComponentsInChildren<Route>().Length;
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
