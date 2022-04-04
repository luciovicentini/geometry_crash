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
    private readonly string NAME_DIVIDER = "-";

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

        if (Coord.GetLineType(line) == LineType.Horizontal)
        {
            StartCoroutine(AnimateHorizontalFalling(line));
        }
        if (Coord.GetLineType(line) == LineType.Vertical)
        {
            StartCoroutine(AnimateVerticalFalling(line));
            Debug.Log($"{line.ToString()} is Vertical");
        }
        /*  TODO: Dependiendo de si la linea es hor o vert se tienen que borrar las fichas de 
            arriba y mostrarlas en la fila inferior desde la fila actual hasta la ultima fila.

            Una vez que se termina de hacer toda la animación se tiene que continuar analizando el resto de las coordenadas.
        */
    }

    private void AnimateDestroyingChips(List<Coord> line)
    {
        Debug.Log("Animating destroying Chips");
        foreach (Coord coord in line)
        {
            GameObject chip = GetChip(coord);
            animatorManager.AnimateChipHide(chip, true);
        }

    }

    private IEnumerator AnimateHorizontalFalling(List<Coord> line)
    {
        AnimateDestroyingChips(line);
        yield return new WaitForSeconds(animatorManager.GetDestroyAnimationTime());

        Debug.Log("Animating Falling Chips");
        List<GameObject> chipsToAnimate = GetChipsAboveLine(line);
        animatorManager.AnimateFallingChips(chipsToAnimate, 1);
        yield return new WaitForSeconds(animatorManager.GetFallingAnimationTime());

        SwitchChipHolder(chipsToAnimate, 1);
    }

    private IEnumerator AnimateVerticalFalling(List<Coord> line)
    {
        AnimateDestroyingChips(line);
        yield return new WaitForSeconds(animatorManager.GetDestroyAnimationTime());

        Debug.Log("Animating Falling Chips");
        List<GameObject> chipsToAnimate = GetChipsAboveCoord(Coord.GetUpperCoord(line));
        animatorManager.AnimateFallingChips(chipsToAnimate, 3);
        yield return new WaitForSeconds(animatorManager.GetFallingAnimationTime());

        SwitchChipHolder(chipsToAnimate, 3);
    }

    private void SwitchChipHolder(List<GameObject> chips, int amount)
    {
        foreach (GameObject chip in chips)
        {
            GameObject chipHolderOld = chip.transform.parent.gameObject;
            string oldParentName = chipHolderOld.name;
            Coord oldParentCoord = GetCoordFromChipHolderName(oldParentName);
            GameObject newParent = GetHolderChipFromPosition(oldParentCoord.y - amount, oldParentCoord.x);
            chip.transform.SetParent(newParent.transform);
            chip.transform.localPosition = new Vector2(0, 0);
        }
    }

    private Coord GetCoordFromChipHolderName(string oldParentName)
    {
        return new Coord(GetYCoordFromName(oldParentName), GetXCoordFromName(oldParentName));
    }

    private int GetYCoordFromName(string oldParentName)
    {
        int dividerIndex = oldParentName.IndexOf(NAME_DIVIDER);
        return int.Parse(oldParentName.Substring(0, dividerIndex - 1));
    }

    private int GetXCoordFromName(string oldParentName)
    {
        int dividerIndex = oldParentName.IndexOf(NAME_DIVIDER);
        return int.Parse(oldParentName.Substring(dividerIndex + 1));
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

    private GameObject GetChip(Coord coord)
    {
        return GetHolderChipFromPosition(coord.y, coord.x).transform.GetChild(0).gameObject;
    }

    private string GetHolderName(int row, int column) => $"{row} {NAME_DIVIDER} {column}";

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
}
