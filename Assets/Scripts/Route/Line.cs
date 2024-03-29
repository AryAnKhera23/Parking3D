using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    [HideInInspector] public LineRenderer lineRenderer;
    [SerializeField] float minimumPointDistance;

    [HideInInspector] public List<Vector3> points = new();
    [HideInInspector] public int pointCount = 0;

    private float pointFixedYAxis;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        pointFixedYAxis = lineRenderer.GetPosition(0).y;
        ClearLine();
    }

    public void ClearLine()
    {
        gameObject.SetActive(false);
        lineRenderer.positionCount = 0;
        pointCount = 0;
        points.Clear();
    }

    public void InitializeLine()
    {
        gameObject.SetActive(true);
    }

    public void AddPoints(Vector3 newPoint)
    {
        newPoint.y = pointFixedYAxis;
        if(pointCount >= 1 && Vector3.Distance(newPoint, GetLastPoint()) < minimumPointDistance) {
            return;
        }

        points.Add(newPoint);
        pointCount++;
        Debug.Log("Total Points: " + pointCount);
        lineRenderer.positionCount = pointCount;
        lineRenderer.SetPosition(pointCount - 1, newPoint);
    }

    private Vector3 GetLastPoint()
    {
        return lineRenderer.GetPosition(pointCount - 1);
    }


    public void SetColor(Color color)
    {
        lineRenderer.sharedMaterials[0].color = color;
    }
}
