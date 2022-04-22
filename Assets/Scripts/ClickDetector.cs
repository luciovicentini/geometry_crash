using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDetector : MonoBehaviour
{
    [SerializeField] bool isSelected;
    [SerializeField] GameObject selectionSprite;
    private bool chipClicked;
    ChipSwitcher chipSwitcher;
    AnimatorManager animatorManager;

    GameObject chip;
    Coroutine selectChip;
    private void Awake()
    {
        chipSwitcher = GetComponentInParent<ChipSwitcher>();
        animatorManager = FindObjectOfType<AnimatorManager>();
    }

    private void Update() {
    
    }

    private void OnMouseDown()
    {
        if (!animatorManager.AnimationsFinished() && chipSwitcher.IsSwitchingChip()) return;
        if (selectChip != null) return;
        isSelected = !isSelected;
        chipClicked = true;
        chip = gameObject.FindChildWithTag("Chip");
        StartAnimationCoroutine();
        SoundManager.PlaySound(SoundManager.Sound.ChipSelected, chip.transform.position);
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
        if (chipClicked) chipSwitcher.SetChipClicked(chip);
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
