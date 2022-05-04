using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrouille : BaseState
{
    private AISM m_sm;

    private NavMeshAgent m_navAgent;
    private Waypoints m_waypoints;
    private Doudou m_doudou;
    private NavMeshPath m_path;
    
    public Patrouille(AISM p_stateMachine, NavMeshAgent p_navAgent, Waypoints p_waypoints, Doudou p_doudou) : base("Patrouille", p_stateMachine)
    {
        m_navAgent = p_navAgent;
        m_waypoints = p_waypoints;
        m_doudou = p_doudou;
        m_sm = p_stateMachine;
        m_path = m_sm.m_path;
    }
    
    public override void Enter()
    {
        base.Enter();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        
        if (Vector3.Distance(m_sm.transform.position, m_waypoints.GetCurrentPoint().transform.position) <= 1)
        {
            m_waypoints.NextPoint();
        }
        
        m_sm.m_pourcentSpeed += 1f*Time.deltaTime;
        if (m_sm.m_pourcentSpeed >= 1f)
        {
            m_sm.m_pourcentSpeed = 0;
        }

        m_navAgent.speed = Mathf.Lerp(0f,m_sm.m_targetSpeed*2f,m_sm.m_courbeLimace.Evaluate(m_sm.m_pourcentSpeed));
        
        m_navAgent.SetDestination(m_waypoints.GetCurrentPoint().transform.position);
        Chasse.GetPath(m_path, m_doudou.transform.position, m_sm.transform.position, NavMesh.AllAreas);
        if (m_path.status == NavMeshPathStatus.PathComplete)
        {
            if (Chasse.GetPathLength(m_path) < m_sm.m_distanceDetection)
            {
                stateMachine.ChangeState(m_sm.m_chasseState);
                Debug.Log("change state");
            }
        }
        
        if (m_sm.m_chasing == true)
        {
            stateMachine.ChangeState(m_sm.m_chasseState);
        }
    }
    
    
}
