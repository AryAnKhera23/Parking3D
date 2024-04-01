using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Line : MonoBehaviour
{
    [HideInInspector] public LineRenderer lineRenderer;
    [SerializeField] private float minimumPointDistance;

    [HideInInspector] public List<Vector3> points = new();
    [HideInInspector] public int pointCount = 0;
    [HideInInspector] public float length;

    private float pointFixedYAxis;
    private Vector3 previousPoint;
    

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
        length = 0f;
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

        if(pointCount == 0)
        {
            previousPoint = newPoint;
        }

        points.Add(newPoint);
        pointCount++;
        length += Vector3.Distance(previousPoint, newPoint);
        previousPoint = newPoint;
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
