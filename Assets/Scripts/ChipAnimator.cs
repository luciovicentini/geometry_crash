using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipAnimator : MonoBehaviour
{
    [SerializeField] bool showPlayAnimation = false;
    Animator chipAnimator;
    AnimatorManager animatorManager;
    private void Start()
    {
        chipAnimator = GetComponent<Animator>();
        animatorManager = FindObjectOfType<AnimatorManager>();
    }

    void Update()
    {
        if (chipAnimator == null) return;
        if (ShouldPlayAnimation())
        {
            animatorManager.SetAnimation(chipAnimator);
        }
    }

    private bool ShouldPlayAnimation()
    {
        return showPlayAnimation || UnityEngine.Random.Range(0, 10000) == 1000;
    }
}
