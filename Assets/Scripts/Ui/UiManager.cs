using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UiManager : MonoBehaviour
{
    //----------------------------------------------- Références & variables ------------------------------------------//

    [SerializeField]private GameObject m_takeLight;

    [SerializeField] private GameObject m_uiDoudou;

    [SerializeField] private GameObject m_pannelPause;

    private GameManager m_gameManager;

    private bool m_switchPannelPause;

    public bool m_flashlightIsHandle = false;
    public bool m_doudouIsHandle = false;
    public bool m_isActivated;


    private void Awake()
    {
        m_switchPannelPause = false;
        m_pannelPause.SetActive(false);
        m_gameManager = FindObjectOfType<GameManager>();
    }
    private void Start()
    {
        UiDisableFlashlight();
        UiDisableDoudou();
    }

    //----------------------------------------------- Ui prendre et dropper un item ------------------------------------------//


    //FLASHLIGHT//
    public void UiTakeFlashlight()
    {
        m_flashlightIsHandle = false;
        if(m_flashlightIsHandle == false)
        {
            m_takeLight.SetActive(true);
        }
        else
        {
            UiDisableFlashlight();
        }
    }
   
    public void UiDisableFlashlight()
    {
        m_flashlightIsHandle = true;
        if (m_flashlightIsHandle == true)
        {
            m_takeLight.SetActive(false);
        }
        else
        {
            Debug.LogError("Problème DisableUi ?", this);
        }
        
    }

    //DOUDOU//
    public void UiDisableDoudou()
    {
        m_doudouIsHandle = true;
        if (m_doudouIsHandle == true)
        {
            m_uiDoudou.SetActive(false);
        }
        else
        {
            Debug.LogError("Problème DisableUi ?", this);
        }

    }
    public void UiTakeDoudou()
    {
        m_doudouIsHandle = false;
        if (m_doudouIsHandle == false)
        {
            m_takeLight.SetActive(true);
        }
        else
        {
            UiDisableDoudou();
        }
    }
    //----------------------------------------------- Pannel d'UI ------------------------------------------//

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
}
