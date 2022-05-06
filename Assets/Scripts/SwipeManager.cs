using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeManager : MonoBehaviour
{
    BackgroundManager backgroundManager;
    ChipSwitcher chipSwitcher;
    Camera main;

    void Awake()
    {
        backgroundManager = FindObjectOfType<BackgroundManager>();
        chipSwitcher = FindObjectOfType<ChipSwitcher>();
        main = Camera.main;
    }

    private void OnEnable() {
        SwipeDetector.OnSwipe += OnSwipe;
    }

    private void OnDisable() {
        SwipeDetector.OnSwipe -= OnSwipe;
    }

    private void OnSwipe(SwipeData data)
    {
        Vector2 swipeStartPositionWorld = main.ScreenToWorldPoint(data.StartPosition);
        if (!IsSwipeInsideBoard(swipeStartPositionWorld)) return;
        chipSwitcher.HandleSwiping(data.Direction);
        
    }

    private bool IsSwipeInsideBoard(Vector2 swipePosition) {
        Vector2 bottomLeftCorner = backgroundManager.GetBoardBottomLeftPosition();
        float boardSize = backgroundManager.GetBoardSizeFloat();
        Vector2 topRightCorner = new Vector2(bottomLeftCorner.x + boardSize, bottomLeftCorner.y + boardSize);

        if (swipePosition.x < bottomLeftCorner.x || 
            swipePosition.x > topRightCorner.x || 
            swipePosition.y < bottomLeftCorner.y ||
            swipePosition.y > topRightCorner.y) return false;
        return true;
    }
}
