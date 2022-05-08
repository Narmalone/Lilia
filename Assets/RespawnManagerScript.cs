using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManagerScript : MonoBehaviour
{
    [SerializeField, Tooltip("Objet qui doit respawn")] public Transform m_toRespawn;
    [SerializeField, Tooltip("Spawner d'objets")] public List<Transform> m_respawnPoint;
    private int index;

    public bool isNextCheckpoint = false;
    [SerializeField] private GameManager m_gameManager;

    private void Awake()
    {
        m_gameManager = FindObjectOfType<GameManager>();
        index = 0;
        if(m_toRespawn == null) { return; }
    }
    private void Update()
    {
       
        
    }
    private void LateUpdate()
    {
        if (m_gameManager.isDead == true)
        {
            Respawn();
        }
    }
    public void NextCheckpoint()
    {
        if (isNextCheckpoint == true)
        {
            index++;
        }
    }
    public void Respawn()
    {
        m_toRespawn.transform.position = m_respawnPoint[index].transform.position;
        m_gameManager.isDead = false;
        Debug.Log(m_toRespawn.transform.position);
        
    }
}
