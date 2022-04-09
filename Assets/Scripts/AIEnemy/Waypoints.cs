using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    private int index = 0;
    
    [SerializeField] private List<GameObject> m_listWaypoints = new List<GameObject>();

    public void nextPoint()
    {
        index = (index+1) % m_listWaypoints.Count;
    }

    public GameObject getCurrentPoint()
    {
        return m_listWaypoints[index];
    }
}
