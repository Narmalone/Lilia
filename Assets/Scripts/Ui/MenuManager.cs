using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    //Variables en rapport avec le MainMenu
    [SerializeField] private GameObject m_pannelOption;
    [SerializeField] private GameObject m_pannelMainMenu;

    //variables dans le Option depuis le menu
    [SerializeField] private GameObject m_newEventSystemFirstSelectedObject;
    [SerializeField] private GameObject m_newSystemCurrentSelectedObject;
    private float r = 255;

    private void Awake()
    {
        m_pannelOption.SetActive(false);
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

    public void OnLoad()
    {
        Debug.Log("Recharger le niveau depuis le dernier checkpoint");
    }

    public void OnOptions()
    {
        m_pannelOption.SetActive(true);
        EventSystem.current.firstSelectedGameObject = m_newEventSystemFirstSelectedObject;
        EventSystem.current.SetSelectedGameObject(m_newSystemCurrentSelectedObject);
    }

    public void OnCredits()
    {
        Debug.Log("Afficher la scene des crédits");
    }

    public void OnQuit()
    {
        Application.Quit();
    }

}