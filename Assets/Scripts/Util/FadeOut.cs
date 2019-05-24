using System;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    private float TOLERANCE = 0.1f;

    public float minimum = 0.0f;
    public float maximum = 1f;
    public float duration = 5.0f;
    private float startTime;

    private SpriteRenderer renderer;

    void OnEnable()
    {
        startTime = Time.time;
        renderer = GetComponent<SpriteRenderer>();
        renderer.color = new Color(1f, 1f, 1f, 1f);
    }

    void Update()
    {
        float t = (Time.time - startTime) / duration;

        var alpha = 1f - Mathf.SmoothStep(minimum, maximum, t);
        renderer.color = new Color(1f, 1f, 1f, alpha);

        if (Math.Abs(alpha) < TOLERANCE)
        {
            gameObject.SetActive(false);
        }
    }
}