using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private int score;
    //scoru ekrana yazma
    [SerializeField] private Text scoreT;
    public int Score { get => score; set => score = value; }

    public void UpdateScore()
    {
        score++;
        Debug.Log($"Score: {Score}");
        scoreT.text = Score.ToString();

        if (score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
    }
    public void ResetScore()
    {
        Score = 0;
        scoreT.text = Score.ToString();
        Debug.Log($"HighScore: {PlayerPrefs.GetInt("HighScore")}");
    }
}
