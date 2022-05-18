using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CadenasOpenScript : MonoBehaviour
{
    [SerializeField] private Animator m_animKey;
    [SerializeField] private Animator m_animDoor;
    [SerializeField] private Animator m_animCadenas;

    string m_name = "isOpen";

    [SerializeField] private GameManager m_gameManager;
    [SerializeField] private LayerMask m_layerPlayer;
    private void Awake()
    {
        if(m_gameManager == null)
        {
            m_gameManager = FindObjectOfType<GameManager>();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((m_layerPlayer.value & (1 << other.gameObject.layer)) > 0)
        {
            if (m_gameManager.gotKey == true)
            {
                m_animCadenas.SetBool(m_name, true);
                m_animKey.SetBool(m_name, true);
                m_animDoor.SetBool(m_name, true);
                Animator.StringToHash(m_name);
            }
        }

           
    }
}
