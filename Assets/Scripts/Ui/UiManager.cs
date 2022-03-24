using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UiManager : MonoBehaviour
{
    //----------------------------------------------- R�f�rences & variables ------------------------------------------//

    [SerializeField]private GameObject m_takeLight;

    [SerializeField] private GameObject m_uiDoudou;

    [SerializeField] private GameObject m_pannelPause;

    [SerializeField]private GameObject m_indicInteraction;
    [SerializeField]private GameObject m_UILampe;
    [SerializeField]private GameObject m_UIDoudou;

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
        m_indicInteraction.SetActive(false);
        m_UILampe.GetComponent<Image>().color = new Color32(100,100,100,255);
        m_UIDoudou.GetComponent<Image>().color = new Color32(100,100,100,255);
    }
    private void Update()
    {
        
    }
    //----------------------------------------------- Ui prendre et dropper un item ------------------------------------------//


    //FLASHLIGHT//
    public void UiTakeFlashlight()
    {
        m_flashlightIsHandle = false;
        if (m_flashlightIsHandle == false)
        {
            m_takeLight.SetActive(true);
        }
        else
        {
            UiDisableFlashlight();
        }
    }

    public void TakableObject()
    {
        m_indicInteraction.SetActive(true);
    }


    public void DisableUi()
    {
        m_indicInteraction.SetActive(false);
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
            Debug.LogError("Probl�me DisableUi ?", this);
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
            Debug.LogError("Probl�me DisableUi ?", this);
        }

    }
    public void UiTakeDoudou()
    {
        m_doudouIsHandle = false;
        if (m_doudouIsHandle == false)
        {
            m_uiDoudou.SetActive(true);
        }
        else
        {
            UiDisableDoudou();
        }
    }
    //----------------------------------------------- Pannel d'UI ------------------------------------------//

    public void DisablePannel()
    {
        m_UILampe.GetComponent<Image>().color = new Color32(255,255,225,255);
    }
    
    public void DropLampe()
    {
        //D�sactiver la Ui qui prend l'objet
        //Activer l'Ui en bas � droite
        //Debug.Log("prendre l'objet");

        m_UILampe.GetComponent<Image>().color = new Color32(100,100,100,255);
    }
    
    public void TakeDoudou()
    {
        //D�sactiver la Ui qui prend l'objet
        //Activer l'Ui en bas � droite
        //Debug.Log("prendre l'objet");

        m_UIDoudou.GetComponent<Image>().color = new Color32(255,255,225,255);
    }
    
    public void DropDoudou()
    {
        //D�sactiver la Ui qui prend l'objet
        //Activer l'Ui en bas � droite
        //Debug.Log("prendre l'objet");

        m_UIDoudou.GetComponent<Image>().color = new Color32(100,100,100,255);
    }
}
