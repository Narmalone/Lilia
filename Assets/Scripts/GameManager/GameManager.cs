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
    public bool gotKey = false;
    public bool isDead;
    public bool canPick = true;

    public bool isPlayerInGame = false;

    [SerializeField] private GameObject m_doudouUi;
    [SerializeField] private GameObject m_veilleuseUi;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        canPick = true;
    }
    public void PlayerNotIngame()
    {
        isPlayerInGame = false;
        if (isPlayerInGame == false)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
    public void PlayerInGame()
    {
        isPlayerInGame = true;
        if (isPlayerInGame == true)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
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
        isPaused = true;
        Time.timeScale = 0;
        PlayerNotIngame();
    }

    public void GameResume()
    {
        isPaused = false;
        Time.timeScale = 1;
        isPlayerInGame = true;
        PlayerInGame();
    }

}