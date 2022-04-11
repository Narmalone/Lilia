using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AISM : StateMachine
{
    [HideInInspector]
    public Patrouille m_patrouilleState;
    [HideInInspector]
    public Chasse m_chasseState;
    
    [SerializeField, Tooltip("L'agent de navigation AI")] private NavMeshAgent m_navAgent;
    [SerializeField, Tooltip("Reference script waypoints")] private Waypoints m_waypoints;
    [SerializeField, Tooltip("Instance du doudou")] private Doudou m_doudou;
    [SerializeField, Tooltip("Event détécté")] private Event1 m_triggeredEvent;
    
    [SerializeField, Tooltip("Distance de détéction du bebs")]
    public float m_distanceDetection;

    [NonSerialized] public bool m_chasing;

    [NonSerialized] public NavMeshPath m_path;

    private BaseState m_currentState;

    private void Awake()
    {
        m_path = new NavMeshPath();
        m_patrouilleState = new Patrouille(this,m_navAgent,m_waypoints,m_doudou);
        m_chasseState = new Chasse(this,m_navAgent,m_doudou);
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
    

    protected override BaseState GetInitialState()
    {
        return m_patrouilleState;
    }
}
