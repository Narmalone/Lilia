using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UiManager : MonoBehaviour
{
    [SerializeField]private GameObject m_takeLight;

    [SerializeField]private GameObject m_uiFlashligt;

    [SerializeField] private GameObject m_pannelPause;

    private GameManager m_gameManager;

    private bool m_switchPannelPause;

    private bool m_switchGamepadPause;

    public bool m_isHandle = false;
    public bool m_isActivated;


    private void Awake()
    {
        m_switchPannelPause = false;
        m_pannelPause.SetActive(false);
        m_gameManager = FindObjectOfType<GameManager>();
        m_switchGamepadPause = false;

    }
    private void Start()
    {
        DisableUi();
    }
    public void takeObject()
    {
        m_isHandle = false;
        if(m_isHandle == false)
        {
            m_takeLight.SetActive(true);
        }
        else
        {
            DisableUi();
        }
    }
    public void DisableUi()
    {
        m_isHandle = true;
        if (m_isHandle == true)
        {
            m_takeLight.SetActive(false);
        }
        else
        {
            Debug.LogError("Problème DisableUi ?", this);
        }
        
    }
    public void DisablePannel()
    {
        m_pannelPause.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        m_switchPannelPause = false;
    }
    public void MenuPause()
    {
        if(m_switchPannelPause == false)
        {
            m_pannelPause.SetActive(true);
            m_switchPannelPause = true;
            m_gameManager.GamePaused();
            Cursor.lockState = CursorLockMode.None;
        }
        else if(m_switchPannelPause == true)
        {
            m_switchPannelPause = false;
            m_gameManager.GameResume();
            Cursor.lockState = CursorLockMode.Locked;
            m_pannelPause.SetActive(false);
        }
    }

    public void GamepadUiPauseSwitch()
    {
        //Faire un swith pour pas que quand on maintient start ça le fasse en boucle
        if(m_switchGamepadPause == false)
        {
            m_pannelPause.SetActive(true);
            m_switchGamepadPause = true;
            Debug.Log("doit s'activer");
        }
        else if (m_switchGamepadPause == true)
        {
            m_pannelPause.SetActive(false);
            m_switchGamepadPause = false;
            Debug.Log("doit s'enlever");

        }
    }
}
