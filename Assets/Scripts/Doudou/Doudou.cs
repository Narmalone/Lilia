using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class Doudou : MonoBehaviour
{
    [SerializeField]UiManager uiManager;
    [SerializeField] private GameManager m_gameManager;
    [SerializeField] private LayerMask m_stairsMask;
    [SerializeField, Tooltip("R�f�rence de la torche")]private GameObject m_doudou;
    [SerializeField] private Transform m_emplacementDoudou;
    [SerializeField] private AppearThings m_appear;
    [SerializeField] private QTEManager m_qte;
    private BoxCollider m_boxDoudouColider;
    private Rigidbody m_rbDoudou;
    [SerializeField] private float m_stepOffset = 0.2f;
    public bool m_callEvent = false;
    public bool m_callEventEnded = false;
    public bool TakeBeforeChase = false;
    [SerializeField] WaypointsEvent m_waypointsEvent;
    private GameObject m_gOPlayer;
    
    [SerializeField] private FMODUnity.EventReference m_fmodEventPickUp;
    
    [SerializeField] private FMODUnity.EventReference m_fmodEventDrop;

    [SerializeField] private AssetMenuScriptValue m_asset;
    
    private void Awake()
    {
        m_boxDoudouColider = m_doudou.GetComponent<BoxCollider>();
        m_rbDoudou = m_doudou.GetComponent<Rigidbody>();
        m_callEvent = false;
        m_gameManager = FindObjectOfType<GameManager>();
        m_gOPlayer = FindObjectOfType<PlayerController>().gameObject;
        m_doudou.transform.localRotation = Quaternion.Euler(0f, -0f, 0f);
    }
    private float m_yRotation = 0f;
    private void Update()
    {
        if(m_callEvent == true)
        {
            m_rbDoudou.isKinematic = true;
            m_rbDoudou.useGravity = false;
            m_gameManager.canPick = false;
        }
    }
    public void PickItem()
    {
        if(TakeBeforeChase == true)
        {
            m_appear.IAdontMove = false;
            m_qte.canDoQte = true;
            TakeBeforeChase = false;
            m_rbDoudou.isKinematic = true;
            m_rbDoudou.useGravity = false;
            m_doudou.transform.position = new Vector3(1100f, 1100f, 1100f);
            m_doudou.GetComponent<BoxCollider>().enabled = false;
            uiManager.DisableUi();
            FMODUnity.RuntimeManager.PlayOneShotAttached(m_fmodEventPickUp.Guid, m_gOPlayer.gameObject);
        }
        else
        {
            m_rbDoudou.isKinematic = true;
            m_rbDoudou.useGravity = false;
            m_doudou.transform.position = new Vector3(1100f, 1100f, 1100f);
            m_doudou.GetComponent<BoxCollider>().enabled = false;
            uiManager.DisableUi();
            FMODUnity.RuntimeManager.PlayOneShotAttached(m_fmodEventPickUp.Guid, m_gOPlayer.gameObject);
        }

    }

    public void DropItem()
    {
        m_doudou.transform.localPosition = m_emplacementDoudou.transform.position;
        m_doudou.transform.SetParent(m_emplacementDoudou);
        m_doudou.transform.localRotation = Quaternion.Euler(0f,-0f,0f);
        m_doudou.transform.parent = null;
        m_rbDoudou.isKinematic = false;
        m_rbDoudou.useGravity = true;
        FMODUnity.RuntimeManager.PlayOneShotAttached(m_fmodEventDrop.Guid,gameObject);

    }
    
    public void CallEventEnded()
    {
        m_callEventEnded = true;
        if(m_callEventEnded == true)
        {
            Debug.Log("l'event est fini");
            m_boxDoudouColider.enabled = true;
            m_rbDoudou.isKinematic = false;
            m_rbDoudou.useGravity = true;
            m_callEvent = false;
            m_gameManager.canPick = true;
            TakeBeforeChase = true;
        }
    }
}
