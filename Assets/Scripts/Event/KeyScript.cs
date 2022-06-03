using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
public class KeyScript : MonoBehaviour
{
    [SerializeField] private GameObject m_thisObject;
    [SerializeField] private Rigidbody m_thisRb;
    [SerializeField] public GameObject m_keyUi;
    [SerializeField, Tooltip("Une fois que le joueur ramasse la cl�")] private Transform m_containerKeyAfterEvent;
    [SerializeField, Tooltip("Une fois que le joueur finis le QTE")] private Transform m_containerDrop;
    [SerializeField] private GameManager m_gameManager;
    [SerializeField] private PlayerController m_player;
    [SerializeField] private AudioManagerScript m_audioScript;
    [SerializeField] private UiManager m_uiManager;
    [SerializeField] private QTEManager m_qte;
    [SerializeField] private LayerMask m_playerMask;
    [SerializeField, Tooltip("0 est au drop, 1 prendre clé")] private StudioEventEmitter[] m_clip;

    private Ray m_ray;

    private RaycastHit m_hit;
    
    public bool m_setKeyPos = false;
    public bool m_dropKeyAfterEvent = false;

    private Renderer m_thisRend;

    //IntencityDetails = 0.2f
    //bool float 1f
    private void Awake()
    {
        m_gameManager = FindObjectOfType<GameManager>();
        m_thisRb = GetComponent<Rigidbody>();
        m_thisRend = GetComponent<Renderer>();
        m_keyUi.SetActive(false);
        m_setKeyPos = false;
    }

    private void LateUpdate()
    {
        if(m_qte.m_qteIsOver == true)
        {
            m_thisRend.material.SetFloat("IntencityDetails", 0.2f);
            m_thisRend.material.SetFloat("_BooleanFloat", 1f);
            SetKeyPos();
        }
    }
    public void SetKeyPos()
    {
        if (m_setKeyPos == false)
        {
            m_thisObject.transform.position = m_containerDrop.transform.position;
            m_thisObject.transform.rotation = m_containerDrop.transform.rotation;
            m_setKeyPos = true;
        }
    }
    public void CanTake()
    {
        if (m_qte.m_qteIsOver == true)
        {
            m_uiManager.TakableObject();
            if (Input.GetKeyDown(KeyCode.E))
            {
                PlayerGotKey();
                Debug.Log("raycast la clé");
            }
            if (m_dropKeyAfterEvent == false)
            {
                m_clip[0].Play();
                m_dropKeyAfterEvent = true;
            }
        }
    }
    public void PlayerGotKey()
    {
        Debug.Log("player got key");
        if(m_gameManager.gotKey == false)
        {
            m_clip[1].Play();
            m_gameManager.gotKey = true;
            m_thisObject.transform.position = new Vector3(1000f, 0f, 0f);
            m_uiManager.DisableUi();
            m_keyUi.SetActive(true);
        }

    }
}
