using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField]
    private TextMeshProUGUI Score;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateScore()
    {
        Score.text = Snake.instance.Score.ToString();
    }
}
