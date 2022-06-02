using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBackwardPortillon : MonoBehaviour
{
    [SerializeField, Tooltip("target == joueur")] private LayerMask m_target;
    [SerializeField] private Doors m_thisDoor;

    private void Awake()
    {
        m_thisDoor = GetComponentInParent<Doors>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((m_target.value & (1 << other.gameObject.layer)) > 0)
        {
            m_thisDoor.isLeftTrigger = true;
        }
    }
}
