using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideChipAnimation : MonoBehaviour
{
    [SerializeField] bool startAnimation = false;
    [SerializeField] float animationSpeed = 1f;
    Vector2 startScale;
    Vector2 hideScale = new Vector2(0, 0);
    float tolerance = 0.01f;

    void Start()
    {
        startScale = transform.localScale;
    }

    void Update()
    {
        if (!startAnimation) return;
        AnimateChip();
    }

    private void AnimateChip()
    {
        float delta = animationSpeed * Time.deltaTime;
        transform.localScale = Vector2.Lerp(transform.localScale, hideScale, delta);
        if (IsAnimationEnded())
        {
            startAnimation = false;
            transform.localScale = hideScale;
        }
    }

    private bool IsAnimationEnded()
    {
        return transform.localScale.x - tolerance <= hideScale.x &&
         transform.localScale.y - tolerance <= hideScale.y;
    }

    internal void StartAnimation()
    {
        startAnimation = true;
    }
}
