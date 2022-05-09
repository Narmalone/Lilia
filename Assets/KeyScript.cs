using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    [SerializeField] private GameObject m_thisObject;
    [SerializeField] private Rigidbody m_thisRb;
    [SerializeField] private GameObject m_keyUi;
    [SerializeField, Tooltip("Une fois que le joueur ramasse la clé")] private Transform m_containerKeyAfterEvent;
    [SerializeField, Tooltip("Une fois que le joueur finis le QTE")] private Transform m_containerDrop;
    [SerializeField] private GameManager m_gameManager;
    [SerializeField] private PlayerController m_player;
    [SerializeField] private AudioManagerScript m_audioScript;
    [SerializeField] private UiManager m_uiManager;
    [SerializeField] private QTEManager m_qte;
    [SerializeField] private LayerMask m_playerMask;
    [SerializeField] private BoxCollider m_thisBox;

    public bool m_setKeyPos = false;
    public bool m_dropKeyAfterEvent = false;
    private void Awake()
    {
        m_gameManager = FindObjectOfType<GameManager>();
        m_thisRb = GetComponent<Rigidbody>();
        m_keyUi.SetActive(false);
        m_thisBox.enabled = true;
    }
    private void Update()
    {
       if(m_qte.m_qteIsOver == true)
        {
            if(m_dropKeyAfterEvent == false)
            {
                m_thisBox.transform.position = m_containerDrop.transform.position;
                m_audioScript.Play("KeySoundEvent_1");
                m_dropKeyAfterEvent = true;
            }
        }
       if(m_gameManager.gotKey == true)
        {
            DropKey();
        }
    }
    public void DropKey()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if(m_player.m_flashlightIsPossessed == false)
            {
                m_thisObject.transform.SetParent(null);
                m_keyUi.SetActive(false);
                m_setKeyPos = false;
                m_thisObject.transform.position = m_containerKeyAfterEvent.transform.position;
                m_thisBox.enabled = true;
                m_gameManager.gotKey = false;
                Debug.Log("le joueur a droppé la clé");

            }
        }
    }
    public void SetKeyPos()
    {
        if(m_setKeyPos == false)
        {
            if (m_player.m_flashlightIsPossessed == false)
            {
                Debug.Log("set key pos");
                m_thisRb.useGravity = false;
                m_thisRb.isKinematic = true;
                m_thisBox.enabled = false;
                m_thisObject.transform.SetParent(m_containerKeyAfterEvent);
                m_thisObject.transform.localRotation = m_containerKeyAfterEvent.transform.localRotation;
                m_thisObject.transform.position = m_containerKeyAfterEvent.transform.position;
                m_setKeyPos = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if ((m_playerMask.value & (1 << other.gameObject.layer)) > 0)
        {
            if (m_player.m_flashlightIsPossessed == false)
            {
                m_uiManager.TakableObject();
                if (Input.GetKeyDown(KeyCode.E))
                {
                    PlayerGotKey();
                    SetKeyPos();
                }              
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if ((m_playerMask.value & (1 << other.gameObject.layer)) > 0)
        {
            m_uiManager.DisableUi();
        }
    }
    public void PlayerGotKey()
    {
        Debug.Log("player got key");
        if(m_gameManager.gotKey == false)
        {
            m_gameManager.gotKey = true;
            m_uiManager.DisableUi();
            m_keyUi.SetActive(true);
        }
      
    }
}
