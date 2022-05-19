using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipHolderSelect : MonoBehaviour
{
    [SerializeField] bool isSelected = false;
    [SerializeField] GameObject selectionSprite;
    [SerializeField] bool processSelected;

    ChipSwitcher chipSwitcher;
    AnimatorManager animatorManager;

    GameObject chip;
    Coroutine animateSelection;

    private void Awake()
    {
        chipSwitcher = GetComponentInParent<ChipSwitcher>();
        animatorManager = FindObjectOfType<AnimatorManager>();
    }

    private void Update()
    {
        if (processSelected)
        {
            ProcessSelection();
        }
        processSelected = false;
    }

    private void ProcessSelection()
    {
        if (!animatorManager.AnimationsFinished() || chipSwitcher.IsSwitchingChip()) return;
        if (animateSelection != null) return;
        chip = gameObject.FindChildWithTag("Chip");

        SoundManager.PlaySound(SoundManager.Sound.ChipSelected, chip.transform.position);
        animateSelection = StartCoroutine(AnimateSelection());

    }

    private IEnumerator AnimateSelection()
    {
        SendAnimation();
        yield return new WaitForSeconds(AnimatorManager.GetSelectAnimationTime());
        animateSelection = null;
    }

    private void SendAnimation()
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

    internal void ToogleSelected()
    {
        SetSelection(!isSelected);
    }

    internal void ResetSelection()
    {
        SetSelection(false);
    }

    internal void SetSelection(bool isSelected)
    {
        this.isSelected = isSelected;
        processSelected = true;
    }

    internal bool GetSelection() => isSelected;
}
