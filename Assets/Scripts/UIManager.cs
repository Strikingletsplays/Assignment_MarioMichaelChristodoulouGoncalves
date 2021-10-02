using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField]
    private TextMeshProUGUI Score;

    [SerializeField]
    private TextMeshProUGUI Streak;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateScore()
    {
        Streak.text = LevelGrid.instance.Multiplier.ToString();
        Score.text = Snake.instance.Score.ToString();                                           //Show Current Score
        if(Snake.instance.Score > PlayerPrefs.GetInt("HighScore", 0))                           //Save High Score to PlayerPrefs
        {
            PlayerPrefs.SetInt("HighScore", Snake.instance.Score);
        }
    }
}
