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
    public bool makeRespawn = true;
    public bool isNextCheckpoint = false;
    [SerializeField] private GameManager m_gameManager;

    private void Awake()
    {
        makeRespawn = true;
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
        //if (Input.GetKeyDown(KeyCode.T))
        //{
            //if (makeRespawn == true)
            //{
                //Respawn();
            //}
        //}  
        if (m_gameManager.isDead == true)
        {
            if (Input.anyKeyDown)
            {
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
        m_player.transform.position = m_playercheckpoints.transform.position;

        m_iaTransform.position = m_iaCheckpoint.position;

        if(m_player.m_doudouIsPossessed == false)
        {
            m_doudoutransform.transform.position = m_doudoucheckpoint.transform.position;
            Debug.Log("joueur a pas le doudou donc tp");
        }

        if(m_player.m_flashlightIsPossessed == false)
        {
            m_veilleuseTransform.position = m_veilleuseCheckPoint.position;
        }

        m_menuManager.OnRespawn();
        m_gameManager.isDead = false;
    }
}
