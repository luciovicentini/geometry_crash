using System;
using System.Collections;
using System.Collections.Generic;
using CustomUtil;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    [SerializeField] GameObject boardBackground;
    [SerializeField] int chipPerSizeAmount = 10;
    [SerializeField] GameObject holderChipPrefab;
    [SerializeField] List<GameObject> chipPrefabs;

    BoardManager boardManager;

    AnimatorManager animatorManager;
    BackgroundManager backgroundManager;
    bool hasFinishDrawingBoard = false;

    void Awake()
    {
        boardManager = FindObjectOfType<BoardManager>();
        animatorManager = FindObjectOfType<AnimatorManager>();
        backgroundManager = FindObjectOfType<BackgroundManager>();
    }

    void Start()
    {
        SetUpHolderChips();
        StartCoroutine(DrawBoard());
    }

    private float GetHolderSize()
    {
        return backgroundManager.GetBoardSizeFloat() / chipPerSizeAmount;
    }

    private void SetUpHolderChips()
    {
        Vector2 startingPoint = backgroundManager.GetBoardBottomLeftPosition();
        float holderSize = GetHolderSize();
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

    internal void SwitchChips(Coord coordChip1, Coord coordChip2)
    {
        GameObject parentChip1 = GetHolderChipFromPosition(coordChip1);
        GameObject parentChip2 = GetHolderChipFromPosition(coordChip2);
        GameObject chip1 = GetChip(coordChip1);
        GameObject chip2 = GetChip(coordChip2);
        SetChipParent(chip1, parentChip2);
        SetChipParent(chip2, parentChip1);
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
        boardManager.RandomizeBoard();
        hasFinishDrawingBoard = false;
        Debug.Log("DrawingBoard");
        for (int i = 0; i < boardManager.GetBoardYLength(); i++)
        {
            for (int j = 0; j < boardManager.GetBoardXLength(); j++)
            {
                Coord position = new Coord(i, j);
                Debug.Log($"DrawingBoard - SetupChipOn {position.ToString()}");
                SetUpInsideChip(boardManager.GetElementOnPosition(position), position.y, position.x);
                yield return null/* new WaitForSeconds(animatorManager.GetCreateAnimationTime()) */;
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
        hasFinishDrawingBoard = false;
        if (Coord.GetLineType(line) == LineType.Horizontal)
        {
            List<GameObject> chipsToAnimate = GetChipsAboveLine(line);
            StartCoroutine(AnimateFallingChips(line, chipsToAnimate, 1));
        }
        if (Coord.GetLineType(line) == LineType.Vertical)
        {
            List<GameObject> chipsToAnimate = GetChipsAboveCoord(Coord.GetUpperCoord(line));
            StartCoroutine(AnimateFallingChips(line, chipsToAnimate, 3));
        }
    }

    private IEnumerator AnimateFallingChips(List<Coord> line, List<GameObject> chipsToAnimate, int rows)
    {
        AnimateDestroyingChips(line);
        yield return new WaitForSeconds(animatorManager.GetDestroyAnimationTime());

        animatorManager.AnimateFallingChips(chipsToAnimate, rows);
        yield return new WaitForSeconds(animatorManager.GetFallingAnimationTime());

        SwitchChipHolder(chipsToAnimate, rows);
        ShowNewChips(GetLastCoords(line));
        yield return new WaitForSeconds(animatorManager.GetCreateAnimationTime());
        hasFinishDrawingBoard = true;
    }

    private void AnimateDestroyingChips(List<Coord> line)
    {
        foreach (Coord coord in line)
        {
            GameObject chip = GetChip(coord);
            animatorManager.AnimateChipHide(chip, true);
        }
    }

    private List<Coord> GetLastCoords(List<Coord> line)
    {
        List<Coord> coords = new List<Coord>();
        for (int i = 1; i < line.Count + 1; i++)
        {
            int delta = Coord.GetLineType(line) == LineType.Horizontal ? 1 : i;
            coords.Add(new Coord(chipPerSizeAmount - delta, line[i - 1].x));
        }
        return coords;
    }

    private void SwitchChipHolder(List<GameObject> chips, int amount)
    {
        foreach (GameObject chip in chips)
        {
            Coord oldParentCoord = Coord.GetCoordFromChipHolderName(chip.transform.parent.gameObject.name);
            GameObject newParent = GetHolderChipFromPosition(oldParentCoord.y - amount, oldParentCoord.x);
            SetChipParent(chip, newParent);
        }
    }

    private void SetChipParent(GameObject chip, GameObject newParent)
    {
        chip.transform.SetParent(newParent.transform);
        chip.transform.localPosition = new Vector2(0, 0);
    }

    private void ShowNewChips(List<Coord> coords)
    {
        foreach (Coord coord in coords)
        {
            GameObject holder = GetHolderChipFromPosition(coord.y, coord.x);
            int chipIndex = boardManager.GetElementOnPosition(coord);
            InstantiateInsideChip(holder, chipIndex, false);
        }
    }

    private void RemoveChildren(GameObject holderChip)
    {
        for (int i = 0; i < holderChip.transform.childCount; i++)
        {
            Destroy(holderChip.transform.GetChild(i).gameObject);
        }
    }
    public GameObject GetHolderChipFromPosition(int row, int column) => 
        gameObject.transform.Find(GetHolderName(row, column)).gameObject;
    public GameObject GetHolderChipFromPosition(Coord coord) =>
        GetHolderChipFromPosition(coord.y, coord.x);
    

    internal GameObject GetChip(Coord coord) => GetHolderChipFromPosition(coord.y, coord.x)
                                                        .transform.GetChild(0).gameObject;

    private string GetHolderName(int row, int column) => $"{row} {Coord.NAME_DIVIDER} {column}";

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

    private List<GameObject> GetChipsAboveLine(List<Coord> line)
    {
        List<GameObject> items = new List<GameObject>();

        foreach (Coord coord in line)
        {
            items.AddRange(GetChipsAboveCoord(coord));
        }

        return items;
    }

    private List<GameObject> GetChipsAboveCoord(Coord coord)
    {
        List<GameObject> items = new List<GameObject>();

        for (int i = coord.y + 1; i < chipPerSizeAmount; i++)
        {
            items.Add(GetChip(new Coord(i, coord.x)));
        }
        return items;
    }

    public int GetChipPerSizeAmount() => chipPerSizeAmount;

    public int GetAmountOfChipElements() => chipPrefabs.Count;
}
