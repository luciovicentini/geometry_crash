using System.Collections;
using System.Collections.Generic;
using CustomUtil;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    int[,] board;
    [SerializeField] bool logBoard;
    [SerializeField] bool shouldCheckMatches;

    GameScene gameScene;

    void Awake()
    {
        gameScene = FindObjectOfType<GameScene>();
    }

    public void SetUpBoard(int rows, int columns)
    {
        // Debug.Log($"Setting up an array[{rows}, {columns}]");
        board = new int[rows, columns];

    }

    void Update()
    {
        if (logBoard)
        {
            LogBoard();
        }

        if (shouldCheckMatches)
        {
            CheckMatchesAllBoard();
        }
    }

    public void SetElementOnPosition(int element, Coord coord)
    {
        board[coord.GetY(), coord.GetX()] = element;
        Debug.Log($"Setting chip in {coord.ToString()} = {gameScene.ChipValueToString(element)}");
    }

    public int GetElementOnPosition(Coord coord)
    {
        return board[coord.GetY(), coord.GetX()];
    }

    private void CheckMatchesAllBoard()
    {

        for (int checkingPositionY = 0; checkingPositionY < GetBoardYLength(); checkingPositionY++)
        {
            for (int checkingPositionX = 0; checkingPositionX < board.GetLength(1); checkingPositionX++)
            {
                CheckingPosition(new Coord(checkingPositionY, checkingPositionX));
            }
        }
        shouldCheckMatches = false;
    }

    private void CheckingPosition(Coord coord)
    {
        if (IsHorizontallyInsideBoard(coord.GetX(), 3))
        {
            if (AreNextHorThreeTheSameChip(coord))
            {
                LogNextThreeHorPositions(coord);
            }
        }

        if (IsVertInsideBoard(coord.GetY(), 3))
        {
            if (AreNextVertThreeTheSameChip(coord))
            {
                LogNextThreeVertPositions(coord);
            }
        }
    }

    private bool AreNextHorThreeTheSameChip(Coord coord)
    {
        return (GetElementOnPosition(coord) == GetElementOnPosition(coord.AddX(1)))
            && (GetElementOnPosition(coord.AddX(1)) == GetElementOnPosition(coord.AddX(2)));
    }


    private bool AreNextVertThreeTheSameChip(Coord coord)
    {
        return (GetElementOnPosition(coord) == GetElementOnPosition(coord.AddY(1)))
            && (GetElementOnPosition(coord.AddY(1)) == GetElementOnPosition(coord.AddY(2)));
    }

    private bool IsHorizontallyInsideBoard(int posX, int length)
    {
        return GetBoardXLength() - 1 > posX + length - 1;
    }

    private bool IsVertInsideBoard(int posY, int length)
    {
        return GetBoardYLength() - 1 > posY + length - 1;
    }

    private int GetBoardYLength()
    {
        return board.GetLength(0);
    }

    private int GetBoardXLength()
    {
        return board.GetLength(1);
    }

    private void LogBoard()
    {

        for (int i = 0; i < GetBoardYLength(); i++)
        {
            string line = $"Row({i}) [";
            for (int j = 0; j < GetBoardXLength(); j++)
            {
                line += $"{gameScene.ChipValueToString(GetElementOnPosition(new Coord(i, j)))}";
                if (j + 1 < GetBoardXLength())
                {
                    line += ", ";
                }
            }
            line += "]";
            Debug.Log(line);
        }
        logBoard = false;
    }

    private void LogNextThreeHorPositions(Coord coord)
    {
        Debug.Log($"Chips [{coord.ToString()}],[{coord.AddX(1).ToString()}],[{coord.AddX(2).ToString()}] are equals");
    }

    private void LogNextThreeVertPositions(Coord coord)
    {
        Debug.Log($"Chips [{coord.ToString()}],[{coord.AddY(1).ToString()}],[{coord.AddY(2).ToString()}] are equals");
    }

    public void SwitchChips(Coord coordChip1, Coord coordChip2)
    {
        int chip1Element = GetElementOnPosition(coordChip1);
        int chip2Element = GetElementOnPosition(coordChip2);
        Debug.Log($"SwitchChips ({chip1Element}) {gameScene.ChipValueToString(chip1Element)} - {coordChip1}");
        Debug.Log($"SwitchChips ({chip2Element}) {gameScene.ChipValueToString(chip2Element)} - {coordChip2}");

        SetElementOnPosition(chip1Element, coordChip2);
        SetElementOnPosition(chip2Element, coordChip1);
    }
}
