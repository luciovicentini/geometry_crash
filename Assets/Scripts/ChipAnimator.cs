using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipAnimator : MonoBehaviour
{

    [SerializeField] bool shouldStartAnim = false;

    // Animator chipAnimator;
    AnimatorManager animationManager;

    private void Awake()
    {
        animationManager = FindObjectOfType<AnimatorManager>();

        // chipAnimator = GetComponent<Animator>();

    }

    private void Start()
    {
        ShowChip();
    }

    public void ShowChip()
    {
        animationManager.AnimateChipShow(gameObject);
    }

    public void HideChip()
    {
        animationManager.AnimateChipHide(gameObject);
    }

    /* void Update()
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
        return shouldStartAnim || animationManager.ShouldStartAnimation();
    } */
}
