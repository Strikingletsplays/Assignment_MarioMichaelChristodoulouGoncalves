using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndScore : MonoBehaviour
{
    public static EndScore instance;

    [SerializeField]
    private TextMeshProUGUI EndScoreValue;
    [SerializeField]
    private GameObject EndCanvas;

    private void Awake()
    {
        instance = this;
    }
    public void ShowEndScore()
    {
        EndScoreValue.text = Snake.instance.Score.ToString();
    }
    public void EnableEndCanvas()
    {
        EndCanvas.SetActive(true);
    }
    public void GoBackToMenu()
    {
        EndCanvas.SetActive(false);
        SceneManager.LoadScene("Main Scene", LoadSceneMode.Single);
    }
}
