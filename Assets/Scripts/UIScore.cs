using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIScore : MonoBehaviour
{
    TextMeshProUGUI scoreText;

    ScoreKeeper scoreKeeper;

    private void Awake()
    {
        scoreText = gameObject.GetComponent<TextMeshProUGUI>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
    }

    private void Update()
    {
        if (scoreKeeper == null) return;
        SetScoreText(scoreKeeper.GetStringCurrentScore());
    }

    private void SetScoreText(string value)
    {
        scoreText.text = value;
    }

}
