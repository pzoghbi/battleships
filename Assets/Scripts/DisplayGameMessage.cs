using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DisplayGameMessage : MonoBehaviour
{
    [SerializeField] private float textShowForSeconds = 1f;
    [SerializeField] private float textFadeOutTime = 1f;
    [SerializeField] private float maxScale = 1.5f;
    [SerializeField] private float minScale = .5f;

    private TextMeshProUGUI textMesh;
    private Coroutine showMessageCoroutine;
    private Coroutine scaleCoroutine;

    private delegate float EasingFunc(float value);

    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    internal void ShowText(string text)
    {
        if (showMessageCoroutine != null) StopCoroutine(showMessageCoroutine);
        showMessageCoroutine = StartCoroutine(ShowMessageRoutine(text, textShowForSeconds, textFadeOutTime, EaseIn));
    }


    private IEnumerator ShowMessageRoutine(string text, float showForSeconds, float fadeOutTime, EasingFunc EasingFunc = null)
    {
        // reset slave routine
        if (scaleCoroutine != null) StopCoroutine(scaleCoroutine);

        textMesh.text = text;
        textMesh.color = Color.white;

        yield return ShiftScaleRoutine(1, maxScale, 1, EaseOut);

        if (GameManager.instance.isGameOver) yield break;

        yield return new WaitForSecondsRealtime(showForSeconds);

        scaleCoroutine = StartCoroutine(ShiftScaleRoutine(transform.localScale.x, minScale, fadeOutTime, EaseIn));

        float lerpSpeed = 1 / fadeOutTime;

        Color startColor = textMesh.color;
        Color endColor = Color.clear;

        float startTime = Time.time;
        float distanceAlpha = Mathf.Abs(startColor.a - endColor.a);
        float distanceCovered = (Time.time - startTime) * lerpSpeed;
        float fraction = distanceCovered / distanceAlpha;


        while (fraction < 1)
        {
            float easing = (EasingFunc != null) ? EasingFunc(fraction) : fraction;
            textMesh.color = Color.Lerp(startColor, endColor, easing);
            distanceCovered = (Time.time - startTime) * lerpSpeed;
            fraction = distanceCovered / distanceAlpha;
            if (fraction >= 1) textMesh.color = endColor;
            yield return null;
        }
    }
    private IEnumerator ShiftScaleRoutine(float startScale, float endScale, float time = 1, EasingFunc Easing = null)
    {
        float lerpSpeed = 1 / time;
        float startTime = Time.time;

        float distance = Mathf.Abs(startScale - endScale);
        float distanceCovered = (Time.time - startTime) * lerpSpeed;
        float fraction = distanceCovered / distance;

        while (fraction < 1)
        {
            float easing = (Easing != null) ? Easing(fraction) : fraction;
            transform.localScale = Vector3.one * Mathf.Lerp(startScale, endScale, easing);
            distanceCovered = (Time.time - startTime) * lerpSpeed;
            fraction = distanceCovered / distance;

            yield return null;
        }
    }

    private float EaseOut(float value)
    {
        return 1 - (1 - value) * (1 - value);
    }

    private float EaseIn(float value)
    {
        return value * value;
    }
}
