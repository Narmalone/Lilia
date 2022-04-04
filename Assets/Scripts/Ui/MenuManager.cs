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
    [SerializeField] private GameObject m_pannelMainMenu;

    //variables dans le Option depuis le menu
    [SerializeField] private GameObject m_newEventSystemFirstSelectedObject;
    [SerializeField] private GameObject m_newSystemCurrentSelectedObject;
    
    
    [SerializeField] private GameObject m_newEventSystemFirstSelectedObject_b;
    [SerializeField] private GameObject m_newSystemCurrentSelectedObject_b;
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

    public void DisableOptionPannel(GameObject p_obj)
    {
        p_obj.SetActive(false);
        EventSystem.current.firstSelectedGameObject = m_newEventSystemFirstSelectedObject_b;
        EventSystem.current.SetSelectedGameObject(m_newSystemCurrentSelectedObject_b);
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
        Debug.Log("Afficher la scene des cr�dits");
    }

    public void OnQuit()
    {
        Application.Quit();
    }

}