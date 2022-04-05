using System.Collections;
using System.Collections.Generic;
using CustomUtil;
using UnityEngine;

public class Match3Logic : MonoBehaviour
{

    BoardManager boardManager;

    private void Awake() {
        boardManager = FindObjectOfType<BoardManager>();
    }
    
    internal bool IsCoordPartOf3MatchLine(Coord coord)
    {
        return Get3MatchLine(coord.Get3MLineInAllDirecctions()) != null;
    }

    internal List<Coord> Get3MatchLine(List<List<Coord>> lineList)
    {
        foreach (List<Coord> line in lineList)
        {
            if (boardManager.IsLineInsideBoard(line) && IsA3Match(line)) 
            {
                return line;
            }
        }
        return null;
    }

    private bool IsA3Match(List<Coord> line)
    {
        for (int i = 0; i < line.Count - 1; i++)
        {
            int elem1 = boardManager.GetElementOnPosition(line[i]);
            int elem2 = boardManager.GetElementOnPosition(line[i + 1]);
            if (elem1 != elem2) return false;
        }
        return true;
    }
}
