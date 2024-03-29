using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    [SerializeField] private UserInput userInput;
    [SerializeField] private int interactableLayer;
    [SerializeField] private float clearInvalidLineDelay = .2f;
    private Line currentLine;
    private Route currentRoute;
    
    private RaycastDetector raycastDetector = new();


    private void Start()
    {
        userInput.OnMouseDown += OnMouseDownHandler;
        userInput.OnMouseUp += OnMouseUpHandler;
        userInput.OnMouseMove += OnMouseMoveHandler;
    }

    private void OnMouseDownHandler()
    {
        ContactInfo contactInfo = raycastDetector.RayCast(interactableLayer);

        if(contactInfo.contacted)
        {
            bool isCar = contactInfo.collider.TryGetComponent(out Car car);
            if(isCar && car.route.state == RouteState.Active)
            {
                currentRoute = car.route;
                currentLine = currentRoute.line;
                currentLine.InitializeLine();
            }
        }
    }
    private void OnMouseMoveHandler()
    {
        if(currentRoute != null)
        {
            ContactInfo contactInfo = raycastDetector.RayCast(interactableLayer);
            if (!contactInfo.contacted)
            {
                Invoke(nameof(ClearCurrentLine), clearInvalidLineDelay);
            }
            if (contactInfo.contacted)
            {
                Vector3 newPoint = contactInfo.point;
                currentLine.AddPoints(newPoint);
                if(contactInfo.collider.TryGetComponent(out Park park))
                {
                    Route parkRoute = park.route;
                    if(parkRoute == currentRoute)
                    {
                        currentLine.AddPoints(contactInfo.transform.position);
                    }
                    else
                    {
                        ClearCurrentLine();
                    }
                    OnMouseUpHandler();
                }
            }
        }
    }
    private void OnMouseUpHandler()
    {
        if (currentRoute != null)
        {
            ContactInfo contactInfo = raycastDetector.RayCast(interactableLayer);
            
            if (contactInfo.contacted)
            {
                if (currentLine.pointCount < 2 || !contactInfo.collider.TryGetComponent(out Park _))
                {
                    ClearCurrentLine();
                }
                else
                {
                    currentRoute.DeativateRoute();
                }
            }
            else
            {
                ClearCurrentLine();
            }
            ResetDrawer();
        }
    }

    private void ClearCurrentLine()
    {
        currentLine?.ClearLine();
    }

    private void ResetDrawer()
    {
        currentLine = null;
        currentRoute = null;
    }
}
