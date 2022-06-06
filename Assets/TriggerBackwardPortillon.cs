using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBackwardPortillon : MonoBehaviour
{
    [SerializeField, Tooltip("target == joueur")] private LayerMask m_target;
    [SerializeField] private Doors m_thisDoor;

    [SerializeField] private LayerMask m_IA;
    [SerializeField] private PlayerScriptAnim m_pcAnim;
    private void Awake()
    {
        m_thisDoor = GetComponentInParent<Doors>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((m_target.value & (1 << other.gameObject.layer)) > 0)
        {
            m_pcAnim.canPlayAnim = true;
            m_thisDoor.isLeftTrigger = true;
        }  
        if ((m_IA.value & (1 << other.gameObject.layer)) > 0)
        {
            m_pcAnim.canPlayAnim = false;
            m_thisDoor.OnComplete();
            m_thisDoor.isLeftTrigger = true;
        }
    }
}
