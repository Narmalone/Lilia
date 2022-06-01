using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;
using FMODUnity;
using FMOD.Studio;
public class Doudou : MonoBehaviour
{
    [SerializeField]UiManager uiManager;
    [SerializeField] private GameManager m_gameManager;
    [SerializeField] private LayerMask m_stairsMask;
    [SerializeField, Tooltip("R�f�rence de la torche")]private GameObject m_doudou;
    public List<Rigidbody> m_bones;
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

    public StudioEventEmitter m_clips;
    [SerializeField] private FMODUnity.EventReference m_fmodEventPickUp;
    private EventInstance m_instancePickUp;

    [SerializeField] private FMODUnity.EventReference m_fmodEventDrop;
    private EventInstance m_instanceDrop;

    [SerializeField] private AudioManagerScript m_audio;
    
    private void Awake()
    {
        m_boxDoudouColider = m_doudou.GetComponent<BoxCollider>();
        m_rbDoudou = m_doudou.GetComponent<Rigidbody>();
        m_callEvent = false;
        m_gameManager = FindObjectOfType<GameManager>();
        m_gOPlayer = FindObjectOfType<PlayerController>().gameObject;
        m_doudou.transform.localRotation = Quaternion.Euler(0f, -0f, 0f);

        //set instance pickup
        m_instancePickUp = RuntimeManager.CreateInstance(m_fmodEventPickUp);
        RuntimeManager.AttachInstanceToGameObject(m_instancePickUp, GetComponent<Transform>());

        //set instance drop
        m_instanceDrop = RuntimeManager.CreateInstance(m_fmodEventDrop);
        RuntimeManager.AttachInstanceToGameObject(m_instanceDrop, GetComponent<Transform>());
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
        foreach(Rigidbody p_rbody in m_bones)
        {
            p_rbody.isKinematic = true;
            p_rbody.useGravity = false;
        }
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
            m_instancePickUp.setVolume(m_audio.volumeSound);
            m_instancePickUp.start();
        }
        else
        {
            m_instancePickUp.setVolume(m_audio.volumeSound);
            m_instancePickUp.start();
            //m_clips.Play();
            m_rbDoudou.isKinematic = true;
            m_rbDoudou.useGravity = false;
            m_doudou.transform.position = new Vector3(1100f, 1100f, 1100f);
            m_doudou.GetComponent<BoxCollider>().enabled = false;
            uiManager.DisableUi();
        }

    }

    public void DropItem()
    {
        foreach (Rigidbody p_rbody in m_bones)
        {
            p_rbody.isKinematic = false;
            p_rbody.useGravity = true;
        }
        m_doudou.transform.localPosition = m_emplacementDoudou.transform.position;
        m_doudou.transform.SetParent(m_emplacementDoudou);
        m_doudou.transform.localRotation = Quaternion.Euler(0f,-0f,0f);
        m_doudou.transform.parent = null;
        m_rbDoudou.isKinematic = false;
        m_rbDoudou.useGravity = true;
        m_instanceDrop.setVolume(m_audio.volumeSound);
        m_instanceDrop.start();

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
