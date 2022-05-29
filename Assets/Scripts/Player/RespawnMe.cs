using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnMe : MonoBehaviour
{
    [SerializeField] PlayerController m_player;
    [SerializeField] Doudou m_doudou;
    [SerializeField] FlashlightManager m_flm;
    [SerializeField] UiManager m_uiManager;
    [Header("Objects Transform"), Space(10)]
    [SerializeField] public Transform m_playertransform;
    [SerializeField] public Transform m_iaTransform;

    [Header("Checkpoints_1"), Space(10)]
    [SerializeField] public Transform m_playercheckpoints;
    [SerializeField] public Transform m_iaCheckpoint;

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
        if (m_gameManager.isDead == true)
        {
            if (Input.anyKeyDown)
            {
                m_iaTransform.position = m_iaCheckpoint.position;
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

        if(m_player.m_doudouIsPossessed == false)
        {
            m_uiManager.TakeDoudou();
            m_doudou.PickItem();
            m_player.m_doudouIsPossessed = true;
            Debug.Log("joueur a pas le doudou donc l'attribuer au joueur");
        }

        if(m_player.m_flashlightIsPossessed == false)
        {
            m_uiManager.TakeLampe();
            m_flm.PickItem();
            m_player.m_flashlightIsPossessed = true;
            //m_veilleuseTransform.position = m_veilleuseCheckPoint.position;
            Debug.Log("joueur a pas la veilleuse donc l'attribuer au joueur");
        }

        m_menuManager.OnRespawn();
        m_gameManager.isDead = false;
    }
}
