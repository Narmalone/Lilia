using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFacingPortillon : MonoBehaviour
{
    [SerializeField, Tooltip("target == joueur")] private LayerMask m_target;
    [SerializeField, Tooltip("target == IA mask")] private LayerMask m_IA;
    public bool isMobActivated = false;
    [SerializeField] private Doors m_thisDoor;

    [SerializeField] private PlayerScriptAnim m_pcAnim;
    private void Awake()
    {
        m_thisDoor = GetComponentInParent<Doors>();
        if(m_pcAnim == null)
        {
            m_pcAnim = FindObjectOfType<PlayerScriptAnim>();
        }
        m_pcAnim.canPlayAnim = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((m_target.value & (1 << other.gameObject.layer)) > 0)
        {
            m_pcAnim.canPlayAnim = true;
            m_thisDoor.isLeftTrigger = false;
        }

        if ((m_IA.value & (1 << other.gameObject.layer)) > 0)
        {
            isMobActivated = true;
            m_thisDoor.OnComplete();
            m_thisDoor.isLeftTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((m_target.value & (1 << other.gameObject.layer)) > 0)
        {
            m_pcAnim.canPlayAnim = false;
        }
        if ((m_IA.value & (1 << other.gameObject.layer)) > 0)
        {
            isMobActivated = false;
        }
    }
}
