using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipManager : MonoBehaviour
{
    [SerializeField] bool isSelected = false;

    SpriteRenderer chipRenderer;
    Color backgroundColor;

    Color originalColor;

    private void Awake()
    {
        backgroundColor = Color.white;
        chipRenderer = GetComponent<SpriteRenderer>();
        SaveOriginalColor();
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
            backgroundColor = originalColor;
        }
    }

    private void SaveOriginalColor()
    {
        if (chipRenderer == null) return;

        originalColor = chipRenderer.color;
    }

    public void ToggleSelection()
    {
        isSelected = !isSelected;
    }

    public void ResetSelection()
    {
        isSelected = false;
    }

    internal void SetSelection(bool v)
    {
        isSelected = v;
    }

    internal bool GetSelection() => isSelected;
   
}
