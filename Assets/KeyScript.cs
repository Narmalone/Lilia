using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    [SerializeField] private GameObject m_thisObject;
    [SerializeField] private GameObject m_keyUi;
    [SerializeField, Tooltip("Une fois que le joueur fais tomber la clé elle se tp à ce transform")] private Transform m_containerKeyAfterEvent;
    [SerializeField] private AudioManagerScript m_audioScript;
    [SerializeField] private UiManager m_uiManager;
    [SerializeField] private QTEManager m_qte;
    [SerializeField] private LayerMask m_playerMask;
    [SerializeField] private BoxCollider m_thisBox;

    public bool m_setKeyPos = false;
    public bool KeyPossessed = false;
    private void Awake()
    {
        m_keyUi.SetActive(false);
        m_thisBox.enabled = false;
    }
    private void Update()
    {
        if(m_qte.m_qteIsOver == true)
        {
            SetKeyPos();
        }
    }
    public void SetKeyPos()
    {
        if(m_setKeyPos == false)
        {
            m_thisBox.enabled = true;
            m_thisObject.transform.localPosition = m_containerKeyAfterEvent.transform.localPosition;
            m_audioScript.Play("KeySoundEvent_1");
            m_setKeyPos = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((m_playerMask.value & (1 << other.gameObject.layer)) > 0)
        {
            m_uiManager.TakableObject();
            
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            m_uiManager.DisableUi();
            PlayerGotKey();
            Debug.Log("le joueur a récupéré la clée");
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
        KeyPossessed = true;
        m_keyUi.SetActive(true);
        m_thisObject.transform.localPosition = new Vector3(200f, 200f, 200f);
    }
}
