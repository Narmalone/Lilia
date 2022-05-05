using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    //Variables en rapport avec le jeu en g�n�ral
    public bool isPc;
    public bool isGamepad;
    public bool isPaused;

    public bool isDead;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        Cursor.lockState = CursorLockMode.Locked;
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
    public void OnBack()
    {
        SceneManager.LoadScene("MainMenu");
    }


    //----------------------------------------------- Statut du jeu (en pause) ------------------------------------------//

    public void GamePaused()
    {
        Time.timeScale = 0;
        Debug.Log("jeu en pause");
    }

    public void GameResume()
    {
        Time.timeScale = 1;
    }

}