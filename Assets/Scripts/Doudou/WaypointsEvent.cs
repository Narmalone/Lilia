using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaypointsEvent : MonoBehaviour
{
    public int m_waypointsIndex;

    [SerializeField, Tooltip("Vitesse à laquelle le doudou va en mettre par secondes")] public float m_speed = 1f;

    [Tooltip("Au minimum un point de départ et 1 point d'arrêt, le dernier et avant-dernier point doivent être collés")]public List<GameObject> m_waypoints;

    [SerializeField] Doudou m_doudou;
    [SerializeField] private RagdollScript m_ragdoll;
    public bool isEventCalled;

    public Transform m_positionAfterCall;
    public bool setNewPos = false;

    private void Awake()
    {
        m_waypointsIndex = 0;
        setNewPos = false;
        if (m_doudou == null)
        {
            m_doudou = FindObjectOfType<Doudou>();
        }
    }

    private void Update()
    {
        if(isEventCalled == true)
        {
            if(setNewPos == false)
            {
                m_doudou.transform.position = m_positionAfterCall.transform.position;
                setNewPos = true;
            }
            else
            {
                m_doudou.m_callEvent = true;
                m_doudou.transform.localPosition = Vector3.MoveTowards(m_doudou.transform.localPosition, m_waypoints[m_waypointsIndex].transform.position, m_speed * Time.deltaTime);
                if (m_doudou.transform.localPosition == m_waypoints[m_waypointsIndex].transform.localPosition)
                {
                    MooveToNextPoint();
                }
            }
        }
        else { return; }
    }
    public void MooveToNextPoint()
    {
        if (m_waypointsIndex >= 0 && m_waypointsIndex < m_waypoints.Count -1)
        {
            if(isEventCalled == true)
            {
                m_waypointsIndex++;
                m_doudou.transform.localPosition = Vector3.MoveTowards(m_doudou.transform.localPosition, m_waypoints[m_waypointsIndex].transform.position, m_speed * Time.deltaTime);
                Debug.Log(m_waypointsIndex);
            }
        }
        if(m_waypointsIndex == m_waypoints.Count -1)
        {
            m_doudou.CallEventEnded();
            isEventCalled = false;
        }
    }
}
