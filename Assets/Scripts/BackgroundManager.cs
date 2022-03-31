using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{

    [SerializeField] float boardMargin = 1f;

    private void Start() {
        SetBoardSize();
    }
    
    internal void SetBoardSize()
    {
        float smallerScreenSideSize = IsWidthSmallerScreenSize() ? GetWidthScreenSize() : GetHeightScreenSize();
        float boardSize = GetBoardSize(smallerScreenSideSize);
        gameObject.transform.localScale = new Vector2(boardSize, boardSize);
        gameObject.transform.position = new Vector2(0, 0);
    }

    public float GetBoardSizeFloat()
    {
        return gameObject.transform.localScale.x;
    }

    private float GetBoardSize(float smallerScreenSideSize)
    {
        return smallerScreenSideSize - (boardMargin * 2);
    }

    private Vector2 GetScreenSizeVector()
    {
        Vector2 topRightCorner = new Vector2(1, 1);
        Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);
        return edgeVector;
    }

    public Vector2 GetBoardBottomLeftPosition()
    {
        return new Vector2(gameObject.transform.localScale.x / 2 * -1, gameObject.transform.localScale.y / 2 * -1);
    }

    bool IsWidthSmallerScreenSize()
    {
        return GetWidthScreenSize() < GetHeightScreenSize();
    }

    float GetWidthScreenSize()
    {
        return GetScreenSizeVector().x * 2;
    }

    float GetHeightScreenSize()
    {
        return GetScreenSizeVector().y * 2;
    }

}
