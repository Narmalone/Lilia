using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneDetection : MonoBehaviour
{
    [SerializeField] private Event1 m_triggeredEvent;

    [SerializeField] private LayerMask m_layerPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if ((m_layerPlayer.value & (1 << other.gameObject.layer)) > 0)
        {
            m_triggeredEvent?.Raise(other.transform.position);
            Debug.Log("un event a été trigger ?");
        }
    }
   
}
