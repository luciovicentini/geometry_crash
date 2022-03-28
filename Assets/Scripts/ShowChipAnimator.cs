using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowChipAnimator : MonoBehaviour
{
    [SerializeField] bool startAnimation = false;
    [SerializeField] float animationSpeed = 1f;
    Vector2 startScale;

    float tolerance = 0.01f;
    bool resetScale = true;
    // Start is called before the first frame update
    void Start()
    {
        startScale = transform.localScale;
        Debug.Log($"StartScale = {startScale}");
    }

    // Update is called once per frame
    void Update()
    {
        if (!startAnimation) return;
        ResetScale();
        AnimateChip();
    }

    private void ResetScale()
    {
        if (!resetScale) return;
        transform.localScale = new Vector2(0, 0);
    }

    private void AnimateChip()
    {
        resetScale = false;
        float delta = animationSpeed * Time.deltaTime;
        transform.localScale = Vector2.Lerp(transform.localScale, startScale, delta);
        if (IsAnimationEnded())
        {
            startAnimation = false;
            resetScale = true;
            transform.localScale = startScale;
        }
    }

    private bool IsAnimationEnded()
    {
        return transform.localScale.x + tolerance > startScale.x &&
         transform.localScale.y + tolerance > startScale.y;
    }
}
