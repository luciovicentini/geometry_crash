using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDetector : MonoBehaviour
{
    ChipManager chip;
    ChipSwitcher chipSwitcher;

    private void Awake() {
        chipSwitcher = GetComponentInParent<ChipSwitcher>();
    }

    private void OnMouseDown() 
    {
        chip = GetComponentInChildren<ChipManager>();
        if (chip == null) return;
        chip.ToggleSelection();
        chipSwitcher.SetChipClicked(chip);
    }
}
