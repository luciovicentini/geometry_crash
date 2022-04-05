using System;
using System.Collections;
using System.Collections.Generic;
using CustomUtil;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] bool logBoard;

    int[,] board;

    int chipsQty = 0;

    GameScene gameScene;
    ScoreKeeper scoreKeeper;

    internal List<Coord> GetAllCoords()
    {
        List<Coord> coords = new List<Coord>();
        for (int posY = 0; posY < GetBoardYLength(); posY++)
        {
            for (int posX = 0; posX < GetBoardXLength(); posX++)
            {
                coords.Add(new Coord(posY, posX));
            }
        }
        return coords;
    }

    void Awake()
    {
        gameScene = FindObjectOfType<GameScene>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
    }

    public void SetUpBoard(int rows, int columns)
    {
        board = new int[rows, columns];
    }

    public void RandomizeBoard()
    {
        SetUpBoard(gameScene.GetChipPerSizeAmount(), gameScene.GetChipPerSizeAmount());
        chipsQty = gameScene.GetAmountOfChipElements();

        for (int i = 0; i < GetBoardYLength(); i++)
        {
            for (int j = 0; j < GetBoardXLength(); j++)
            {
                int randomChipIndex = GetRandomChipIndex();
                board[i, j] = randomChipIndex;
            }
        }
    }

    public int GetRandomChipIndex() => UnityEngine.Random.Range(0, chipsQty);

    void Update()
    {
        if (logBoard)
        {
            LogBoard();
        }
    }

    public void SetElementOnPosition(int element, Coord coord) => board[coord.y, coord.x] = element;

    public int GetElementOnPosition(Coord coord) => board[coord.y, coord.x];

    public void Proccess3Match(List<Coord> line)
    {
        scoreKeeper.ScoreLine();
        LogListCoords(line);
        if (Coord.GetLineType(line) == LineType.Horizontal)
        {
            ProcessHorLine(line);
            RefillHor(line);
        }
        else
        {
            ProcessVertLine(line);
            RefillVert(line);
        }
        gameScene.process3MLine(line);
    }

    public bool IsLineInsideBoard(List<Coord> line)
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

    public int GetBoardYLength()
    {
        return board.GetLength(0);
    }

    public int GetBoardXLength()
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
    }
}
