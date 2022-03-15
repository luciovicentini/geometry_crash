using System;
using System.Collections;
using System.Collections.Generic;
using CustomUtil;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] bool logBoard;
    [SerializeField] bool shouldCheckMatches = false;
    [SerializeField] bool shouldDrawBoard = false;

    int[,] board;

    bool boardHadAMatch = false;

    int chipsQty = 0;

    GameScene gameScene;

    void Awake()
    {
        gameScene = FindObjectOfType<GameScene>();
    }

    public void SetUpBoard(int rows, int columns)
    {
        board = new int[rows, columns];
    }

    public void SetUpChips(int chipsQty)
    {
        this.chipsQty = chipsQty;
        PopulateBoard();
    }

    private void PopulateBoard()
    {
        for (int i = 0; i < GetBoardYLength(); i++)
        {
            for (int j = 0; j < GetBoardXLength(); j++)
            {
                int randomChipIndex = GetRandomChipIndex();
                board[i, j] = randomChipIndex;
            }
        }
        shouldDrawBoard = false;
    }

    public int GetRandomChipIndex()
    {
        return UnityEngine.Random.Range(0, chipsQty);
    }

    internal void Check3Matches()
    {
        
    }

    void Update()
    {
        if (shouldDrawBoard)
        {
            DrawBoard();
        }

        if (logBoard)
        {
            LogBoard();
        }

        if (shouldCheckMatches)
        {
            CheckMatchesAllBoard();
            if (!boardHadAMatch)
            {
                Debug.Log("This board has not more matches");
            }
        }
    }

    private void DrawBoard()
    {
        for (int i = 0; i < GetBoardYLength(); i++)
        {
            for (int j = 0; j < GetBoardXLength(); j++)
            {
                Coord position = new Coord(i, j);
                gameScene.SetUpInsideChip(GetElementOnPosition(position), position.y, position.x);
            }
        }
        shouldDrawBoard = false;
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
        boardHadAMatch = false;
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
            boardHadAMatch = true;
            LogListCoords(line);
            ProcessHorLine(line);
            RefillHor(line);
        }
    }

    private void CheckingVertical3Match(Coord coord)
    {
        List<Coord> line = coord.CreateLine(3, LineType.Vertical);

        boardHadAMatch = true;
        LogListCoords(line);
        ProcessVertLine(line);
        RefillVert(line);
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
        return IsInsideY(coord) && IsInsideX(coord);
    }

    private bool IsInsideX(Coord coord)
    {
        return GetBoardXLength() > coord.x && coord.x >= 0;
    }

    private bool IsInsideY(Coord coord)
    {
        return GetBoardYLength() > coord.y && coord.y >= 0;
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
        BoobleUpHorizontalLine(line);
    }

    private void BoobleUpHorizontalLine(List<Coord> line)
    {
        foreach (Coord coord in line)
        {
            BoobleCoordUpToTop(coord);
        }
    }

    private void ProcessVertLine(List<Coord> line)
    {
        RemoveChips(line);
        BoobleUpVerticalLine(line);
    }

    private void BoobleUpVerticalLine(List<Coord> line)
    {
        Coord originalCoord = line[0];
        for (int i = 0; i < line.Count; i++)
        {
            BoobleCoordUpToTop(originalCoord);
        }
    }

    private void BoobleCoordUpToTop(Coord coord)
    {
        int amountRows = GetAmountOfRowToTop(coord);
        for (int i = 0; i < amountRows; i++)
        {
            Coord originCoord = coord.AddY(i);
            Coord nextCoordUp = originCoord.AddY(1);
            SwitchChips(originCoord, nextCoordUp);
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
        SetElementOnPosition(GetRandomChipIndex(), coord.AddY(delta));
    }

    private void RefillVert(List<Coord> line)
    {
        int lastRow = GetBoardYLength() - 1;

        for (int i = 0; i < line.Count; i++)
        {
            Coord coord = new Coord(lastRow - i, line[0].x);
            SetElementOnPosition(GetRandomChipIndex(), coord);
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
        shouldDrawBoard = true;
    }

    internal bool CheckCoordMade3Match(Coord coord)
    {
        bool is3MatchMade = false;
        List<List<Coord>> lineList = coord.Get3MLineInAllDirecctions();
        foreach (List<Coord> line in lineList)
        {
            if (!IsLineInsideBoard(line)) continue;
            if (!IsA3Match(line)) continue;
            is3MatchMade = true;
        }
        return is3MatchMade;
    }

}
