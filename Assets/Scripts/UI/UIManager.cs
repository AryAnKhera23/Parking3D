using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] private LineDrawer lineDrawer;

    [Space]
    [SerializeField] private CanvasGroup availableLineCanvasGroup;
    [SerializeField] private  Image availableLineFill;
    private bool isAvailableLineUIActive = false;

    [Space]
    [SerializeField] private Image fadePanel;
    [SerializeField] private float fadeDuration;


    private Route activeRoute;
    private void Start()
    {
        fadePanel.DOFade(0f, fadeDuration).From(1f);

        availableLineCanvasGroup.alpha = 0f;

        lineDrawer.OnBeginDraw += OnBeginDrawHandler;
        lineDrawer.OnDraw += OnDrawHandler;
        lineDrawer.OnEndDraw += OnEndDrawHandler;
    }

    private void OnEndDrawHandler()
    {
        if (isAvailableLineUIActive)
        {
            isAvailableLineUIActive = false;
            activeRoute = null;

            availableLineCanvasGroup.DOFade(0f, 0.3f).From(1f);
        }
    }

    private void OnDrawHandler()
    {
        if (isAvailableLineUIActive)
        {
            float maxLineLength = activeRoute.maxLineLength;
            float lineLength = activeRoute.line.length;

            availableLineFill.fillAmount = lineLength / maxLineLength;
        }
    }

    private void OnBeginDrawHandler(Route route)
    {
        activeRoute = route;
        availableLineFill.color = activeRoute.carColor;
        availableLineFill.fillAmount = 1f;
        availableLineCanvasGroup.DOFade(1f, .3f).From(0f);
        isAvailableLineUIActive = true;
    }
}
