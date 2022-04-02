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

    [SerializeField] private NavMeshAgent m_navAgent;
    [SerializeField] private Waypoints m_waypoints;
    [SerializeField] private Doudou m_doudou;
    
    [SerializeField, Tooltip("Distance de détéction du bebs")]
    public float m_distanceDetection = 5f;

    [NonSerialized]
    public bool m_chasing;

    [NonSerialized] public NavMeshPath m_path;

    private void Awake()
    {
        m_path = new NavMeshPath();
        m_patrouilleState = new Patrouille(this,m_navAgent,m_waypoints,m_doudou);
        m_chasseState = new Chasse(this,m_navAgent,m_doudou);
    }

    protected override BaseState GetInitialState()
    {
        return m_patrouilleState;
    }
}
