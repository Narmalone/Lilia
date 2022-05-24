using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Chasse : BaseState
{
    private AISM m_sm;

    private NavMeshAgent m_navAgent;
    private GameObject m_target;
    private NavMeshPath m_path;
    

    public Chasse(AISM p_stateMachine, NavMeshAgent p_navAgent, GameObject p_target) : base("Chasse", p_stateMachine)
    {
        m_navAgent = p_navAgent;
        m_target = p_target;
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

        if (m_sm.m_player.m_doudouIsPossessed == true)
        {
            if (ReferenceEquals(m_target, m_sm.m_player.gameObject) == false)
            {
                m_target = m_sm.m_player.gameObject;
            }
        }
        else
        {
            if (ReferenceEquals(m_target, m_sm.m_doudou.gameObject) == false)
            {
                m_target = m_sm.m_doudou.gameObject;
            }
        }
        
        m_sm.m_pourcentSpeed += 0.5f*Time.deltaTime;
        if (m_sm.m_pourcentSpeed >= 1)
        {
            m_sm.m_pourcentSpeed = 0;
            m_sm.m_fmodInstanceDrag = FMODUnity.RuntimeManager.CreateInstance(m_sm.m_fmodEventDrag.Guid);
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(m_sm.m_fmodInstanceDrag, m_sm.gameObject.transform);
            m_sm.m_fmodInstanceDrag.start();
            m_sm.m_occlusion.AddInstance(m_sm.m_fmodInstanceDrag);
            m_sm.m_fmodInstanceDrag.release();
            //FMODUnity.RuntimeManager.PlayOneShotAttached(m_sm.m_fmodEventDrag.Guid,  m_sm.gameObject);
        }

        m_navAgent.speed = Mathf.Lerp(0f,m_sm.m_targetSpeed*2f,m_sm.m_courbeLimace.Evaluate(m_sm.m_pourcentSpeed));
        
        m_navAgent.SetDestination(m_target.transform.position);
        GetPath(m_path, m_target.transform.position, m_sm.transform.position, NavMesh.AllAreas);
        if (m_path.status== NavMeshPathStatus.PathComplete)
        {
            if (GetPathLength(m_path) > m_sm.m_distanceDetection && m_sm.m_chasing == false)
            {
                stateMachine.ChangeState(m_sm.m_patrouilleState);
            }
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
