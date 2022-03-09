using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipSwitcher : MonoBehaviour
{
    ChipManager chip1;

    ChipManager chip2;   
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
                SwitchChips(chip1, chip2);
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
        string chipHolder1Name = chip1.transform.parent.gameObject.name;
        string chipHolder2Name = chip2.transform.parent.gameObject.name;
        int chipHolder1XCoord = GetXCoordFromName(chipHolder1Name);
        int chipHolder1YCoord = GetYCoordFromName(chipHolder1Name);
        int chipHolder2XCoord = GetXCoordFromName(chipHolder2Name);
        int chipHolder2YCoord = GetYCoordFromName(chipHolder2Name);
        
        if ((chipHolder1XCoord + 1 == chipHolder2XCoord
            || chipHolder1XCoord - 1 == chipHolder2XCoord) 
            && chipHolder1YCoord == chipHolder2YCoord)
        {
            return true;
        }
        if ((chipHolder1YCoord + 1 == chipHolder2YCoord
            || chipHolder1YCoord - 1 == chipHolder2YCoord) 
            && chipHolder1XCoord == chipHolder2XCoord)
        {
            return true;
        }
        return false;
    }

    private int GetXCoordFromName(string name)
    {
        int dividerIndex = GetDividerIndex(name);
        return int.Parse(name.Substring(0, dividerIndex).Trim());
    }

    private int GetYCoordFromName(string name)
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
        chip1 = null;
        chip2 = null;
    }

    private void SwitchChips(ChipManager chip1, ChipManager chip2)
    {
        GameObject parent1 = chip1.gameObject.transform.parent.gameObject;
        GameObject parent2 = chip2.gameObject.transform.parent.gameObject;
        chip1.transform.SetParent(parent2.transform);
        chip2.transform.SetParent(parent1.transform);
        chip1.transform.localPosition = Vector2.zero;
        chip2.transform.localPosition = Vector2.zero;
        chip1.ResetSelection();
        chip2.ResetSelection();
    }
}
