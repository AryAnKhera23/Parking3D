using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] public Route route;
    [SerializeField] public Transform bottomTransform;
    [SerializeField] private MeshRenderer meshRenderer;
    public void SetColor(Color color)
    {
        meshRenderer.sharedMaterials[0].color = color;
    }
}
