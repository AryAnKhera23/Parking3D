using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Park : MonoBehaviour
{
    [SerializeField] private Route route;
    [SerializeField] private SpriteRenderer spriteRenderer;
    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }
}
