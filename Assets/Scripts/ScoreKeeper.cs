using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    int currentScore = 0;

    public int GetCurrentScore() => currentScore;
    public void AddToCurrentScore(int value)
    {
        currentScore += value;
        currentScore = Mathf.Clamp(currentScore, 0, int.MaxValue);
    }
    
    public void ResetCurrentScore() => currentScore = 0;

    public string GetStringCurrentScore() => currentScore.ToString("000000000");
}
