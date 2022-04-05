using System.Collections;
using System.Collections.Generic;
using CustomUtil;
using UnityEngine;

public class MatchSeeker : MonoBehaviour
{
    GameScene gameScene;
    BoardManager boardManager;

    bool shouldCheckMatches = false;

    private void Awake()
    {
        gameScene = FindObjectOfType<GameScene>();
        boardManager = FindObjectOfType<BoardManager>();
    }

    void Start()
    {
        shouldCheckMatches = true;
        StartCoroutine(Check3Matches(boardManager.GetAllCoords()));
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public IEnumerator Check3Matches(List<Coord> coords)
    {
        while (true)
        {
            yield return new WaitUntil(() => gameScene.HasFinishDrawingBoard());
            Debug.Log($"Check3MatchesAllPositions");
            foreach (Coord coord in coords)
            {
                Debug.Log($"Check3MatchesAllPositions - Checking coord = {coord.ToString()}");
                List<Coord> line = Get3MatchLine(coord.Get3MLineInAllDirecctions());
                          
                if (line != null)
                {
                    shouldCheckMatches = true;
                    boardManager.Proccess3Match(line);
                    yield return new WaitUntil(() => gameScene.HasFinishDrawingBoard());
                }
            }
            if (!shouldCheckMatches) DisableScript();
            shouldCheckMatches = false;
        }
    }

    private List<Coord> Get3MatchLine(List<List<Coord>> lineList)
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

    public void DisableScript()
    {
        enabled = false;
    }

}
