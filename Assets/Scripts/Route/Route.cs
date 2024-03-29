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
    [SerializeField] private Car car;
    [SerializeField] private Line line;
    [SerializeField] private Park park;

    [Space]
    [Header("Colors: ")]
    [SerializeField] private Color carColor;
    [SerializeField] private Color lineColor;

    public void Deativate()
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
