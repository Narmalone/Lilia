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
    [SerializeField] private bool isActivable = false;
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
    [SerializeField] private float m_timeBeforeClose = 1f;
    
    [SerializeField]
    private EventReference m_fmodEventPortillonOpen;
    
    [SerializeField]
    private EventReference m_fmodEventPortillonClose;
    
    [SerializeField]
    private EventReference m_fmodEventPorteOpen;

    private FirstPersonOcclusion m_occlusion;

    private Ray m_ray;
    private RaycastHit m_hit;
    private Slider mySlider;



    private void Awake()
    {
        //Slider Instance et set variables

        m_occlusion = FindObjectOfType<FirstPersonOcclusion>();
        
        mySlider = sliderInstance;
        mySlider.minValue = 0;
        mySlider.value = m_incrementValue;
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
    private void LateUpdate()
    {

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
            m_doorController.SetTrigger(m_isOpenDoorAnim);
            isActivable = false;
            isOpen = true;
            
            EventInstance fmodInstance = RuntimeManager.CreateInstance(m_fmodEventPorteOpen.Guid);
            RuntimeManager.AttachInstanceToGameObject(fmodInstance, gameObject.transform);
            fmodInstance.start();
            m_occlusion.AddInstance(fmodInstance);
            fmodInstance.release();
        }
        //Si c'est un portillon
        else if (isDoor == false)
        {
            m_doorController.SetTrigger(m_isOpenPortillonAnim);
            isActivable = false;
            isOpen = true;
            StartCoroutine(Chrono());
            Debug.Log("le bb");
            
            EventInstance fmodInstance = RuntimeManager.CreateInstance(m_fmodEventPortillonOpen.Guid);
            RuntimeManager.AttachInstanceToGameObject(fmodInstance, gameObject.transform);
            fmodInstance.start();
            m_occlusion.AddInstance(fmodInstance);
            fmodInstance.release();
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
        mySlider.value = m_incrementValue;
        mySlider.maxValue = m_speedToOpen;
        if(isOpen == true)
        {
            m_doorController.SetTrigger(m_isClosedPortillonAnim);
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
