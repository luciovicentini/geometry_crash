using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipManager : MonoBehaviour
{
    [SerializeField] bool isSelected = false;

    SpriteRenderer chipRenderer;
    Color backgroundColor;

    private void Awake()
    {
        backgroundColor = Color.white;
        chipRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        UpdateBackgroundColor();
        UpdateBackground();
    }

    private void UpdateBackground()
    {
        if (chipRenderer == null) return;

        chipRenderer.color = backgroundColor;
    }

    private void UpdateBackgroundColor()
    {
        if (isSelected)
        {
            backgroundColor = Color.red;
        }
        else
        {
            backgroundColor = Color.white;
        }
    }

    public void ToggleSelection()
    {
        isSelected = !isSelected;
    }

    public void ResetSelection()
    {
        isSelected = false;
    }
}
