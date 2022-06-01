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
    [SerializeField,Tooltip("image d'UI pour l'intercation")]private GameObject m_indicInteraction;
    [SerializeField,Tooltip("image d'UI pour l'indication de lampe")]private GameObject m_UILampe;
    [SerializeField,Tooltip("image d'UI pour l'indication de doudou")]private GameObject m_UIDoudou;
    [SerializeField,Tooltip("Objet d'Ui du doudou quand il est en main")]private GameObject m_objDoudouUi;
    [SerializeField,Tooltip("Objet d'Ui de la veilleuse quand elle est en main")]private GameObject m_objVeilleuseUi;
    [SerializeField] private PlayerController m_player;

    private GameManager m_gameManager;

    [NonSerialized]
    public bool m_flashlightIsHandle = false;
    [NonSerialized]
    public bool m_doudouIsHandle = false;

    private bool isAnimated = false;
    private bool animActivated = false;
    [NonSerialized]
    public bool m_isActivated;
    private int indexAnim = 0;
    public int indexAnimMax = 2;

    [SerializeField] private Sprite m_leftClickSprite;
    [SerializeField] private Sprite m_rightClickSprite;
    [SerializeField] private Sprite m_EKey;
    [SerializeField] private Sprite m_EKeyNotInteract;

    [SerializeField] private Color m_dontInteract;
    [SerializeField] private Color m_possessed;
    private void Awake()
    {
        m_gameManager = FindObjectOfType<GameManager>();
        m_indicInteraction.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        m_indicInteraction.SetActive(false);
        m_objDoudouUi.SetActive(false);
        m_objVeilleuseUi.SetActive(false);
        m_UILampe.GetComponent<Image>().color = new Color32(100, 100, 100, 255);
        m_UIDoudou.GetComponent<Image>().color = new Color32(100, 100, 100, 255);
        isAnimated = false;
        animActivated = false;
        indexAnim = 0;
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

    private void LateUpdate()
    {
        if(animActivated == true)
        {
            AnimUi();
        }
    }
    //----------------------------------------------- UI Objets du joueur ------------------------------------------//

    //Normal si le joueur appuie sur E sa blink
    public void AnimUi()
    {
        if(m_player.isLeftHandFull == true)
        if (isAnimated == false)
        {
            m_UIDoudou.GetComponent<Animator>().SetTrigger("FadeOut");
            isAnimated = true;
        }
        if (isAnimated == true)
        {
            if (indexAnim <= indexAnimMax)
            {
                m_UIDoudou.GetComponent<Animator>().SetTrigger("FadeIn");
                isAnimated = false;
                indexAnim++;
            }
            else
            {
                animActivated = false;
            }
        }
    }
    public void DropSomethingBefore()
    {
        if(m_player.isLeftHandFull == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                indexAnim = 0;
                animActivated = true;
            }
            
            Debug.Log("doudou couleur rouge");
            m_indicInteraction.GetComponent<Image>().sprite = m_EKeyNotInteract;
            m_indicInteraction.SetActive(true);
        }
        if (m_player.isRightHandFull == true)
        {
            m_indicInteraction.GetComponent<Image>().sprite = m_EKeyNotInteract;
            m_indicInteraction.SetActive(true);
        }
        if(m_player.isTwoHandFull == true)
        {
            m_indicInteraction.GetComponent<Image>().sprite = m_EKeyNotInteract;
            m_indicInteraction.SetActive(true);
        }
        Debug.Log("dans le drop something");
    }
    public void StopRaycastBefore()
    {
        animActivated = false;
        if (m_player.isLeftHandFull == true)
        {
            m_indicInteraction.SetActive(false);
            m_UIDoudou.GetComponent<Animator>().SetTrigger("Reset");
        }
        if (m_player.isRightHandFull == true)
        {
            m_indicInteraction.SetActive(false);
            m_UILampe.GetComponent<Image>().color = m_possessed;
            m_UILampe.GetComponent<Animator>().SetTrigger("Reset");
        }
        if (m_player.isTwoHandFull == true)
        {
            m_indicInteraction.SetActive(false);
            m_UIDoudou.GetComponent<Animator>().SetTrigger("Reset");
            m_UILampe.GetComponent<Animator>().SetTrigger("Reset");
        }
        Debug.Log("stop raycast before");
    }
    //Affichage en bas à droite
    public void TakeLampe()
    {
        //D�sactiver la Ui qui prend l'objet
        //Activer l'Ui en bas � droite

        m_UILampe.GetComponent<Image>().color = Color.red;
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
