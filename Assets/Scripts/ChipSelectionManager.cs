using System;
using System.Collections;
using System.Collections.Generic;
using CustomUtil;
using UnityEngine;

public class ChipSelectionManager : MonoBehaviour
{
    [SerializeField] ChipHolderSelect chip1;
    [SerializeField] ChipHolderSelect chip2;
    [SerializeField] bool processChipSelection = false;

    ChipSwitcher chipSwitcher;

    private void Awake() {
        chipSwitcher = FindObjectOfType<ChipSwitcher>();
    }

    private void Update()
    {
        if (processChipSelection)
        {
            ProcessChipSelection();
        }
        processChipSelection = false;
    }

    internal void SetChip(ChipHolderSelect chipTouched)
    {
        chipTouched.ToogleSelected();
        if (chip1 == null) chip1 = chipTouched;
        else SetChip2OrDeselectChip1(chipTouched);
    }

    private void SetChip2OrDeselectChip1(ChipHolderSelect chipTouched)
    {
        if (chip1 == chipTouched)
        {
            chip1 = null;
        }
        else
        {
            chip2 = chipTouched;
            processChipSelection = true;
        }
    }

    private void ProcessChipSelection()
    {
        Coord coordChip1 = Coord.GetCoordFromChipHolderName(chip1.gameObject.name);
        Coord coordChip2 = Coord.GetCoordFromChipHolderName(chip2.gameObject.name);
        if (AreContinuous(coordChip1, coordChip2))
        {
            StartCoroutine(chipSwitcher.SwitchChips(coordChip1, coordChip2));
            ForgetSelectedChips();
        }
        else
        {
            chip1.ResetSelection();
            chip1 = chip2;
            chip2 = null;
        }
    }

    private bool AreContinuous(Coord chip1Coords, Coord chip2Coords)
    {
        if ((chip1Coords.x + 1 == chip2Coords.x
            || chip1Coords.x - 1 == chip2Coords.x)
            && chip1Coords.y == chip2Coords.y)
        {
            return true;
        }
        if ((chip1Coords.y + 1 == chip2Coords.y
            || chip1Coords.y - 1 == chip2Coords.y)
            && chip1Coords.x == chip2Coords.x)
        {
            return true;
        }
        return false;
    }

    private void ForgetSelectedChips()
    {
        if (chip1 != null)
        {
            chip1.ResetSelection();
            chip1 = null;
        }

        if (chip2 != null)
        {
            chip2.ResetSelection();
            chip2 = null;
        }
    }
}
