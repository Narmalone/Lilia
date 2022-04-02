using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PannelManager : MonoBehaviour
{
    //Ui Pannels
    [SerializeField] private GameObject m_pannelPause;
    [SerializeField] private GameObject m_pannelOption;

    private GameManager m_gameManager;

    private void Awake()
    {
        m_pannelPause.SetActive(false);
        m_pannelOption.SetActive(false);
        m_gameManager = FindObjectOfType<GameManager>();
    }

    //----------------------------------------------- Pannel d'UI ------------------------------------------//

    //Pannel pause//
    public void OnPannelPause()
    {
        m_pannelPause.SetActive(true);
    }
    public void OnResume()
    {
        m_pannelPause.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        m_gameManager.GameResume();
    }

    public void OnOption()
    {
        m_pannelOption.SetActive(true);
    }

    public void OnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void OnQuit()
    {
        Application.Quit();
    }
}
