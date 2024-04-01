using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LineDrawer : MonoBehaviour
{
    [SerializeField] private UserInput userInput;
    [SerializeField] private int interactableLayer;
    [SerializeField] private float clearInvalidLineDelay = .2f;
    private Line currentLine;
    private Route currentRoute;
    
    private RaycastDetector raycastDetector = new();

    public UnityAction<Route> OnBeginDraw;
    public UnityAction OnDraw;
    public UnityAction OnEndDraw;
    public UnityAction<Route, List<Vector3>> OnParkLinkedToLine;


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

                OnBeginDraw?.Invoke(currentRoute);
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
                Invoke(nameof(OnMouseUpHandler), clearInvalidLineDelay);
                return;
            }

            if (contactInfo.contacted)
            {
                Vector3 newPoint = contactInfo.point;

                if (currentLine.length >= currentRoute.maxLineLength)
                {
                    OnMouseUpHandler();
                    return;
                }
                    
                currentLine.AddPoints(newPoint);
                OnDraw?.Invoke();

                if(contactInfo.collider.TryGetComponent(out Park park))
                {
                    Route parkRoute = park.route;
                    if(parkRoute == currentRoute)
                    {
                        currentLine.AddPoints(contactInfo.transform.position);
                        OnDraw?.Invoke();
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
                    OnParkLinkedToLine?.Invoke(currentRoute, currentLine.points);
                    currentRoute.DeativateRoute();
                }
            }
            else
            {
                ClearCurrentLine();
            }
            ResetDrawer();
            OnEndDraw?.Invoke();
        }
    }

    private void ClearCurrentLine()
    {
        if(currentLine != null)
            currentLine.ClearLine();
        ResetDrawer();
    }

    private void ResetDrawer()
    {
        currentLine = null;
        currentRoute = null;
    }
}
