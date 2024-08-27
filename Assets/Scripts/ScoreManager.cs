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
        Score++;
        Debug.Log($"Score: {Score}");
        scoreT.text = Score.ToString();
    }
    public void ResetScore()
    {
        Score = 0;
        scoreT.text = Score.ToString();
    }
}
