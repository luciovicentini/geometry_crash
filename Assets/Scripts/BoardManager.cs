using System.Collections;
using System.Collections.Generic;
using CustomUtil;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    int[,] board;
    [SerializeField] bool logBoard;
    [SerializeField] bool shouldCheckMatches = false;
    [SerializeField] bool shouldDestroyMatches = false;

    GameScene gameScene;

    void Awake()
    {
        gameScene = FindObjectOfType<GameScene>();
    }

    public void SetUpBoard(int rows, int columns)
    {
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
        board[coord.y, coord.x] = element;
        // Debug.Log($"Setting chip in {coord.ToString()} = {gameScene.ChipValueToString(element)}");
    }

    public int GetElementOnPosition(Coord coord)
    {
        return board[coord.y, coord.x];
    }

    private void CheckMatchesAllBoard()
    {

        for (int checkingPositionY = 0; checkingPositionY < GetBoardYLength(); checkingPositionY++)
        {
            for (int checkingPositionX = 0; checkingPositionX < board.GetLength(1); checkingPositionX++)
            {
                Checking3Match(new Coord(checkingPositionY, checkingPositionX));
            }
        }
        shouldCheckMatches = false;
    }

    private void Checking3Match(Coord coord)
    {
        CheckingHorizontal3Match(coord);
        CheckingVertical3Match(coord);
    }

    private void CheckingHorizontal3Match(Coord coord)
    {
        List<Coord> line = coord.CreateLine(3, LineType.Horizontal);

        if (IsLineInsideBoard(line) && IsA3Match(line))
        {
            if (shouldDestroyMatches)
            {
                LogListCoords(line);
                ProcessHorLine(line);
                RefillHor(line);
            }
        }
    }

    private void CheckingVertical3Match(Coord coord)
    {
        List<Coord> line = coord.CreateLine(3, LineType.Vertical);

        if (IsLineInsideBoard(line) && IsA3Match(line))
        {
            if (shouldDestroyMatches)
            {
                LogListCoords(line);
                ProcessVertLine(line);
                RefillVert(line);
            }
        }
    }

    private bool IsA3Match(List<Coord> line)
    {
        for (int i = 0; i < line.Count - 1; i++)
        {
            int elem1 = GetElementOnPosition(line[i]);
            int elem2 = GetElementOnPosition(line[i + 1]);
            if (elem1 != elem2) return false;
        }
        return true;
    }

    private bool IsLineInsideBoard(List<Coord> line)
    {
        bool isLineInside = true;
        foreach (Coord coord in line)
        {
            if (!IsCoordInsideBoard(coord))
            {
                isLineInside = false;
                break;
            }
        }
        return isLineInside;
    }

    private bool IsCoordInsideBoard(Coord coord)
    {
        return GetBoardYLength() > coord.y && GetBoardXLength() > coord.x;
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
        string board = "";
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
            board += $"{line}\n";
        }
        Debug.Log(board);
        logBoard = false;
    }

    private void ProcessHorLine(List<Coord> line)
    {
        RemoveChips(line);
        SwitchHorLineChips(line);
    }

    private void SwitchHorLineChips(List<Coord> line)
    {
        foreach (Coord coord in line)
        {
            TakeDownColumnByOne(coord);
        }
    }

    private void ProcessVertLine(List<Coord> line)
    {
        RemoveChips(line);
        VertLineSwitchChips(line);
    }

    private void VertLineSwitchChips(List<Coord> line)
    {
        Coord originalCoord = line[0];
        for (int i = 0; i < line.Count; i++)
        {
            TakeDownColumnByOne(originalCoord);
        }
    }

    private void TakeDownColumnByOne(Coord coord)
    {
        int amount = GetAmountOfRowToTop(coord);
        for (int i = 0; i < amount; i++)
        {
            Coord coord1 = coord.AddY(i);
            Coord coord2 = coord1.AddY(1);
            SwitchChips(coord1, coord2);
        }
    }

    private void RefillHor(List<Coord> line)
    {
        foreach (Coord coord in line)
        {
            RefillOnTop(coord);
        }
    }

    private void RefillOnTop(Coord coord)
    {
        int delta = GetAmountOfRowToTop(coord);
        SetElementOnPosition(gameScene.GetRandomChipIndex(), coord.AddY(delta));
    }

    private void RefillVert(List<Coord> line)
    {
        int lastRow = GetBoardYLength() - 1;

        for (int i = 0; i < line.Count; i++)
        {
            Coord coord = new Coord(lastRow - i, line[0].x);
            SetElementOnPosition(gameScene.GetRandomChipIndex(), coord);
        }
    }

    private int GetAmountOfRowToTop(Coord coord) => GetBoardYLength() - 1 - coord.y;

    private void LogListCoords(List<Coord> coords)
    {
        Debug.Log($"Chips [{Coord.ListDebugging(coords)}] are equals");
    }

    private void RemoveChips(List<Coord> coords)
    {
        foreach (Coord coord in coords)
        {
            RemoveChip(coord);
        }
        shouldDestroyMatches = false;
    }

    private void RemoveChip(Coord coord)
    {
        SetElementOnPosition(-1, coord);
    }

    public void SwitchChips(Coord coordChip1, Coord coordChip2)
    {
        int chip1Element = GetElementOnPosition(coordChip1);
        int chip2Element = GetElementOnPosition(coordChip2);

        SetElementOnPosition(chip1Element, coordChip2);
        SetElementOnPosition(chip2Element, coordChip1);
    }
}
