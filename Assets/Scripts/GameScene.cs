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

    BoardManager boardManager;
    Camera cam;

    void Awake()
    {
        boardManager = FindObjectOfType<BoardManager>();
        boardManager.SetUpBoard(chipPerSizeAmount, chipPerSizeAmount);
        cam = Camera.main;
    }

    void Start()
    {
        SetBoardSize();
        DrawStartingBoard();
    }

    void SetBoardSize()
    {
        float smallerScreenSideSize = IsWidthSmallerScreenSize() ? GetWidthScreenSize() : GetHeightScreenSize();
        float boardSize = GetBoardSize(smallerScreenSideSize);
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
        for (int i = 0; i < chipPerSizeAmount; i++)
        {
            for (int j = 0; j < chipPerSizeAmount; j++)
            {
                Vector3 position = CalculateChipPosition(startingPoint, holderSize, i, j);
                GameObject holderChip = InstantiateChipHolder(position);
                holderChip.name = $"{i} - {j}";

                int randomChipIndex = GetRandomChipIndex();
                InstantiateInsideChip(holderChip, randomChipIndex);
                boardManager.SetElementOnPosition(randomChipIndex, new CustomUtil.Coord(i, j));
            }
        }
    }

    public int GetRandomChipIndex()
    {
        return UnityEngine.Random.Range(0, chipPrefabs.Count);
    }

    private static Vector3 CalculateChipPosition(Vector2 startingPoint, float holderSize, int i, int j)
    {
        float xPosition = startingPoint.x + (holderSize * j) + holderSize / 2;
        float yPosition = startingPoint.y + (holderSize * i) + holderSize / 2;
        Vector3 position = new Vector3(xPosition, yPosition, 0);
        return position;
    }

    private GameObject InstantiateChipHolder(Vector3 position)
    {
        GameObject holderChip = Instantiate(holderChipPrefab, position, Quaternion.identity, gameObject.transform);
        holderChip.transform.localScale = new Vector2(GetHolderSize(), GetHolderSize());
        return holderChip;
    }

    private void InstantiateInsideChip(GameObject holderChip, int chipIndex)
    {
        GameObject insideChip = Instantiate(chipPrefabs[chipIndex], holderChip.transform.position, Quaternion.identity, holderChip.transform);
        insideChip.transform.localScale = new Vector2(
            insideChip.transform.localScale.x * holderPaddingScale,
            insideChip.transform.localScale.y * holderPaddingScale);
    }

    public string ChipValueToString(int value)
    {
        if (value == -1) return "Empty";
        return chipPrefabs[value].name;
    }
}
