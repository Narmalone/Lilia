using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Chasse : BaseState
{
    private AISM m_sm;

    private NavMeshAgent m_navAgent;
    private Doudou m_doudou;
    private NavMeshPath m_path;
    

    public Chasse(AISM p_stateMachine, NavMeshAgent p_navAgent, Doudou p_doudou) : base("Chasse", p_stateMachine)
    {
        m_navAgent = p_navAgent;
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

        m_navAgent.SetDestination(m_doudou.transform.position);

        GetPath(m_path, m_doudou.transform.position, m_sm.transform.position, NavMesh.AllAreas);
        if (GetPathLength(m_path) > m_sm.m_distanceDetection)
        {
            stateMachine.ChangeState(m_sm.m_patrouilleState);
        }
        if (m_sm.m_chasing == false)
        {
            stateMachine.ChangeState(m_sm.m_patrouilleState);
        }
    }
    public static bool GetPath( NavMeshPath path, Vector3 fromPos, Vector3 toPos, int passableMask )
    {
        path.ClearCorners();
       
        if ( NavMesh.CalculatePath( fromPos, toPos, passableMask, path ) == false )
            return false;
       
        return true;
    }
       
    public static float GetPathLength( NavMeshPath path )
    {
        float lng = 0.0f;
       
        if (( path.status != NavMeshPathStatus.PathInvalid ) && ( path.corners.Length > 1 ))
        {
            for ( int i = 1; i < path.corners.Length; ++i )
            {
                lng += Vector3.Distance( path.corners[i-1], path.corners[i] );
            }
        }
       
        return lng;
    }
}
