using System;
using System.Collections;
using System.Collections.Generic;
using CustomUtil;
using UnityEngine;

public class ChipSwitcher : MonoBehaviour
{
    ChipSelection chip1;

    ChipSelection chip2;

    BoardManager boardManager;

    private void Awake()
    {
        boardManager = FindObjectOfType<BoardManager>();
    }

    public void SetChipClicked(ChipSelection selectedChip)
    {
        if (chip1 == null)
        {
            chip1 = selectedChip;
        }
        else
        {
            if (chip1 == selectedChip)
            {
                chip1 = null;
            }
            else
            {
                chip2 = selectedChip;
            }
        }

        if (chip1 != null && chip2 != null)
        {
            if (AreContinuous(chip1, chip2))
            {
                Coord coordChip1 = GetCoordFromChip(chip1);
                Coord coordChip2 = GetCoordFromChip(chip2);
                chip1.GetComponent<HideChipAnimation>().StartAnimation();
                chip2.GetComponent<HideChipAnimation>().StartAnimation();
                boardManager.SwitchChips(coordChip1, coordChip2);

                chip1.GetComponent<ShowChipAnimator>().StartAnimation();
                chip2.GetComponent<ShowChipAnimator>().StartAnimation();

                // if (HasA3MatchFormed(coordChip1, coordChip2))
                // {
                    // TODO antes de checkear si hay matches deberia hacer la animacion de las dos chips.

                    
                    // boardManager.Check3Matches();
                // }
                // else
                // {
                    // boardManager.SwitchChips(coordChip2, coordChip1);
                // }
                ForgetSelectedChips();
            }
            else
            {
                chip1.ResetSelection();
                chip1 = selectedChip;
                chip2 = null;
            }
        }
    }

    // private bool HasA3MatchFormed(Coord coordChip1, Coord coordChip2) =>
    //     boardManager.CheckCoordMade3Match(coordChip1)
    //     || boardManager.CheckCoordMade3Match(coordChip2);

    private bool AreContinuous(ChipSelection chip1, ChipSelection chip2)
    {
        Coord chip1Coords = GetCoordFromChip(chip1);
        Coord chip2Coords = GetCoordFromChip(chip2);

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

    private static int GetDividerIndex(string name)
    {
        return name.IndexOf("-");
    }

    private void ForgetSelectedChips()
    {
        chip1.ResetSelection();
        chip1 = null;

        chip2.ResetSelection();
        chip2 = null;
    }

    private Coord GetCoordFromChip(ChipSelection chip) => Coord.GetCoordFromChipHolderName(chip.transform.parent.gameObject.name);
}
