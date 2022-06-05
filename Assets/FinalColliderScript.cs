using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalColliderScript : MonoBehaviour
{

    [SerializeField] private Animator m_doorToClose;
    [SerializeField] private LayerMask m_playerMask;
    [SerializeField] FinalScript m_final;

    private void Awake()
    {
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(m_final.canFinal == true)
        {
            if ((m_playerMask.value & (1 << other.gameObject.layer)) > 0)
            {
                m_doorToClose.SetTrigger("isFinalClose");
                gameObject.SetActive(false);
            }
        }
        
    }
}
