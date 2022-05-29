using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{

    [SerializeField] private LayerMask m_playerMask;
    [SerializeField] private LayerMask m_doudouMask;
    [SerializeField] private LayerMask m_portillonMask;
    [SerializeField] private MenuManager m_menuManager;
    [SerializeField] private GameManager m_gameManager;
    [SerializeField] RespawnMe m_respawn;
    private Collider m_currentPortillon;
    private void Awake()
    {
       if(m_menuManager == null)
        {
            m_menuManager = FindObjectOfType<MenuManager>();
        }
        if (m_gameManager == null)
        {
            m_gameManager = FindObjectOfType<GameManager>();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((m_portillonMask.value & (1 << other.gameObject.layer)) > 0)
        {
            other.gameObject.GetComponent<Doors>().OnComplete();
            Debug.Log(other.name);
        }
        if ((m_playerMask.value & (1 << other.gameObject.layer)) > 0)
        {
            Debug.Log("GameOver");
            m_menuManager.OnDeath();
            m_gameManager.isDead = true;
            m_respawn.makeRespawn = true;
        }
        if ((m_doudouMask.value & (1 << other.gameObject.layer)) > 0)
        {
            m_menuManager.OnDeath();
            m_gameManager.isDead = true;
            m_respawn.makeRespawn = true;
        }

    }
}
