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
    private FMODUnity.EventReference m_fmodEventRespiration;
    
    private EventInstance m_fmodInstanceRespiration;
    
    public FMODUnity.EventReference m_fmodEventDrag;

    public EventInstance m_fmodInstanceDrag;

    [SerializeField]
    private FMODUnity.EventReference m_fmodEventSonBB;

    public FirstPersonOcclusion m_occlusion;

    private Vector3 m_previousPos;
    
    private void Awake()
    {
        m_occlusion = FindObjectOfType<FirstPersonOcclusion>();
        Debug.Log("Awake");
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
        
        //m_fmodInstanceContinuous.start();
        
        m_fmodInstanceRespiration = FMODUnity.RuntimeManager.CreateInstance(m_fmodEventRespiration);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(m_fmodInstanceRespiration,  GetComponent<Transform>());
        m_fmodInstanceRespiration.start();
        m_occlusion.AddInstance(m_fmodInstanceRespiration);

        StartCoroutine(SonBB());
        
    }

    private void Update()
    {
        base.Update();
        if(transform.position == m_previousPos)
        {
            m_fmodInstanceDrag.stop(STOP_MODE.ALLOWFADEOUT);
        }
        m_previousPos = transform.position;
    }

    void OnEnable()
    {
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
        while(true)
        {
            
            int time = 10 + Random.Range(0, 10);
            yield return new WaitForSeconds(time);
            m_fmodInstanceRespiration.stop(STOP_MODE.ALLOWFADEOUT);
            StartCoroutine(ReEnableRespiration());
            var instance = FMODUnity.RuntimeManager.CreateInstance(m_fmodEventSonBB.Guid);
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(instance, gameObject.transform);
            instance.start();
            m_occlusion.AddInstance(instance);
            instance.release();
        }
    }

    private IEnumerator ReEnableRespiration()
    {
        yield return new WaitForSeconds(7f);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(m_fmodInstanceRespiration,  GetComponent<Transform>());
        m_fmodInstanceRespiration.start();
        m_occlusion.AddInstance(m_fmodInstanceRespiration);
    }
    
    protected override BaseState GetInitialState()
    {
        return m_patrouilleState;
    }

    
   
}
