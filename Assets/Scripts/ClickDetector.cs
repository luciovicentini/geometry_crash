using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDetector : MonoBehaviour
{
    [SerializeField] bool isSelected;
    private bool chipClicked;
    ChipSwitcher chipSwitcher;
    AnimatorManager animatorManager;
    GameObject selectionSprite;

    Coroutine selectChip;
    private void Awake()
    {
        chipSwitcher = GetComponentInParent<ChipSwitcher>();
        animatorManager = FindObjectOfType<AnimatorManager>();
        selectionSprite = gameObject.transform.Find("SelectionSprite").gameObject;
    }

    private void OnMouseDown()
    {
        if (selectChip != null) return;
        isSelected = !isSelected;
        chipClicked = true;
        StartAnimationCoroutine();
    }

    private void StartAnimationCoroutine()
    {
        if (selectChip != null) return;
        selectChip = StartCoroutine(ToggleSelection());
    }

    private IEnumerator ToggleSelection()
    {
        StartAnimation();
        yield return new WaitForSeconds(animatorManager.GetSelectAnimationTime());
        if (chipClicked) chipSwitcher.SetChipClicked(gameObject.FindChildWithTag("Chip"));
        selectChip = null;
        chipClicked = false;
    }

    private void StartAnimation()
    {
        if (isSelected)
        {
            animatorManager.AnimateShowSelectionSprite(selectionSprite);
        }
        else
        {
            animatorManager.AnimateHideSelectionSprite(selectionSprite);
        }
    }

    internal void ResetSelection() {
        SetSelection(false);
    } 

    internal void SetSelection(bool isSelected)
    {
        this.isSelected = isSelected;
        StartAnimationCoroutine();
    }

    internal bool GetSelection() => isSelected;
}
