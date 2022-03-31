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
        boardManager.SetUpBoard(chipPerSizeAmount, chipPerSizeAmount);
        boardManager.SetUpChips(chipPrefabs.Count);
        StartCoroutine(DrawBoard());
        StartCoroutine(boardManager.Check3MatchesAllPositions());
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
            animatorManager.AnimateChipHide(chip, true);
        }

        /*  TODO: Dependiendo de si la linea es hor o vert se tienen que borrar las fichas de 
            arriba y mostrarlas en la fila inferior desde la fila actual hasta la ultima fila.

            Una vez que se termina de hacer toda la animación se tiene que continuar analizando el resto de las coordenadas.
        */
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
