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
    private float timeToWait = 1f;
    [SerializeField] private GameObject m_handsUi;
    private void Awake()
    {
        makeRespawn = true;
        m_gameManager = FindObjectOfType<GameManager>();
        index = 0;
        timeToWait = 1f;
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
            m_gameManager.isPc = false;
            m_iaTransform.position = m_iaCheckpoint.position;
            m_player.transform.position = m_playercheckpoints.transform.position;
            if (makeRespawn == true)
            {
                Debug.Log("lancer chrono");
                StartChrono();
            }
        }
    }
    public void StartChrono()
    {
        Debug.Log("dans chrono");
        if (timeToWait >= 0f)
        {
            timeToWait -= Time.deltaTime;
        }
        if(timeToWait <= 0f)
        {
            timeToWait = 0f;
            if (Input.anyKeyDown)
            {
                m_gameManager.isPc = true;
                Respawn();
                makeRespawn = false;
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
            m_player.isLeftHandFull = true;
        }

        if(m_player.m_flashlightIsPossessed == false)
        {
            m_uiManager.TakeLampe();
            m_flm.PickItem();
            m_player.m_flashlightIsPossessed = true;
            m_player.isRightHandFull = true;
        }

        m_menuManager.OnRespawn();
        m_gameManager.isDead = false;
        timeToWait = 1f;
        m_handsUi.SetActive(true);
    }
}
