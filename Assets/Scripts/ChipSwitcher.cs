using System.Collections;
using System.Collections.Generic;
using CustomUtil;
using UnityEngine;

public class ChipSwitcher : MonoBehaviour
{
    ChipManager chip1;

    ChipManager chip2;

    BoardManager boardManager;

    private void Awake()
    {
        boardManager = FindObjectOfType<BoardManager>();
    }


    public void SetChipClicked(ChipManager selectedChip)
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
                boardManager.SwitchChips(GetCoordFromChip(chip1), GetCoordFromChip(chip2));
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

    private bool AreContinuous(ChipManager chip1, ChipManager chip2)
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

    private int GetYCoordFromName(string name)
    {
        int dividerIndex = GetDividerIndex(name);
        return int.Parse(name.Substring(0, dividerIndex).Trim());
    }

    private int GetXCoordFromName(string name)
    {
        int dividerIndex = GetDividerIndex(name);
        string s = name.Substring(dividerIndex + 1, name.Length - dividerIndex - 1).Trim();
        return int.Parse(s);
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

    private Coord GetCoordFromChip(ChipManager chip)
    {
        string name = chip.transform.parent.gameObject.name;
        
        int xCoord = GetXCoordFromName(name);
        int yCoord = GetYCoordFromName(name);
        
        return new Coord(yCoord, xCoord);
    }
}
