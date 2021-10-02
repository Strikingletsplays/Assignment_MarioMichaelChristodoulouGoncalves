using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private GameObject HighScore;
    [SerializeField]
    private TextMeshProUGUI HighScoreText;
    [SerializeField]
    private TextMeshProUGUI EndScore;
    
    private void Start()
    {
        HighScoreText.text = PlayerPrefs.GetInt("HighScore").ToString();            //Get HighScore from PlayerPrefs 
    }
                                                                                    //Button's functionality
    public void StartGameButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game Scene", LoadSceneMode.Single);
    }

    public void BackToMenuButton()
    {
        SceneManager.LoadScene("Main Scene", LoadSceneMode.Single);
    }

    public void ShowTopScoreButton()
    {
        HighScore.SetActive(true);
    }

    public void HideTopScoreButton()
    {
        HighScore.SetActive(false);
    }
}
