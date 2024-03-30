using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum RouteState
{
    Active,
    Inactive
}

public class Route : MonoBehaviour
{
    public RouteState state;
    [SerializeField] public Car car;
    [SerializeField] public Line line;
    [SerializeField] private Park park;
    [SerializeField] private LineDrawer lineDrawer;
    [HideInInspector] public Vector3[] linePoints;

    [Space]
    [Header("Colors: ")]
    [SerializeField] private Color carColor;
    [SerializeField] private Color lineColor;

    private void Start()
    {
        lineDrawer.OnParkLinkedToLine += OnParkLinkedToLineHandler;
    }

    private void OnParkLinkedToLineHandler(Route route, List<Vector3> points)
    {
        if(route == this)
        {
            linePoints = points.ToArray();
            GameLoop.Instance.SetRouteReady(this);
        }
    }

    public void DeativateRoute()
    {
        state = RouteState.Inactive;
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(!Application.isPlaying &&  car != null) {
            line.lineRenderer.SetPosition(0, car.bottomTransform.position);
            line.lineRenderer.SetPosition(1, park.transform.position);

            car.SetColor(carColor);
            line.SetColor(lineColor);
            park.SetColor(carColor);
        }
    }
#endif
}
