using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] XXXMenuManager menuManager;
    private int score;
    //scoru ekrana yazma
    [SerializeField] private Text scoreT;
    public int Score { get => score; set => score = value; }

    private void Start()
    {
        if (PlayerPrefs.HasKey("HighScore"))
        {
            menuManager.athScoreTxt.text = $"ATH: {PlayerPrefs.GetInt("HighScore")}";
        }
        else
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
    }

    public void UpdateScore()
    {
        score++;
        Debug.Log($"Score: {Score}");
        scoreT.text = Score.ToString();

        if (score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", score);
            menuManager.athScoreTxt.text = $"ATH: {PlayerPrefs.GetInt("HighScore")}";
        }
    }
    public void ResetScore()
    {
        Score = 0;
        scoreT.text = Score.ToString();
        Debug.Log($"HighScore: {PlayerPrefs.GetInt("HighScore")}");
    }
}
