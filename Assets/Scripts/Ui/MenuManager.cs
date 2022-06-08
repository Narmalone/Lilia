using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    //Variables en rapport avec le MainMenu
    [SerializeField] private GameObject m_pannelOption;
    [SerializeField] private GameObject m_pannelPause;
    [SerializeField] private GameObject m_pannelDeath;
    [SerializeField] private GameObject m_pannelGame;
    //variables dans le Option depuis le menu
    [SerializeField] private GameObject m_newEventSystemFirstSelectedObject;
    [SerializeField] private GameObject m_newSystemCurrentSelectedObject;
    
    
    [SerializeField] private GameObject m_newEventSystemFirstSelectedObject_b;
    [SerializeField] private GameObject m_newSystemCurrentSelectedObject_b;
    private float r = 255;

    private GameManager m_gameManager;



    private void Awake()
    {
        m_pannelOption.SetActive(false);
        m_gameManager = FindObjectOfType<GameManager>();
        if(m_pannelPause == null)
        {
            return;
        }
    }

    //----------------------------------------------- Changements de scenes ------------------------------------------//

    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    //----------------------------------------------- Main Menu ------------------------------------------//

    public void OnPlay()
    {
        NextScene();
    }
    public void OnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void OnResume(GameObject p_obj)
    {
        m_pannelGame.SetActive(true);
        p_obj.SetActive(false);
        m_gameManager.GameResume();
    }

    public void OnPause()
    {
        m_pannelGame.SetActive(false);
        m_pannelPause.SetActive(true);
    }

    public void OnOptions()
    {
        m_pannelOption.SetActive(true);
        EventSystem.current.firstSelectedGameObject = m_newEventSystemFirstSelectedObject;
        EventSystem.current.SetSelectedGameObject(m_newSystemCurrentSelectedObject);
    }

    public void OnDeath()
    {
        m_pannelDeath.SetActive(true);
    }public void OnRespawn()
    {
        m_pannelDeath.SetActive(false);
    }
    public void DisableOptionPannel(GameObject p_obj)
    {
        p_obj.SetActive(false);
        EventSystem.current.firstSelectedGameObject = m_newEventSystemFirstSelectedObject_b;
        EventSystem.current.SetSelectedGameObject(m_newSystemCurrentSelectedObject_b);
    }
    public void OnCredits()
    {
        m_gameManager.PlayerNotIngame();
        SceneManager.LoadScene("Credits");
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    public void ActivePannel(GameObject p_obj)
    {
        p_obj.SetActive(true);
    }

    public void DisablePannel(GameObject p_obj)
    {
        p_obj.SetActive(false);
    }

}