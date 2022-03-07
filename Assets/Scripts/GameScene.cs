using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    [SerializeField] GameObject boardBackground;
    [SerializeField] float boardMargin = 1f;
    [SerializeField] int chipPerSizeAmount = 10;

    [SerializeField][Range(0f, 1f)] float holderPaddingScale = 0.9f;

    [SerializeField] GameObject holderChipPrefab;
    [SerializeField] List<GameObject> chipPrefabs;

    Camera cam;


    void Start()
    {
        cam = Camera.main;

        SetBoardSize();
        DrawStartingBoard();
    }

    void SetBoardSize()
    {
        float smallerScreenSideSize = IsWidthSmallerScreenSize() ? GetWidthScreenSize() : GetHeightScreenSize();
        Debug.Log(smallerScreenSideSize);
        float boardSize = GetBoardSize(smallerScreenSideSize);
        Debug.Log("board size = " + boardSize);
        boardBackground.transform.localScale = new Vector2(boardSize, boardSize);
        boardBackground.transform.position = new Vector2(0, 0);
    }

    private float GetHolderSize()
    {
        return GetBoardSizeFloat() / chipPerSizeAmount;
    }

    private float GetBoardSizeFloat()
    {
        return boardBackground.transform.localScale.x;
    }

    void Update()
    {

    }

    private float GetBoardSize(float smallerScreenSideSize)
    {
        return smallerScreenSideSize - (boardMargin * 2);
    }

    private Vector2 GetScreenSizeVector()
    {
        Vector2 topRightCorner = new Vector2(1, 1);
        Vector2 edgeVector = cam.ViewportToWorldPoint(topRightCorner);
        return edgeVector;
    }

    private Vector2 GetBoardBottomLeftPosition()
    {
        return new Vector2(boardBackground.transform.localScale.x / 2 * -1, boardBackground.transform.localScale.y / 2 * -1);
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

    void DrawStartingBoard()
    {
        Vector2 startingPoint = GetBoardBottomLeftPosition();
        float holderSize = GetHolderSize();
        Debug.Log("holder size = " + holderSize);
        for (int i = 0; i < chipPerSizeAmount; i++)
        {
            for (int j = 0; j < chipPerSizeAmount; j++)
            {
                float xPosition = startingPoint.x + (holderSize * j) + holderSize / 2;
                float yPosition = startingPoint.y + (holderSize * i) + holderSize / 2;
                Vector3 position = new Vector3(xPosition, yPosition, 0);
                InstantiateChip(position);
            }
        }
    }

    public void InstantiateChip(Vector3 position)
    {
        GameObject holderChip = Instantiate(holderChipPrefab, position, Quaternion.identity, gameObject.transform);
        holderChip.transform.localScale = new Vector2(GetHolderSize(), GetHolderSize());
        
        int randomChipIndex = UnityEngine.Random.Range(0, chipPrefabs.Count); 
        GameObject insideChip = Instantiate(chipPrefabs[randomChipIndex], holderChip.transform.position, Quaternion.identity, holderChip.transform);
        insideChip.transform.localScale = new Vector2(
            insideChip.transform.localScale.x * holderPaddingScale,
            insideChip.transform.localScale.y * holderPaddingScale);
    }

}
