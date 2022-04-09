using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    //Variables en rapport avec le MainMenu
    [SerializeField] private GameObject m_pannelOption;
    [SerializeField] private GameObject m_pannelMainMenu;

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