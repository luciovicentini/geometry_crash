using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipAnimator : MonoBehaviour
{

    [SerializeField] bool shouldStartAnim = false;
    Animator chipAnimator;
    AnimatorManager animationManager;

    private void Start()
    {
        animationManager = FindObjectOfType<AnimatorManager>();
        chipAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!ShouldStartAnimation()) return;
        StartChipAnimation();
    }

    private void StartChipAnimation()
    {
        chipAnimator.SetTrigger("StartAnimation");
        shouldStartAnim = false;
    }

    private bool ShouldStartAnimation()
    {
        return shouldStartAnim ||  animationManager.ShouldStartAnimation();
    }
}
