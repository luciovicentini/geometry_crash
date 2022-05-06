using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{

    [SerializeField] float boardMargin = 1f;

    float boardSize;

    private void Start()
    {
        SetBoardSize();
    }

    private void SetBoardSize()
    {
        float smallerScreenSideSize = IsWidthSmallerScreenSize() ? GetWidthScreenSize() : GetHeightScreenSize();
        GetBoardSize(smallerScreenSideSize);
        gameObject.transform.localScale = new Vector2(boardSize, boardSize);
        gameObject.transform.position = GetBoardBottomPosition();
    }

    private Vector2 GetBoardBottomPosition()
    {
        return new Vector2(0, GetYBoardPosition());
    }

    private float GetYBoardPosition()
    {
        float screenHeight = GetHeightScreenSize();
        float deltaScreenBoard = screenHeight - boardSize;
        return deltaScreenBoard / 2 * -1 + boardMargin;
    }

    public float GetBoardSizeFloat()
    {
        return gameObject.transform.localScale.x;
    }

    private void GetBoardSize(float smallerScreenSideSize)
    {
        boardSize = smallerScreenSideSize - (boardMargin * 2);
    }

    private Vector2 GetScreenSizeVector()
    {
        Vector2 topRightCorner = new Vector2(1, 1);
        Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);
        return edgeVector;
    }

    public Vector2 GetBoardBottomLeftPosition()
    {
        return new Vector2(gameObject.transform.localScale.x / 2 * -1,
            gameObject.transform.localScale.y / 2 * -1 + GetYBoardPosition());
    }

    private bool IsWidthSmallerScreenSize()
    {
        return GetWidthScreenSize() < GetHeightScreenSize();
    }

    private float GetWidthScreenSize()
    {
        return GetScreenSizeVector().x * 2;
    }

    private float GetHeightScreenSize()
    {
        return GetScreenSizeVector().y * 2;
    }
}
