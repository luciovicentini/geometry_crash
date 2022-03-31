using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDetector : MonoBehaviour
{
    ChipSelection chip;
    ChipSwitcher chipSwitcher;

    private void Awake() {
        chipSwitcher = GetComponentInParent<ChipSwitcher>();
    }

    private void OnMouseDown() 
    {
        chip = GetComponentInChildren<ChipSelection>();
        if (chip == null) return;
        
        
        // chip.ToggleSelection();
        // chipSwitcher.SetChipClicked(chip);
    }
}
