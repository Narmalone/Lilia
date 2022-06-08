using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;
public class Doors : MonoBehaviour
{
    [SerializeField] private LayerMask m_targetMask;
    [SerializeField] private PlayerController m_player;
    [SerializeField] private UiManager m_uiManager;
    [SerializeField] public bool isActivable = false;
    [SerializeField] private bool isCompleted = false;
    [SerializeField] public bool isDoor = false;
    [SerializeField] public bool isOpen = false;
    public bool isNotReactivable = false;
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
    
    [SerializeField]
    private EventReference m_fmodEventPortillonOpen;
    
    [SerializeField]
    private EventReference m_fmodEventPortillonClose;
    
    [SerializeField]
    private EventReference m_fmodEventPorteOpen;

    private FirstPersonOcclusion m_occlusion;

    //false = animation 1, true = animation 2
    public bool isLeftTrigger = false;

    private Ray m_ray;
    private RaycastHit m_hit;
    private Slider mySlider;

    [SerializeField, Tooltip("0 = ouverture d'une porte, 1 = ouverture portillon, 2 = fermeture portillon")] private StudioEventEmitter[] m_clip;

    private PlayerScriptAnim m_pcAnim;

    TriggerFacingPortillon m_triggLeft;
    TriggerBackwardPortillon m_triggRight;

    private void Awake()
    {
        m_triggLeft = GetComponentInChildren<TriggerFacingPortillon>();
        m_triggRight = GetComponentInChildren<TriggerBackwardPortillon>();
        //Slider Instance et set variables

        m_occlusion = FindObjectOfType<FirstPersonOcclusion>();
        
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

                    Debug.Log("Compl�t�");
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
            m_clip[0].Play();
            m_doorController.SetTrigger(m_isOpenDoorAnim);
            isActivable = false;
            isOpen = true;
            gameObject.layer = default;
            m_pcAnim.canPlayAnim = true;
            m_pcAnim.PlayAnimPlayerToPortillons();
            EventInstance fmodInstance = RuntimeManager.CreateInstance(m_fmodEventPorteOpen.Guid);
            RuntimeManager.AttachInstanceToGameObject(fmodInstance, gameObject.transform);
            fmodInstance.start();
            m_occlusion.AddInstance(fmodInstance);
            fmodInstance.release();
        }
        //Si c'est un portillon
        else if (isDoor == false)
        {
            if(isLeftTrigger == false)
            {
                m_clip[1].Play();
                m_doorController.SetTrigger(m_isOpenBackwardPortillonAnim);
                if (m_triggLeft.isMobActivated == false && m_triggRight.isMobActivated == false)
                {
                    m_pcAnim.PlayAnimPlayerToPortillons();
                }
            }
            if (isLeftTrigger == true)
            {
                m_clip[1].Play();
                m_doorController.SetTrigger(m_isOpenPortillonAnim);
                m_pcAnim.canPlayAnim = true;
                if (m_triggLeft.isMobActivated == false && m_triggRight.isMobActivated == false)
                {
                    m_pcAnim.PlayAnimPlayerToPortillons();
                }
            }
            isActivable = false;
            isOpen = true;
            if(isNotReactivable == false)
            {
                StartCoroutine(Chrono());
            }
            
            EventInstance fmodInstance = RuntimeManager.CreateInstance(m_fmodEventPortillonOpen.Guid);
            RuntimeManager.AttachInstanceToGameObject(fmodInstance, gameObject.transform);
            fmodInstance.start();
            m_occlusion.AddInstance(fmodInstance);
            fmodInstance.release();
        }
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
        m_clip[2].Play();
        if (isOpen == true)
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
        if (!isDoor)
        {
            EventInstance fmodInstance = RuntimeManager.CreateInstance(m_fmodEventPortillonClose.Guid);
            RuntimeManager.AttachInstanceToGameObject(fmodInstance, gameObject.transform);
            fmodInstance.start();
            m_occlusion.AddInstance(fmodInstance);
            fmodInstance.release();
        }

    }

    //D�sactiver slider si y'a pas de raycast
    public void DisableSlider()
    {
        mySlider.gameObject.SetActive(false);
    }
}
