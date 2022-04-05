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

    private TextSoundOption m_txtsoundOption;
    
    public float m_getSensibility;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        m_txtsoundOption = FindObjectOfType<TextSoundOption>();
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
    }

    public void GameResume()
    {
        Time.timeScale = 1;
    }

    public void GetMenuSensibility()
    {
        m_getSensibility = m_txtsoundOption.m_sensibility;
        //Debug.Log((m_getSensibility));
    }
    
}