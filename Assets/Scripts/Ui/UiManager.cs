using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UiManager : MonoBehaviour
{
    [SerializeField]private GameObject m_indicInteraction;
    [SerializeField]private GameObject m_UILampe;
    [SerializeField]private GameObject m_UIDoudou;
    [SerializeField]private GameObject m_takeLight;

    [SerializeField]private GameObject m_uiFlashligt;

    [SerializeField] private GameObject m_pannelPause;

    private GameManager m_gameManager;

    private bool m_switchPannelPause;

    public bool m_isHandle = false;
    public bool m_isActivated;


    private void Awake()
    {
        m_switchPannelPause = false;
        m_pannelPause.SetActive(false);
        m_gameManager = FindObjectOfType<GameManager>();
    }
    private void Start()
    {
        m_indicInteraction.SetActive(false);
        m_UILampe.GetComponent<Image>().color = new Color32(100,100,100,255);
        m_UIDoudou.GetComponent<Image>().color = new Color32(100,100,100,255);

        DisableUi();
    }
    public void TakableObject()
    {
        m_indicInteraction.SetActive(true);
    }
    public void DisableUi()
    {
        m_indicInteraction.SetActive(false);
    }
    
    public void TakeLampe()
    {
        m_UILampe.GetComponent<Image>().color = new Color32(255,255,225,255);
    }
    
    public void DropLampe()
    {
        //Dï¿½sactiver la Ui qui prend l'objet
        //Activer l'Ui en bas ï¿½ droite
        //Debug.Log("prendre l'objet");

        m_UILampe.GetComponent<Image>().color = new Color32(100,100,100,255);
    }
    
    public void TakeDoudou()
    {
        //Dï¿½sactiver la Ui qui prend l'objet
        //Activer l'Ui en bas ï¿½ droite
        //Debug.Log("prendre l'objet");

        m_UIDoudou.GetComponent<Image>().color = new Color32(255,255,225,255);

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
    
    public void DropDoudou()
    {
        //Dï¿½sactiver la Ui qui prend l'objet
        //Activer l'Ui en bas ï¿½ droite
        //Debug.Log("prendre l'objet");

        m_UIDoudou.GetComponent<Image>().color = new Color32(100,100,100,255);
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
}
