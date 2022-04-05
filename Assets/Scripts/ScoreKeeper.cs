using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    [SerializeField] int _3MatchScore = 100;

    int currentScore = 0;

    public int GetCurrentScore() => currentScore;

    public void ScoreLine() => AddToCurrentScore(_3MatchScore);
    public void AddToCurrentScore(int value)
    {
        currentScore += value;
        currentScore = Mathf.Clamp(currentScore, 0, int.MaxValue);
    }
    
    public void ResetCurrentScore() => currentScore = 0;

    public string GetStringCurrentScore() => currentScore.ToString("000000000");
}
