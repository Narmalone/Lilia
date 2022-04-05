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
    private MouseLock m_mouseLock;
    
    public float m_getSensibility;
    public float m_setSensibility;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if(m_txtsoundOption != null)
        {
            m_txtsoundOption = FindObjectOfType<TextSoundOption>();
        }
        else
        {
            return;
        }
        if(m_mouseLock != null)
        {
            m_mouseLock = FindObjectOfType<MouseLock>();
            SetSensibility();
        }
        else
        {
            return;
        }
        Cursor.lockState = CursorLockMode.Locked;

        GetMenuSensibility();
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

        m_getSensibility = TextSoundOption.instance.m_sensibility;
    }

    public void SetSensibility()
    {
        
    }
    
}