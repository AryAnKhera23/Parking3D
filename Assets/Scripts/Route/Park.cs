using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Park : MonoBehaviour
{
    [SerializeField] public Route route;
    [HideInInspector] public new BoxCollider collider;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private ParticleSystem parkFX;
    private ParticleSystem.MainModule fxMainModule;

    private void Start()
    {
        collider = GetComponent<BoxCollider>();
        fxMainModule = parkFX.main;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Car car))
        {
            if(car.route == route)
            {
                GameLoop.Instance.OnCarEntersPark?.Invoke(route);
                PlayParkFX();
            }
        }
    }

    private void PlayParkFX()
    {
        fxMainModule.startColor = route.carColor;
        parkFX.Play();
    }

    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }
}
