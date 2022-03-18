using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public enum m_enumState { 
        PATROUILLE,
            CHASSE
    };

    [SerializeField] private NavMeshAgent m_navAgent;
    [SerializeField] private Waypoints m_waypoints;
    [SerializeField] private PlayerController m_player;

    private m_enumState m_currentState = m_enumState.PATROUILLE;

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
            m_navAgent.SetDestination(m_player.transform.position);
        }
    }
}
