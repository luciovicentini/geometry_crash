using System;
using System.Collections;
using System.Collections.Generic;
using CustomUtil;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    [SerializeField] GameObject boardBackground;
    [SerializeField] float boardMargin = 1f;
    [SerializeField] int chipPerSizeAmount = 10;
    [SerializeField] GameObject holderChipPrefab;
    [SerializeField] List<GameObject> chipPrefabs;

    BoardManager boardManager;
    Camera cam;
    Vector2 startingPoint;
    float holderSize;
    private AnimatorManager animatorManager;

    bool hasFinishDrawingBoard = false;
    void Awake()
    {
        boardManager = FindObjectOfType<BoardManager>();
        animatorManager = FindObjectOfType<AnimatorManager>();
        cam = Camera.main;
    }

    void Start()
    {
        SetBoardSize();
        startingPoint = GetBoardBottomLeftPosition();
        holderSize = GetHolderSize();
        SetUpHolderChips();
        boardManager.SetUpBoard(chipPerSizeAmount, chipPerSizeAmount);
        boardManager.SetUpChips(chipPrefabs.Count);
        StartCoroutine(DrawBoard());
        StartCoroutine(boardManager.Check3MatchesAllPositions());
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

    private void SetUpHolderChips()
    {
        for (int row = 0; row < chipPerSizeAmount; row++)
        {
            for (int column = 0; column < chipPerSizeAmount; column++)
            {
                Vector2 position = CalculateChipPosition(startingPoint, holderSize, row, column);
                GameObject holderChip = InstantiateChipHolder(position);
                holderChip.name = GetHolderName(row, column);
            }
        }
    }

    private static Vector2 CalculateChipPosition(Vector2 startingPoint, float holderSize, int i, int j)
    {
        float xPosition = startingPoint.x + (holderSize * j) + holderSize / 2;
        float yPosition = startingPoint.y + (holderSize * i) + holderSize / 2;
        Vector2 position = new Vector2(xPosition, yPosition);
        return position;
    }

    private GameObject InstantiateChipHolder(Vector2 position)
    {
        GameObject holderChip = Instantiate(holderChipPrefab, position, Quaternion.identity, gameObject.transform);
        holderChip.transform.localScale = new Vector2(GetHolderSize(), GetHolderSize());
        return holderChip;
    }

    private IEnumerator DrawBoard()
    {
        hasFinishDrawingBoard = false;
        Debug.Log("DrawingBoard");
        for (int i = 0; i < boardManager.GetBoardYLength(); i++)
        {
            for (int j = 0; j < boardManager.GetBoardXLength(); j++)
            {
                Coord position = new Coord(i, j);
                Debug.Log($"DrawingBoard - SetupChipOn {position.ToString()}");
                SetUpInsideChip(boardManager.GetElementOnPosition(position), position.y, position.x);
                yield return new WaitForSeconds(0.01f);
            }
        }
        hasFinishDrawingBoard = true;
    }
    
    public void SetUpInsideChip(int chipIndex, int row, int column)
    {
        GameObject holderChip = GetHolderChipFromPosition(row, column);
        bool wasChipSelected = GetSelection(holderChip);
        RemoveChildren(holderChip);
        InstantiateInsideChip(holderChip, chipIndex, wasChipSelected);
    }

    internal void process3MLine(List<Coord> line)
    {
        foreach (Coord coord in line)
        {
            GameObject chip = GetHolderChipFromPosition(coord.y, coord.x).transform.GetChild(0).gameObject;
            animatorManager.AnimateChipHide(chip);
        }
    }

    private void RemoveChildren(GameObject holderChip)
    {
        for (int i = 0; i < holderChip.transform.childCount; i++)
        {
            Destroy(holderChip.transform.GetChild(i).gameObject);
        }
    }

    public GameObject GetHolderChipFromPosition(int row, int column)
    {
        return GameObject.Find(GetHolderName(row, column));
    }

    private string GetHolderName(int row, int column) => $"{row} - {column}";

    private void InstantiateInsideChip(GameObject holderChip, int chipIndex, bool isSelected)
    {
        GameObject insideChip = Instantiate(chipPrefabs[chipIndex], holderChip.transform.position, Quaternion.identity, holderChip.transform);
        insideChip.GetComponent<ChipSelection>().SetSelection(isSelected);
    }

    private bool GetSelection(GameObject holderChip)
    {
        ChipSelection cm = holderChip.GetComponentInChildren<ChipSelection>();
        if (cm == null) return false;
        return cm.GetSelection();
    }

    public string ChipValueToString(int value)
    {
        if (value == -1) return "Empty";
        return chipPrefabs[value].name;
    }

    public bool HasFinishDrawingBoard() => hasFinishDrawingBoard;
}
