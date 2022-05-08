using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PortillonScript : MonoBehaviour
{
    [Header("References scripts")]
    [SerializeField] private GameManager m_gameManager;
    [SerializeField] private UiManager m_uiManager;
    
    [Header("References dans l'objet"), Space(10)]
    [SerializeField] private BoxCollider m_boxCollider;
    [SerializeField] private MeshCollider m_thisMesh;
    public Slider sliderInstance;

    [Header("References Mask"), Space(10)]
    [SerializeField] private LayerMask m_playerMask;

    private Animator m_animator;
    private string m_openAnim = "Open";
    
    [Header("Variables pour Designers"), Space(10)]
    [SerializeField, Tooltip("Vitesse à laquelle le joueur ouvre le portillon temps de secondes"), Range(0,5)]private float m_speedToOpen;
    [SerializeField, Tooltip("Vitesse à laquelle la valeur s'incrémente par seconde"),Range(0,1)]private float m_incrementValue;
    [SerializeField, Tooltip("Booléen qui définit si le portillon est activable ou pas")]private bool m_isActivable;
    [SerializeField, Tooltip("Couleur du portillon quand il est activable")] private Color m_activePortillonColor;
    [SerializeField, Tooltip("Couleur du portillon quand il n'est pas activable")] private Color m_inactivePortillonColor;

    [Header("Références Renderer et materials"), Space(10)]
    private Renderer m_myRend;
    private void Awake()
    {
        if (m_animator == null)
        {
            m_animator = GetComponentInParent<Animator>();
        };

        m_myRend = GetComponent<Renderer>();

        sliderInstance.minValue = 0;
        sliderInstance.value = m_incrementValue;
        sliderInstance.maxValue = m_speedToOpen;
    }

    private void Start()
    {
        sliderInstance.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (m_isActivable == true)
        {
            Activable();
        }
        else if (m_isActivable == false)
        {
            Unactivable();
        }
    }

    public void OnComplete()
    {
        m_animator.SetBool(m_openAnim,true);
        Animator.StringToHash(m_openAnim);
        m_boxCollider.enabled = false;
        m_thisMesh.enabled = false;
        Debug.Log("jouer l'anim");
        m_uiManager.DisableUi();
    }

    public void UnlockPortillon()
    {
        if (m_isActivable == true)
        {
            Debug.Log("dans le unlock portillon");
            sliderInstance.gameObject.SetActive(true);
            if (m_incrementValue < m_speedToOpen)
            {
                m_incrementValue += m_incrementValue * Time.deltaTime;
                sliderInstance.value = m_incrementValue;
                Debug.Log(m_incrementValue);
            }
            else if (m_incrementValue >= m_speedToOpen)
            {
                OnComplete();
                sliderInstance.gameObject.SetActive(false);
                Debug.Log("Complété");
            } 
        }
    }

    public void Activable()
    {
        m_myRend.material.color = m_activePortillonColor;
        m_boxCollider.enabled = true;
    }
    public void Unactivable()
    {
        m_myRend.material.color = m_inactivePortillonColor;
        m_boxCollider.enabled = false;
    }
}
