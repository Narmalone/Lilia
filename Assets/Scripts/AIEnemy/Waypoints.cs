using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    private int index = 0;
    
    [SerializeField] private List<GameObject> m_listWaypoints = new List<GameObject>();
    [SerializeField] private GameObject m_waypointsPref;

    public void NextPoint()
    {
        index = (index+1) % m_listWaypoints.Count;
    }

    public GameObject GetCurrentPoint()
    {
        return m_listWaypoints[index];
    }

    public void CreateWaypoint()
    {
        Instantiate(m_waypointsPref);
    }
}
