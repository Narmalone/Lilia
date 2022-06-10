using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class UiManager : MonoBehaviour
{
    //----------------------------------------------- R�f�rences & variables ------------------------------------------//

    //Ui In game
    [SerializeField,Tooltip("image d'UI pour l'intercation")]private GameObject m_indicInteraction;
    [SerializeField,Tooltip("image d'UI pour l'indication de lampe")]private GameObject m_UILampe;
    [SerializeField,Tooltip("image d'UI pour l'indication de doudou")]private GameObject m_UIDoudou;
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
    private int m_alreadyShrinkingDoudou = 0;
    private int m_alreadyShrinkingLampe = 0;
    private int m_alreadyFlashing;

    [SerializeField] private Sprite m_leftClickSprite;
    [SerializeField] private Sprite m_leftClickNotPossibleSprite;
    [SerializeField] private Sprite m_rightClickNotPossibleSprite;
    [SerializeField] private Sprite m_rightClickSprite;
    [SerializeField] private Sprite m_EKey;
    [SerializeField] private Sprite m_EKeyNotInteract;

    [SerializeField] private Color m_dontInteract;
    [SerializeField] private Color m_possessed;

    private Vector3 m_initialSizeDoudouUi;
    private Vector3 m_initialSizeLampeUi;
    private void Awake()
    {
        m_gameManager = FindObjectOfType<GameManager>();
        m_indicInteraction.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        m_indicInteraction.SetActive(false);
        m_UILampe.GetComponent<Image>().color = new Color32(100, 100, 100, 255);
        m_UIDoudou.GetComponent<Image>().color = new Color32(100, 100, 100, 255);
        isAnimated = false;
        animActivated = false;
        indexAnim = 0;
        m_initialSizeDoudouUi = m_UIDoudou.transform.localScale;
        m_initialSizeLampeUi = m_UILampe.transform.localScale;
        m_alreadyShrinkingDoudou = 0;
        m_alreadyShrinkingLampe = 0;
    }
    //----------------------------------------------- Ui prendre et dropper un item ------------------------------------------//


    //Interactions quand raycast//

    public void TakableFlashlight()
    {
        m_indicInteraction.GetComponent<Image>().sprite = m_rightClickSprite;
        m_indicInteraction.SetActive(true);
    }
    //récup doudou ou la chaise
     public void TakableDoudou()
    {
        m_indicInteraction.GetComponent<Image>().sprite = m_leftClickSprite;
        m_indicInteraction.SetActive(true);
    }  
    
    public void NotChairTakable()
    {
        if(m_player.isLeftHandFull == true)
        {
            m_indicInteraction.GetComponent<Image>().sprite = m_leftClickNotPossibleSprite;
        }
        if (m_player.isRightHandFull == true)
        {
            m_indicInteraction.GetComponent<Image>().sprite = m_rightClickNotPossibleSprite;
        }
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
    }

    private IEnumerator GrowUpAndDown(GameObject p_go)
    {
        Vector3 beginningSize;
        if (ReferenceEquals(p_go, m_UIDoudou))
        {
            beginningSize = m_initialSizeDoudouUi;
            m_alreadyShrinkingDoudou++;
        }
        else
        {
            beginningSize = m_initialSizeLampeUi;
            m_alreadyShrinkingLampe++;
        }

        if ((ReferenceEquals(p_go, m_UIDoudou) && m_alreadyShrinkingDoudou < 2) || (ReferenceEquals(p_go, m_UILampe) && m_alreadyShrinkingLampe < 2))
        {
            while(p_go.transform.localScale.x < beginningSize.x * 2)
            {
                p_go.transform.localScale = Vector3.MoveTowards(p_go.transform.localScale, new Vector3(beginningSize.x*2, beginningSize.y*2, 1), 0.1f);
                yield return null;
            }
            while(p_go.transform.localScale.x > beginningSize.x)
            {
                p_go.transform.localScale = Vector3.MoveTowards(p_go.transform.localScale, beginningSize, 0.1f);
                yield return null;
            }
        }

        if (ReferenceEquals(p_go, m_UIDoudou))
        {
            m_alreadyShrinkingDoudou--;
        }
        else
        {
            m_alreadyShrinkingLampe--;
        }



    }
    
    private IEnumerator FadeInOut(GameObject p_go)
    {
        m_alreadyFlashing++;
        int numberOfRepetition = 0;
        while (numberOfRepetition <= 2)
        {
            p_go.GetComponent<Animator>().SetTrigger("FadeOut");
            yield return new WaitForSeconds(0.2f);
            p_go.GetComponent<Animator>().SetTrigger("FadeIn");
            yield return new WaitForSeconds(0.2f);
            numberOfRepetition++;
        }

        m_alreadyFlashing--;
        yield return null;
    }
    
    private IEnumerator FadeInOut(GameObject p_go,GameObject p_go2)
    {
        m_alreadyFlashing++;
        int numberOfRepetition = 0;
        while (numberOfRepetition <= 2)
        {
            p_go.GetComponent<Animator>().SetTrigger("FadeOut");
            p_go2.GetComponent<Animator>().SetTrigger("FadeOut");
            yield return new WaitForSeconds(0.2f);
            p_go.GetComponent<Animator>().SetTrigger("FadeIn");
            p_go2.GetComponent<Animator>().SetTrigger("FadeIn");
            yield return new WaitForSeconds(0.2f);
            numberOfRepetition++;
        }
        m_alreadyFlashing--;
        yield return null;
    }
    //----------------------------------------------- UI Objets du joueur ------------------------------------------//

    //Normal si le joueur appuie sur E sa blink
    public void AnimUi()
    {
        if (m_alreadyFlashing < 2)
        {
            Debug.Log("Je rentre dans ANimUI");
            if(m_player.isLeftHandFull == true && m_player.isRightHandFull == true)
            {
                StartCoroutine(FadeInOut(m_UIDoudou, m_UILampe));
                StartCoroutine(GrowUpAndDown(m_UIDoudou));
                StartCoroutine(GrowUpAndDown(m_UILampe));
            }
        
            if (m_player.isLeftHandFull == true && m_player.isRightHandFull == false)
            {
                StartCoroutine(FadeInOut(m_UIDoudou));
                StartCoroutine(GrowUpAndDown(m_UIDoudou));
            }
        
            if(m_player.isRightHandFull == true && m_player.isLeftHandFull == false)
            {
                StartCoroutine(FadeInOut(m_UILampe));
                StartCoroutine(GrowUpAndDown(m_UILampe));
                Debug.Log("Je rentre dans ");
            }
            if(m_player.isTwoHandFull == true)
            {
                StartCoroutine(FadeInOut(m_UIDoudou, m_UILampe));
                StartCoroutine(GrowUpAndDown(m_UIDoudou));
                StartCoroutine(GrowUpAndDown(m_UILampe));
            }
        }
    }
    public void DropSomethingBefore()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            AnimUi();
        }
        if(m_player.isLeftHandFull == true)
        {
            m_indicInteraction.GetComponent<Image>().sprite = m_EKeyNotInteract;
            m_indicInteraction.SetActive(true);
        }
        else if (m_player.isRightHandFull == true)
        {
            m_indicInteraction.GetComponent<Image>().sprite = m_EKeyNotInteract;
            m_indicInteraction.SetActive(true);
        }
        else if(m_player.isTwoHandFull == true)
        {
            m_indicInteraction.GetComponent<Image>().sprite = m_EKeyNotInteract;
            m_indicInteraction.SetActive(true);
        }
    }
    public void StopRaycastBefore()
    {
        animActivated = false;
        if (m_player.isLeftHandFull == true)
        {
            m_indicInteraction.SetActive(false);
            m_UIDoudou.GetComponent<Animator>().SetTrigger("Reset");
        }
        else if (m_player.isRightHandFull == true)
        {
            m_indicInteraction.SetActive(false);
            m_UILampe.GetComponent<Animator>().SetTrigger("Reset");
        }
        else if (m_player.isTwoHandFull == true)
        {
            m_indicInteraction.SetActive(false);
            m_UIDoudou.GetComponent<Animator>().SetTrigger("Reset");
            m_UILampe.GetComponent<Animator>().SetTrigger("Reset");
        }
    }
    
    //Affichage en bas à droite
    public void TakeLampe()
    {
        //D�sactiver la Ui qui prend l'objet
        //Activer l'Ui en bas � droite

        m_UILampe.GetComponent<Image>().color = new Color32(255, 255, 225, 255);     
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
