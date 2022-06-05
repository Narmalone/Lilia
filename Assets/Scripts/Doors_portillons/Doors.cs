using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Doors : MonoBehaviour
{
    [SerializeField] private LayerMask m_targetMask;
    [SerializeField] private PlayerController m_player;
    [SerializeField] private UiManager m_uiManager;
    [SerializeField] public bool isActivable = false;
    [SerializeField] private bool isCompleted = false;
    [SerializeField] public bool isDoor = false;
    [SerializeField] public bool isOpen = false;
    private bool isRay = false;

    [Header("Variables en rapport avec le temps"), Space(10)]
    [SerializeField, Tooltip("Vitesse � laquelle le joueur ouvre le portillon temps de secondes"), Range(0, 5)] private float m_speedToOpen;
    [SerializeField, Tooltip("Vitesse � laquelle la valeur s'incr�mente par seconde"), Range(0, 1)] private float m_incrementValue;

    [Header("Variables de r�f�rences"), Space(10)]
    [Tooltip("R�f�rence du prefab"), SerializeField] private Slider sliderInstance;
    [SerializeField, Tooltip("Position d'ou le Slider doit spawn")] private RectTransform m_pos;

    [Header("Animation"), Space(10)]
    [SerializeField] private Animator m_doorController;
    private string m_isOpenDoorAnim = "isOpen";
    private string m_isOpenPortillonAnim = "isOpen";
    private string m_isClosedPortillonAnim = "isClose";
    
    private string m_isOpenBackwardPortillonAnim = "isOpenBackward";
    private string m_isClosedBackwardPortillonAnim = "isCloseBackward";
    [SerializeField] private float m_timeBeforeClose = 1f;

    //false = animation 1, true = animation 2
    public bool isLeftTrigger = false;

    private Ray m_ray;
    private RaycastHit m_hit;
    private Slider mySlider;

    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        
    }
    private void Awake()
    {
        //Slider Instance et set variables

        mySlider = sliderInstance;
        mySlider.minValue = 0;
        mySlider.value = mySlider.minValue;
        mySlider.maxValue = m_speedToOpen;

        mySlider.gameObject.SetActive(false);

        //Get Animator
        if(m_doorController == null)
        {
            m_doorController.GetComponent<Animator>();
        }
        isCompleted = false;
        isOpen = false;
    }

    //Fonction ou le joueur doit maintenir la touche
    public void ActiveDoors()
    {
        if(isActivable == true)
        {
            if(isCompleted == false)
            {
                mySlider.gameObject.SetActive(true);
                if (m_incrementValue < m_speedToOpen)
                {
                    m_incrementValue += m_incrementValue * Time.deltaTime;
                    mySlider.value = m_incrementValue;
                }
                else if (m_incrementValue >= m_speedToOpen)
                {
                    //Lancer fonction qui ouvre
                    OnComplete();
                }
            }
        }
    }
    //Lorsque le joueur � mont� le slider au max
    public void OnComplete()
    {
        mySlider.gameObject.SetActive(false);
        m_uiManager.DisableUi();
        isCompleted = true;
        //Si c'est une porte
        if (isDoor == true)
        {
            m_doorController.SetTrigger(m_isOpenDoorAnim);
            isActivable = false;
            isOpen = true;
            gameObject.layer = default;
        }
        //Si c'est un portillon
        else if (isDoor == false)
        {
            if(isLeftTrigger == false)
            {
                m_doorController.SetTrigger(m_isOpenBackwardPortillonAnim);
            }
            if(isLeftTrigger == true)
            {
                m_doorController.SetTrigger(m_isOpenPortillonAnim);
                Debug.Log("open anim portillon normal");
            }
            isActivable = false;
            isOpen = true;
            StartCoroutine(Chrono());
        }
        //Jouer son
        //la porte l'objet n'est plus activable ou si c un portillon trouver sol
    }

    IEnumerator Chrono()
    {
        //Attendre le temps donn� dans l'inspecteur en seconde
        yield return new WaitForSeconds(m_timeBeforeClose);
        ResetPortillon();
    }

    //R�initialisation des variables et animations de fermeture de portillon
    private void ResetPortillon()
    {
        StopAllCoroutines();
        mySlider.minValue = 0;
        m_incrementValue = 0.2f;
        mySlider.value = mySlider.minValue;
        mySlider.maxValue = m_speedToOpen;
        if(isOpen == true)
        {
            if(isLeftTrigger == false)
            {
                m_doorController.SetTrigger(m_isClosedBackwardPortillonAnim);
            }
            if(isLeftTrigger == true)
            {
                m_doorController.SetTrigger(m_isClosedPortillonAnim);
            }
        }
        isOpen = false;
        isActivable = true;
        isCompleted = false;
    }

    //D�sactiver slider si y'a pas de raycast
    public void DisableSlider()
    {
        mySlider.gameObject.SetActive(false);
    }
}
