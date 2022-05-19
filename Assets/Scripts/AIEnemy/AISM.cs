using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class AISM : StateMachine
{
    [HideInInspector]
    public Patrouille m_patrouilleState;
    [HideInInspector]
    public Chasse m_chasseState;
    
    [SerializeField, Tooltip("L'agent de navigation AI")] private NavMeshAgent m_navAgent;
    [SerializeField, Tooltip("Reference script waypoints")] private Waypoints m_waypoints;
    [SerializeField, Tooltip("Instance du doudou")] private GameObject m_target;
    [SerializeField, Tooltip("Event détécté")] private Event1 m_triggeredEvent;
    
    [SerializeField, Tooltip("GO Joueur")] public PlayerController m_player;
    [SerializeField, Tooltip("GO DouDou")] public Doudou m_doudou;
    
    [SerializeField, Tooltip("Distance de détéction du bebs")]
    public float m_distanceDetection;

    [NonSerialized] public bool m_chasing;

    [NonSerialized] public NavMeshPath m_path;

    [NonSerialized] public float m_targetSpeed;
    
    [NonSerialized] public float m_pourcentSpeed = 1f;

    public float m_basicIaSpeed = 5f;
    public float m_speedAfterSpawn = 1f;
    
    public AnimationCurve m_courbeLimace;
    
    [SerializeField]
    private FMODUnity.EventReference m_fmodEventConstant;
    
    private FMOD.Studio.EventInstance m_fmodInstanceConstant;
    
    [SerializeField]
    private FMODUnity.EventReference m_fmodEventDrag;

    [NonSerialized]
    public FMOD.Studio.EventInstance m_fmodInstanceDrag;
    
    [SerializeField]
    private FMODUnity.EventReference m_fmodEventContinuous;
    
    private FMOD.Studio.EventInstance m_fmodInstanceContinuous;
    
    [SerializeField]
    private FMODUnity.EventReference m_fmodEventSonBB;
    
    private FMOD.Studio.EventInstance m_fmodInstanceSonBB;
    
    private void Awake()
    {
        m_targetSpeed = m_navAgent.speed;
        m_path = new NavMeshPath();
        if (m_player.m_doudouIsPossessed == true)
        {
            m_target = m_player.gameObject;
        }
        else
        {
            m_target = m_doudou.gameObject;
        }
        m_patrouilleState = new Patrouille(this,m_navAgent,m_waypoints,m_target);
        m_chasseState = new Chasse(this,m_navAgent,m_target);
        
        m_fmodInstanceContinuous = FMODUnity.RuntimeManager.CreateInstance(m_fmodEventContinuous);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(m_fmodInstanceContinuous,  GetComponent<Transform>());
        m_fmodInstanceContinuous.start();
        
        m_fmodInstanceConstant = FMODUnity.RuntimeManager.CreateInstance(m_fmodEventConstant);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(m_fmodInstanceConstant,  GetComponent<Transform>());
        m_fmodInstanceConstant.start();
        
        m_fmodInstanceSonBB = FMODUnity.RuntimeManager.CreateInstance(m_fmodEventSonBB);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(m_fmodInstanceSonBB,  GetComponent<Transform>());
        m_fmodInstanceSonBB.start();
        m_fmodInstanceSonBB.release();
        StartCoroutine(SonBB());
        
        m_fmodInstanceDrag = FMODUnity.RuntimeManager.CreateInstance(m_fmodEventDrag);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(m_fmodInstanceDrag,  GetComponent<Transform>());
        m_fmodInstanceDrag.start();
        m_fmodInstanceDrag.release();
        //Debug.Log($"Démarage du son de drag : {m_fmodInstanceDrag.start()}");
    }
    void OnEnable()
    {
        Awake();
        m_triggeredEvent.onTriggered += HandleTriggerEvent;
    }

    private void OnDisable()
    {
        m_triggeredEvent.onTriggered -= HandleTriggerEvent;
    }

    private void HandleTriggerEvent(Vector3 p_position)
    {
        Debug.Log("Ok, Je suis triggered");
    }

    private IEnumerator SonBB()
    {
        yield return new WaitForSeconds(3+Random.Range(0,7));
        m_fmodInstanceSonBB.start();
        StartCoroutine(SonBB());
        Debug.Log("Je fais le bb");
    }
    
    protected override BaseState GetInitialState()
    {
        return m_patrouilleState;
    }

   
}
