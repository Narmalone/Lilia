using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]

public class CaisseProto : MonoBehaviour
{

    [SerializeField] private Event1 m_triggerEvent;
    [SerializeField] private GameObject m_thisGameObject;
    private PlayerController m_player;
    private Rigidbody m_rbody;
    private UiManager m_uiManager;
    private SphereCollider m_sphereCollider;
    [SerializeField] private Transform m_twoHandsContainer;
    [SerializeField] private LayerMask m_playerMask;
    
    [SerializeField] [Range(0.1f, 5)] private float m_rangeCol;
    
    [SerializeField][Range(1,100)] private float m_speedPourcentReduction;

    [SerializeField] private Vector3 m_positionFinal;
    
    [SerializeField] private Vector3 m_rotationFinal;
    
    private float m_pourcentSpeed;

    public bool onHand;
    public bool canTake;

    private void Awake()
    {
        if ((m_player = FindObjectOfType<PlayerController>()) == null)
        {
            Debug.Log("Pas de joueur", this);
        }
        
        if ((m_uiManager = FindObjectOfType<UiManager>()) == null)
        {
            Debug.Log("Pas de UiManager", this);
        }

        m_rbody = GetComponent<Rigidbody>();
        
        m_sphereCollider = GetComponent<SphereCollider>();

        onHand = false;
        canTake = false;
    }
    
    private void OnEnable()
    {
        m_triggerEvent.onTriggered += HandleTriggerEvent;
        
    }
    
    private void OnValidate()
    {
        if (m_sphereCollider == null)
        {
            m_sphereCollider = GetComponent<SphereCollider>();
        }
        m_sphereCollider.radius = m_rangeCol;

        m_pourcentSpeed = 100 - m_speedPourcentReduction;
    }
    
    private void OnDisable()
    {
        m_triggerEvent.onTriggered -= HandleTriggerEvent;
    }
    
    public void HandleTriggerEvent(Vector3 p_newPos)
    {
        if (onHand == true){
            transform.position = m_positionFinal;
            transform.rotation = Quaternion.Euler(m_rotationFinal);
            m_thisGameObject.transform.parent = null;
            m_rbody.useGravity = true;
            m_rbody.isKinematic = false;
            onHand = false;
            m_player.m_speed /= m_pourcentSpeed / 100; 
            Debug.Log("l�cher la caisse");
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((m_playerMask.value & (1 << other.gameObject.layer)) > 0)
        {
            canTake = true;
        }
        Debug.Log("Trigger enter");
    }

    private void OnTriggerExit(Collider other)
    {
        m_uiManager.DisableUi();
        if ((m_playerMask.value & (1 << other.gameObject.layer)) > 0)
        {
            m_uiManager.DisableUi();
            
            canTake = false;
        }
        Debug.Log("trigger exit");
    }
    
    private void Update()
    {
        CanTake();
        OnHand();
    }
    
    public void CanTake()
    {
        if (Input.GetKeyDown(KeyCode.E) && canTake == true && m_player.m_doudouIsPossessed == false && m_player.m_flashlightIsPossessed == false && onHand == false)
        {
            m_thisGameObject.transform.SetParent(m_twoHandsContainer);
            m_thisGameObject.transform.localPosition = new Vector3(0f, -0.8f, 0f);
            m_thisGameObject.transform.localRotation = Quaternion.Euler(-90f, m_twoHandsContainer.transform.localRotation.y, m_twoHandsContainer.transform.localRotation.z);
            m_rbody.useGravity = false;
            m_rbody.isKinematic = true;
            canTake = false;
            m_player.m_speed *= m_pourcentSpeed / 100; 
            Debug.Log("prendre l'objet");
            
        }
    }
    
    public void OnHand()
    {
        if (Input.GetKeyDown(KeyCode.G) && onHand == true)
        {
            m_thisGameObject.transform.parent = null;
            m_rbody.useGravity = true;
            m_rbody.isKinematic = false;
            onHand = false;
            m_player.m_speed /= m_pourcentSpeed / 100; 
            Debug.Log("l�cher la caisse");
        }
        if(onHand == true)
        {
            m_uiManager.DisableUi();
        }

    }
}