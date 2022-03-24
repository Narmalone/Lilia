using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    //Variables en rapport avec le jeu en général
    public bool isPc;
    public bool isGamepad;
    public bool isPaused;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    //----------------------------------------------- Choose Your Platform ------------------------------------------//

    public void PcSelected()
    {
        isPc = true;
        isGamepad = false;
        NextScene();
    }

    public void GamepadSelected()
    {
        isGamepad = true;
        isPc = false;
        NextScene();
    }

    //----------------------------------------------- Changements de scenes ------------------------------------------//

    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    //----------------------------------------------- Statut du jeu (en pause) ------------------------------------------//

    public void GamePaused()
    {
        Time.timeScale = 0;
    }

    public void GameResume()
    {
        Time.timeScale = 1;
    }

}