using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipAnimator : MonoBehaviour
{
    AnimatorManager animationManager;

    private void Awake()
    {
        animationManager = FindObjectOfType<AnimatorManager>();
    }

    private void Start()
    {
        ShowChip();
    }

    public void ShowChip()
    {
        animationManager.AnimateChipShow(gameObject);
    }

    public void HideChip(bool shouldDestroy)
    {
        animationManager.AnimateChipHide(gameObject, shouldDestroy);
    }
}
