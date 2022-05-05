using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{

    [SerializeField] private LayerMask m_playerMask;
    [SerializeField] private MenuManager m_menuManager;
    private void Awake()
    {
       if(m_menuManager == null)
        {
            m_menuManager = FindObjectOfType<MenuManager>();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((m_playerMask.value & (1 << other.gameObject.layer)) > 0)
        {
            Debug.Log("GameOver");
            m_menuManager.OnDeath();
        }
    }
}
