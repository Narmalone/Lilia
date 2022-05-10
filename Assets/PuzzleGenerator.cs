using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGenerator : MonoBehaviour
{
    [SerializeField] PlayerController m_player;
    [SerializeField] Transform m_containerPlayer;
    [SerializeField] private GameManager m_gameManager;
    [SerializeField] private UiManager m_uiManager;
    [SerializeField] float m_rangeToNotOut = 0.3f;
    [SerializeField] private LayerMask m_playerMask;

    [SerializeField] public List<GameObject> m_interruptersList;
    [SerializeField] public GameObject m_currentSelected;
    private int m_index;

    public bool isLocked = false;

    private void Awake()
    {
        m_index = 0;
        m_gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((m_playerMask.value & (1 << other.gameObject.layer)) > 0)
        {
            if(m_gameManager.isPc == true)
            {
                if(isLocked == false)
                {
                    m_uiManager.TakableObject();
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        LockPlayer();
                    }
                }
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if ((m_playerMask.value & (1 << other.gameObject.layer)) > 0)
        {
            if (m_gameManager.isPc == true)
            {
                m_uiManager.DisableUi();
            }
        }
    }
    public void LockPlayer()
    {
        m_player.transform.position = m_containerPlayer.transform.position;
        if (Vector3.Distance(m_player.transform.position, m_containerPlayer.transform.position) > m_rangeToNotOut)
        {
            m_player.transform.position = m_containerPlayer.transform.position;
        }
        isLocked = true;
        Select();
    }
    public void UnlockPlayer()
    {
        if(m_gameManager.isPc == true)
        {
            if(isLocked == true)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    isLocked = false;
                }
            }
        }
    }

    public void Select()
    {
        m_currentSelected = m_interruptersList[m_index];
        foreach(GameObject p_obj in m_interruptersList)
        {
            
        }
    }

    public void SwitchSelect()
    {
        if(m_gameManager.isPc == true)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                m_index++;
                Select();
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                m_index--;
                Select();
            }
        }
    }
}
