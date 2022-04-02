using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UiManager : MonoBehaviour
{
    //----------------------------------------------- R�f�rences & variables ------------------------------------------//

    //Ui In game
    [SerializeField]private GameObject m_indicInteraction;
    [SerializeField]private GameObject m_UILampe;
    [SerializeField]private GameObject m_UIDoudou;

    private GameManager m_gameManager;

    public bool m_flashlightIsHandle = false;
    public bool m_doudouIsHandle = false;
    public bool m_isActivated;


    private void Awake()
    {
        m_gameManager = FindObjectOfType<GameManager>();
    }
    private void Start()
    {
        m_indicInteraction.SetActive(false);
        m_UILampe.GetComponent<Image>().color = new Color32(100,100,100,255);
        m_UIDoudou.GetComponent<Image>().color = new Color32(100,100,100,255);
    }
    private void Update()
    {
        
    }
    //----------------------------------------------- Ui prendre et dropper un item ------------------------------------------//


    //FLASHLIGHT//


    public void TakableObject()
    {
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

        m_UILampe.GetComponent<Image>().color = new Color32(255,255,225,255);
    }
    
    public void DropLampe()
    {
        //D�sactiver la Ui qui prend l'objet
        //Activer l'Ui en bas � droite
        m_UILampe.GetComponent<Image>().color = new Color32(100,100,100,255);
    }
    
    public void TakeDoudou()
    {
        //D�sactiver la Ui qui prend l'objet
        //Activer l'Ui en bas � droite

        m_UIDoudou.GetComponent<Image>().color = new Color32(255,255,225,255);
    }
    
    public void DropDoudou()
    {
        //D�sactiver la Ui qui prend l'objet
        m_UIDoudou.GetComponent<Image>().color = new Color32(100,100,100,255);
    }
}
