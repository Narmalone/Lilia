using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnMe : MonoBehaviour
{
    [SerializeField] PlayerController m_player;
    [Header("Objects Transform"), Space(10)]
    [SerializeField] public Transform m_doudoutransform;
    [SerializeField] public Transform m_playertransform;
    [SerializeField] public Transform m_iaTransform;
    [SerializeField] public Transform m_veilleuseTransform;

    [Header("Checkpoints_1"), Space(10)]
    [SerializeField] public Transform m_playercheckpoints;
    [SerializeField] public Transform m_doudoucheckpoint;
    [SerializeField] public Transform m_iaCheckpoint;
    [SerializeField] public Transform m_veilleuseCheckPoint;

    [SerializeField] private MenuManager m_menuManager;

    private int index;

    public bool isNextCheckpoint = false;
    [SerializeField] private GameManager m_gameManager;

    private void Awake()
    {
        m_gameManager = FindObjectOfType<GameManager>();
        index = 0;
    }
    private void OnEnable()
    {
        Awake();
    }
    private void Update()
    {
       
    }
    private void LateUpdate()
    {
        if (m_gameManager.isDead == true)
        {
            if (Input.anyKeyDown)
            {
                m_player.transform.position = m_playercheckpoints.transform.position;
                Respawn();
                Debug.Log("mort et appuie sur une touche");
            }
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
        m_iaTransform.position = m_iaCheckpoint.position;

        if (m_player.m_doudouIsPossessed == true)
        {
            m_doudoutransform.SetParent(null);
        }
        if(m_player.m_flashlightIsPossessed == true)
        {
            m_veilleuseTransform.SetParent(null);
        }
        m_doudoutransform.transform.position = m_doudoucheckpoint.transform.position;
        m_veilleuseTransform.position = m_veilleuseCheckPoint.position;
        Debug.Log("fonction respawn");

        m_menuManager.OnRespawn();
        m_gameManager.isDead = false;
    }
    public void RespawnPlayer()
    {
        m_player.transform.position = m_playercheckpoints.transform.position;
    }
}
