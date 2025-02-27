﻿using System.Collections;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadein;
    [SerializeField] private float fadeinTime = 5f;
    [SerializeField] private AnimationCurve fadeinCurve;

    private void Awake()
    {
        if (KeepOnLoad.Instance)
            Destroy(KeepOnLoad.Instance.gameObject);
    }

    private void Start()
    {
        ScriptableRendererFeatureManager.Instance.ToggleAllFog(false);
        StartCoroutine(FadeInCoroutine());
    }

    private IEnumerator FadeInCoroutine()
    {
        float time = 0;

        while (time < fadeinTime)
        {
            fadein.alpha = Mathf.Lerp(1, 0, fadeinCurve.Evaluate(time / fadeinTime));
            time += Time.deltaTime;
            yield return null;
        }

        fadein.gameObject.SetActive(false);
    }
}