using System.Collections;
using System.Collections.Generic;
using CustomUtil;
using UnityEngine;

public class MatchSeeker : MonoBehaviour
{
    GameScene gameScene;
    BoardManager boardManager;
    Match3Logic match3Logic;

    bool shouldCheckMatches = false;

    private void Awake()
    {
        gameScene = FindObjectOfType<GameScene>();
        boardManager = FindObjectOfType<BoardManager>();
        match3Logic = GetComponent<Match3Logic>();
    }

    void Start()
    {
        Init();
    }

    public void Init()
    {
        enabled = true;
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
                List<Coord> line = match3Logic.Get3MatchLine(coord.Get3MLineInAllDirecctions());
                          
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

    public void DisableScript()
    {
        enabled = false;
    }

}
