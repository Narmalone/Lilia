using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;

public class AIController : MonoBehaviour
{
    public enum m_enumState { 
        PATROUILLE,
            CHASSE
    };

    private bool m_chasing = false;

    [SerializeField] private NavMeshAgent m_navAgent;
    [SerializeField] private Waypoints m_waypoints;
    [SerializeField] private Doudou m_doudou;

    [SerializeField, Tooltip("Distance de détéction du bebs")]
    private float m_distanceDetection = 5f;

    [NonSerialized]
    public NavMeshPath m_path;
    
    private m_enumState m_currentState = m_enumState.PATROUILLE;

    private void Awake()
    {
        m_path = new NavMeshPath();
    }

    public void FollowDoudou()
    {
        m_currentState = m_enumState.CHASSE;
        m_chasing = true;
    }

    public void UnfollowDoudou()
    {
        m_currentState = m_enumState.PATROUILLE;
        m_chasing = false;
    }
    
    private void Update()
    {
        if (m_currentState == m_enumState.PATROUILLE)
        {
            if (Vector3.Distance(gameObject.transform.position, m_waypoints.getCurrentPoint().transform.position) <= 5)
            {
                m_waypoints.nextPoint();
            }
            m_navAgent.SetDestination(m_waypoints.getCurrentPoint().transform.position);
        }
        else
        {
            m_navAgent.SetDestination(m_doudou.transform.position);
        }
        
        GetPath(m_path,m_doudou.transform.position, transform.position,NavMesh.AllAreas);
        if (GetPathLength(m_path) < m_distanceDetection)
        {
            m_currentState = m_enumState.CHASSE;
        }
        else
        {
            if (m_chasing == false)
            {
                m_currentState = m_enumState.PATROUILLE;
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
