using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Car : MonoBehaviour
{
    [SerializeField] public Route route;
    [SerializeField] public Transform bottomTransform;
    [SerializeField] public Transform bodyTransform;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private ParticleSystem smokeFX;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float idleCarDanceValue;
    [SerializeField] private float durationMultiplier;
    [SerializeField] private float explosionForce = 400f;
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private float upwardExplosionForceMultiplier = 2f;
    [SerializeField] private float lookAtPercentage = .05f;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        bodyTransform.DOLocalMoveY(idleCarDanceValue, .1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear); 
    }

    public void MoveCar(Vector3[] path)
    {
        rb.DOLocalPath(path, 2f * durationMultiplier * path.Length).SetLookAt(lookAtPercentage, true).SetEase(Ease.Linear);
    }
    public void SetColor(Color color)
    {
        meshRenderer.sharedMaterials[0].color = color;
    }

    public void StopDancingAnimation()
    {
        bodyTransform.DOKill(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out Car otherCar))
        {
            StopDancingAnimation();
            rb.DOKill(false);

            Vector3 hitPoint = collision.contacts[0].point;
            AddExplosionForce(hitPoint);
            smokeFX.Play();
            GameLoop.Instance.OnCarCollision.Invoke(route);
        }
    }

    private void AddExplosionForce(Vector3 point)
    {
        rb.AddExplosionForce(explosionForce, point, explosionRadius);
        rb.AddForceAtPosition(Vector2.up * upwardExplosionForceMultiplier, point, ForceMode.Impulse);
        rb.AddTorque(new Vector3(GetRandomAngle(), GetRandomAngle(), GetRandomAngle()));
    }

    private float GetRandomAngle()
    {
        float angle = 10f;
        float random = UnityEngine.Random.value;
        return random > .5f ? angle : -angle;
    }
}
