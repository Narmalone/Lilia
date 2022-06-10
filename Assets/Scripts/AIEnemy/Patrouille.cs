using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

public class Patrouille : BaseState
{
    private AISM m_sm;

    private NavMeshAgent m_navAgent;
    private Waypoints m_waypoints;
    private GameObject m_target;
    private NavMeshPath m_path;
    
    public Patrouille(AISM p_stateMachine, NavMeshAgent p_navAgent, Waypoints p_waypoints, GameObject p_target) : base("Patrouille", p_stateMachine)
    {
        m_navAgent = p_navAgent;
        m_waypoints = p_waypoints;
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
        
        if(m_sm.m_final.CallMobNewWaypoint == false)
        {
            m_sm.m_bebeAnimator.SetTrigger("Walk");
            
            Chasse.GetPath(m_path, m_target.transform.position, m_sm.transform.position, NavMesh.AllAreas);
            if (m_path.status == NavMeshPathStatus.PathComplete)
            {
                if (Chasse.GetPathLength(m_path) < m_sm.m_distanceDetection)
                {
                    m_navAgent.isStopped = false;
                    stateMachine.ChangeState(m_sm.m_chasseState);
                    m_sm.m_bebeAnimator.SetTrigger("Run");
                    m_sm.m_mouselock.m_sound.EventInstance.setParameterByName("Parameter 1",1);
                    Debug.Log("change state");
                }
            }
        
            if (m_sm.m_chasing == true)
            {
                stateMachine.ChangeState(m_sm.m_chasseState);
            }
        }


       
        if (Vector3.Distance(m_sm.transform.position, m_waypoints.GetCurrentPoint().transform.position) <= 1)
        {
            if (UnityEngine.Random.Range(0, 2) == 0)
            {
                m_sm.m_bebeAnimator.Update(1);
                m_waypoints.NextPoint();
            }
            else
            {
                if (m_sm.m_final.CallMobNewWaypoint == true)
                {
                    m_sm.StopCoroutine((StopMovement()));
                }
                else
                {
                    m_sm.m_bebeAnimator.Update(0f);
                    m_sm.StartCoroutine(StopMovement());   
                }
            }
        }
        m_sm.m_pourcentSpeed += 0.5f*Time.deltaTime;
        if (m_sm.m_pourcentSpeed >= 1f)
        {
            m_sm.m_pourcentSpeed = 0;
            m_sm.m_fmodInstanceDrag = FMODUnity.RuntimeManager.CreateInstance(m_sm.m_fmodEventDrag.Guid);
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(m_sm.m_fmodInstanceDrag, m_sm.gameObject.transform);
            m_sm.m_fmodInstanceDrag.start();
            m_sm.m_occlusion.AddInstance(m_sm.m_fmodInstanceDrag);
            m_sm.m_fmodInstanceDrag.release();
            //FMODUnity.RuntimeManager.PlayOneShotAttached(m_sm.m_fmodEventDrag.Guid,  m_sm.gameObject);
        }

        m_navAgent.speed = Mathf.Lerp(0f,m_sm.m_targetSpeed,m_sm.m_courbeLimace.Evaluate(m_sm.m_pourcentSpeed));
        
        m_navAgent.SetDestination(m_waypoints.GetCurrentPoint().transform.position);
       
    }

    
    private IEnumerator StopMovement()
    {
        m_navAgent.isStopped = true;
        yield return new WaitForSeconds(5f);
        m_navAgent.isStopped = false;
        m_waypoints.NextPoint();
    }
    
}
