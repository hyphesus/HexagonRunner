using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int score;

    public int Score { get => score; set => score = value; }

    public void UpdateScore()
    {
        Score++;
        Debug.Log($"Score: {Score}");
    }
}
