using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Car : MonoBehaviour
{
    [SerializeField] public Route route;
    [SerializeField] public Transform bottomTransform;
    [SerializeField] public Transform bodyTransform;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float idleCarDanceValue;
    [SerializeField] private float durationMultiplier;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        bodyTransform.DOLocalMoveY(idleCarDanceValue, .1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear); 
    }

    public void MoveCar(Vector3[] path)
    {
        rb.DOLocalPath(path, 2f * durationMultiplier * path.Length).SetLookAt(.01f, true).SetEase(Ease.Linear);
    }
    public void SetColor(Color color)
    {
        meshRenderer.sharedMaterials[0].color = color;
    }
}
