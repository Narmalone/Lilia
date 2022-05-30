using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UiManager : MonoBehaviour
{
    //----------------------------------------------- R�f�rences & variables ------------------------------------------//

    //Ui In game
    public GameObject m_playingGameObject;
    
    [SerializeField,Tooltip("image d'UI pour l'intercation")]private GameObject m_indicInteraction;
    [SerializeField,Tooltip("image d'UI pour l'indication de lampe")]private GameObject m_UILampe;
    [SerializeField,Tooltip("image d'UI pour l'indication de doudou")]private GameObject m_UIDoudou;
    [SerializeField,Tooltip("Objet d'Ui du doudou quand il est en main")]private GameObject m_objDoudouUi;
    [SerializeField,Tooltip("Objet d'Ui de la veilleuse quand elle est en main")]private GameObject m_objVeilleuseUi;

    private GameManager m_gameManager;

    [NonSerialized]
    public bool m_flashlightIsHandle = false;
    [NonSerialized]
    public bool m_doudouIsHandle = false;
    [NonSerialized]
    public bool m_isActivated;

    [SerializeField] private Sprite m_leftClickSprite;
    [SerializeField] private Sprite m_rightClickSprite;
    [SerializeField] private Sprite m_EKey;

    private void Awake()
    {
        m_gameManager = FindObjectOfType<GameManager>();
        m_indicInteraction.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }
    private void Start()
    {
        m_indicInteraction.SetActive(false);
        m_objDoudouUi.SetActive(false);
        m_objVeilleuseUi.SetActive(false);
        m_UILampe.GetComponent<Image>().color = new Color32(100,100,100,255);
        m_UIDoudou.GetComponent<Image>().color = new Color32(100,100,100,255);
    }
    private void Update()
    {
        
    }
    //----------------------------------------------- Ui prendre et dropper un item ------------------------------------------//


    //Interactions quand raycast//

    public void TakableFlashlight()
    {
        m_indicInteraction.GetComponent<Image>().sprite = m_rightClickSprite;
        m_indicInteraction.SetActive(true);
    }
     public void TakableDoudou()
    {
        m_indicInteraction.GetComponent<Image>().sprite = m_leftClickSprite;
        m_indicInteraction.SetActive(true);
    }

    
    public void TakableObject()
    {
        m_indicInteraction.GetComponent<Image>().sprite = m_EKey;
        m_indicInteraction.SetActive(true);
    }


    public void DisableUi()
    {
        m_indicInteraction.SetActive(false);
    }

    public bool InteractEnabled()
    {
        return m_indicInteraction.activeSelf;
    }

    
    public void DisablePannel()
    {
        m_UILampe.GetComponent<Image>().color = new Color32(255,255,225,255);
    }


    //----------------------------------------------- UI Objets du joueur ------------------------------------------//

    public void TakeLampe()
    {
        //D�sactiver la Ui qui prend l'objet
        //Activer l'Ui en bas � droite

        m_UILampe.GetComponent<Image>().color = Color.white;
        m_objVeilleuseUi.SetActive(true);
    }
    
    public void DropLampe()
    {
        //D�sactiver la Ui qui prend l'objet
        //Activer l'Ui en bas � droite
        m_UILampe.GetComponent<Image>().color = new Color32(100,100,100,255);
        m_objVeilleuseUi.SetActive(false);
    }

    public void TakeDoudou()
    {
        //D�sactiver la Ui qui prend l'objet
        //Activer l'Ui en bas � droite

        m_UIDoudou.GetComponent<Image>().color = new Color32(255,255,225,255);
        m_objDoudouUi.SetActive(true);
    }
    
    public void DropDoudou()
    {
        //D�sactiver la Ui qui prend l'objet
        m_UIDoudou.GetComponent<Image>().color = new Color32(100,100,100,255);
        m_objDoudouUi.SetActive(false);
    }
}
