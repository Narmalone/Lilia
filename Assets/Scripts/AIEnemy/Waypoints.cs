using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    private int index = 0;
    [SerializeField] FinalScript m_final;
    [SerializeField] private List<GameObject> m_listWaypoints = new List<GameObject>();
    [SerializeField] private List<GameObject> m_finalWaypoints = new List<GameObject>();
    [SerializeField] private GameObject m_waypointsPref;

    public void NextPoint()
    {
        if (m_final.finalTriggered == true)
        {
            if(index == m_finalWaypoints.Count-1)
            {
                Debug.Log("est au 3° point");
                m_final.OnPlace();
            }
            index = (index + 1) % m_finalWaypoints.Count;
        }
        else
        {
            index = (index + 1) % m_listWaypoints.Count;
        }
    }

    public GameObject GetCurrentPoint()
    {
        if (m_final.finalTriggered == true)
        {
            return m_finalWaypoints[index];
        }
        else
        {
            return m_listWaypoints[index];
        }
    }

    public void CreateWaypoint()
    {
        Instantiate(m_waypointsPref);
    }
}
