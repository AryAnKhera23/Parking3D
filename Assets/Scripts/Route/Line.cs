using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    [SerializeField] public LineRenderer lineRenderer;
    
    public void SetColor(Color color)
    {
        lineRenderer.sharedMaterials[0].color = color;
    }
}
