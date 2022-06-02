using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]

public class CaisseProto : MonoBehaviour
{

    [SerializeField] private GameObject m_thisGameObject;
    [SerializeField] private PlayerController m_player;
    private Rigidbody m_rbody;
    [SerializeField] private UiManager m_uiManager;
    private SphereCollider m_sphereCollider;
    [SerializeField] private Transform m_twoHandsContainer;
    [SerializeField] private Transform m_finalPosition;
    [SerializeField] private Transform m_playerLocked;
    [SerializeField] private LayerMask m_playerMask;
    [SerializeField] private FunRadio m_phone;
    [SerializeField] private LayerMask m_twoHandsMask;
    [SerializeField] [Range(0.1f, 5)] private float m_rangeCol;
    
    [SerializeField][Range(1,100)] private float m_speedPourcentReduction;
    [SerializeField] private BoxCollider m_boxPhone;
    private float m_pourcentSpeed;

    public bool canTake;
    private bool inPhoneBox = false;
    private bool chairLocked = false;
    private bool playerCanLock = false;
    public bool isPlayerLocked = false;
    public bool hasPhoneAnwser = false;
    private Renderer m_thisRend;
    private void Awake()
    {

        m_rbody = GetComponent<Rigidbody>();
        m_sphereCollider = GetComponent<SphereCollider>();
        m_thisRend = GetComponent<Renderer>();
        
        inPhoneBox = false;
        chairLocked = false;
        playerCanLock = false;
        isPlayerLocked = false;
        hasPhoneAnwser = false;
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

    private void OnTriggerEnter(Collider other)
    {
        

        if ((m_playerMask.value & (1 << other.gameObject.layer)) > 0)
        {
            canTake = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other == m_boxPhone)
        {
            inPhoneBox = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other == m_boxPhone)
        {
            inPhoneBox = false;
        }
        if ((m_playerMask.value & (1 << other.gameObject.layer)) > 0)
        {            
            canTake = false;
            m_thisRend.material.SetFloat("_BooleanFloat", 0f);
        }
    }
    
    private void Update()
    {
        OnHand();
    }
    
    public void CanTake()
    {
        if (canTake == true)
        {
            if(m_player.hasChair == false)
            {
                if (chairLocked == false)
                {
                    m_thisGameObject.transform.SetParent(m_twoHandsContainer);
                    m_thisGameObject.transform.localPosition = new Vector3(0f, -0.8f, 0f);
                    m_thisGameObject.transform.localRotation = Quaternion.Euler(-90f, m_twoHandsContainer.transform.localRotation.y, m_twoHandsContainer.transform.localRotation.z);
                    m_rbody.useGravity = false;
                    m_rbody.isKinematic = true;
                    m_sphereCollider.enabled = false;
                    m_player.m_speed = 1.5f;
                    m_player.hasChair = true;
                    canTake = false;
                    m_player.isTwoHandFull = true;
                }
            }
            
        }
    }

    public void LockPlayer()
    {
        isPlayerLocked = true;
        m_sphereCollider.enabled = false;
        m_player.OnChair = true;
        m_player.m_speed = 0f;
        m_player.transform.position = m_playerLocked.transform.position;
        m_thisRend.material.SetFloat("_BooleanFloat", 0f);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UnlockPlayer();
        }
    }
    public void UnlockPlayer()
    {
        m_player.OnChair = false;
        playerCanLock = false;
        isPlayerLocked = false;
        m_player.m_speed = 2f;
    }
    private void LateUpdate()
    {
        if(playerCanLock == true) 
        {
            if (chairLocked == true)
            {
                LockPlayer();
            }
        }
        
    }
    public void OnHand()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (inPhoneBox == false)
            {
                m_thisGameObject.transform.parent = null;
                m_rbody.useGravity = true;
                m_rbody.isKinematic = false;
                m_sphereCollider.enabled = true;
                m_player.m_speed = 2f;
                m_player.hasChair = false;
                Debug.Log("dropper la chaise car il n'est pas dans le collider du phone");
                m_player.isTwoHandFull = false;
            }
            else
            {
                m_thisGameObject.transform.parent = null;
                m_thisGameObject.transform.rotation = Quaternion.Euler(-90f, m_finalPosition.transform.localRotation.y, transform.localRotation.z);
                m_thisGameObject.transform.position = m_finalPosition.transform.position;
                m_sphereCollider.enabled = true;
                m_boxPhone.enabled = false;
                playerCanLock = true;
                chairLocked = true;
            }
        }

    }
}
