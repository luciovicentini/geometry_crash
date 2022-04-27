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
    Coroutine selectChipAnimationCR;

    private void Awake()
    {
        chipSwitcher = GetComponentInParent<ChipSwitcher>();
        animatorManager = FindObjectOfType<AnimatorManager>();
        ObjectDetector.OnChipClicked += OnClicked;
    }

    private void OnClicked(String chipName)
    {
        if (chipName == this.gameObject.name)
        {
            if (!animatorManager.AnimationsFinished() && chipSwitcher.IsSwitchingChip()) return;
            if (selectChipAnimationCR != null) return;
            isSelected = !isSelected;
            chipClicked = true;
            chip = gameObject.FindChildWithTag("Chip");
            StartAnimationCoroutine();
            SoundManager.PlaySound(SoundManager.Sound.ChipSelected, chip.transform.position);
        }
    }

    private void StartAnimationCoroutine()
    {
        if (selectChipAnimationCR != null) return;
        selectChipAnimationCR = StartCoroutine(ToggleSelection());
    }

    private IEnumerator ToggleSelection()
    {
        StartAnimation();
        yield return new WaitForSeconds(animatorManager.GetSelectAnimationTime());
        if (chipClicked) chipSwitcher.SetChipClicked(chip);
        selectChipAnimationCR = null;
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

    internal void ResetSelection()
    {
        SetSelection(false);
    }

    internal void SetSelection(bool isSelected)
    {
        this.isSelected = isSelected;
        StartAnimationCoroutine();
    }

    internal bool GetSelection() => isSelected;
}
