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
    [SerializeField] private LayerMask m_groundMask;
    [SerializeField, Tooltip("R�f�rence de la torche")]private GameObject m_doudou;

    [SerializeField] private Transform m_emplacementDoudou;
    [SerializeField] private AppearThings m_appear;
    [SerializeField] private QTEManager m_qte;
    //private BoxCollider m_boxDoudouColider;
    public Rigidbody m_rbDoudou;
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
    public Renderer myRend;
    public Transform m_RootComponent;

    [SerializeField] RagdollScript m_ragdoll;

    [SerializeField] private StudioEventEmitter m_clip;
    private void Awake()
    {
        //m_boxDoudouColider = m_doudou.GetComponent<BoxCollider>();
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
    private void Start()
    {
        m_ragdoll.ActivateRagdoll();
    }
    private float m_yRotation = 0f;
    private void Update()
    {
        if (m_callEvent == true)
        {
            m_rbDoudou.isKinematic = true;
            m_rbDoudou.useGravity = false;
            m_gameManager.canPick = false;
        }

        if(m_RootComponent.transform.position.y <= -20)
        {
            m_rbDoudou.transform.position = m_doudou.transform.position;
        }
    }
    public void PickItem()
    {

        if (TakeBeforeChase == true)
        {
            StartCoroutine(StartIaMoove());
            m_qte.canDoQte = true;
            TakeBeforeChase = false;
            m_clip.Play();
            m_doudou.transform.position = new Vector3(1100f, 1100f, 1100f);
            uiManager.DisableUi();
            m_instancePickUp.setVolume(m_audio.volumeSound);
            m_instancePickUp.start();
        }
        else
        {
            m_instancePickUp.setVolume(m_audio.volumeSound);
            m_instancePickUp.start();
            m_rbDoudou.isKinematic = true;
            m_rbDoudou.useGravity = false;
            m_doudou.transform.position = new Vector3(1100f, 1100f, 1100f);
            uiManager.DisableUi();
            m_ragdoll.DisableRagdoll();
        }

    }
    IEnumerator StartIaMoove()
    {
        yield return new WaitForSeconds(5f);
        m_appear.IAdontMove = false;
        StopCoroutine(StartIaMoove());
    }
    public void DropItem()
    {
        m_RootComponent.transform.position = m_doudou.transform.position;
        m_doudou.transform.localPosition = m_emplacementDoudou.transform.position;
        m_doudou.transform.SetParent(m_emplacementDoudou);
        m_doudou.transform.localRotation = Quaternion.Euler(0f,-0f,0f);
        m_doudou.transform.parent = null;

        m_ragdoll.ActivateRagdoll();
        m_instanceDrop.setVolume(m_audio.volumeSound);
        m_instanceDrop.start();

    }
    public void CallEventEnded()
    {
        m_callEventEnded = true;
        if(m_callEventEnded == true)
        {
            m_ragdoll.ActivateRagdoll();
            Debug.Log("l'event est fini");
            //m_boxDoudouColider.enabled = true;
            m_callEvent = false;
            m_gameManager.canPick = true;
            TakeBeforeChase = true;
        }
    }
}
