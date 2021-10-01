using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    //Button's functionality

    public void StartGameButton()
    {
        SceneManager.LoadScene("Game Scene", LoadSceneMode.Single);
    }

    public void BackToMenuButton()
    {
        SceneManager.LoadScene("Main Scene", LoadSceneMode.Single);
    }

    public void ShowTopScoreButton()
    {

    }
}
