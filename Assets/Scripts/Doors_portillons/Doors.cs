using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
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
    [SerializeField, Tooltip("Vitesse à laquelle le joueur ouvre le portillon temps de secondes"), Range(0, 5)] private float m_speedToOpen;
    [SerializeField, Tooltip("Vitesse à laquelle la valeur s'incrémente par seconde"), Range(0, 1)] private float m_incrementValue;

    [Header("Variables de références"), Space(10)]
    [Tooltip("Référence du prefab"), SerializeField] private Slider sliderInstance;
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

    [SerializeField, Tooltip("0 = ouverture d'une porte, 1 = ouverture portillon, 2 = fermeture portillon")] private StudioEventEmitter[] m_clip;

    private PlayerScriptAnim m_pcAnim;
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
        if(m_pcAnim == null)
        {
            m_pcAnim = FindObjectOfType<PlayerScriptAnim>();
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
    //Lorsque le joueur à monté le slider au max
    public void OnComplete()
    {
        mySlider.gameObject.SetActive(false);
        m_uiManager.DisableUi();
        isCompleted = true;
        //Si c'est une porte
        if (isDoor == true)
        {
            m_clip[0].Play();
            m_doorController.SetTrigger(m_isOpenDoorAnim);
            isActivable = false;
            isOpen = true;
            gameObject.layer = default;
            m_pcAnim.canPlayAnim = true;
            m_pcAnim.PlayAnimPlayerToPortillons();
        }
        //Si c'est un portillon
        else if (isDoor == false)
        {
            if(isLeftTrigger == false)
            {
                m_clip[1].Play();
                m_doorController.SetTrigger(m_isOpenBackwardPortillonAnim);
                m_pcAnim.PlayAnimPlayerToPortillons();
            }
            if (isLeftTrigger == true)
            {
                m_clip[2].Play();
                m_doorController.SetTrigger(m_isOpenPortillonAnim);
                m_pcAnim.canPlayAnim = true;
                m_pcAnim.PlayAnimPlayerToPortillons();
            }
            isActivable = false;
            isOpen = true;
            StartCoroutine(Chrono());
        }
    }

    IEnumerator Chrono()
    {
        //Attendre le temps donné dans l'inspecteur en seconde
        yield return new WaitForSeconds(m_timeBeforeClose);
        ResetPortillon();
    }

    //Réinitialisation des variables et animations de fermeture de portillon
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

    //Désactiver slider si y'a pas de raycast
    public void DisableSlider()
    {
        mySlider.gameObject.SetActive(false);
    }
}
