using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointsEvent : MonoBehaviour
{
    public int m_waypointsIndex;

    [SerializeField, Tooltip("Vitesse à laquelle le doudou va entre chaques points par secondes")] public float m_speed = 1f;

    public List<GameObject> m_waypoints;

    [SerializeField] Doudou m_doudou;

    public bool isEventCalled;

    private void Awake()
    {
        m_waypointsIndex = 0;

        if(m_doudou == null)
        {
            m_doudou = FindObjectOfType<Doudou>();
        }
    }

    private void Update()
    {
        if(isEventCalled == true)
        {
            m_doudou.m_callEvent = true;
            m_doudou.transform.localPosition = Vector3.MoveTowards(m_doudou.transform.localPosition, m_waypoints[m_waypointsIndex].transform.position, m_speed * Time.deltaTime);
            if (m_doudou.transform.localPosition == m_waypoints[m_waypointsIndex].transform.localPosition)
            {
                MooveToNextPoint();
                Debug.Log("bouge au prochain point");
            }
        }
    }
    public void MooveToNextPoint()
    {
        m_waypointsIndex++;
        m_doudou.transform.localPosition = Vector3.MoveTowards(m_doudou.transform.localPosition, m_waypoints[m_waypointsIndex].transform.position, m_speed * Time.deltaTime);
    }
}
