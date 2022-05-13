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
    private GameObject m_lastObjSelected;
    [SerializeField] private Color m_selectedColor;
    [SerializeField] private Color m_notSelectedColor;

    private int m_index;

    public bool isLocked = false;
    public bool isTrigger = false;

    private void Awake()
    {
        m_index = 0;
        m_gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        m_uiManager.TakableObject();
    }
    private void OnTriggerStay(Collider other)
    {
        if ((m_playerMask.value & (1 << other.gameObject.layer)) > 0)
        {
            isTrigger = true;
        }
    }
    public void LateUpdate()
    {
        LockedPlayer();
        if (isLocked == true && isTrigger == true)
        {
            m_uiManager.DisableUi();
            if (Vector3.Distance(m_player.transform.position, m_containerPlayer.transform.position) > m_rangeToNotOut)
            {
                m_player.transform.position = Vector3.MoveTowards(m_player.transform.position, m_containerPlayer.transform.position, 0.1f);
                m_player.m_speed = 0f;
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
    public void LockedPlayer()
    {
        if (isLocked == false)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                isLocked = true;
            }
        }
        else
        {
            UnlockPlayer();
            Select();
        }


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
                    isTrigger = false;
                }
            }
        }
    }

    public void Select()
    {
        m_currentSelected = m_interruptersList[m_index];
        
        if(m_currentSelected != null)
        {
            m_lastObjSelected = m_currentSelected;
            m_currentSelected.GetComponent<Renderer>().material.color = m_selectedColor;
        }
        SwitchSelect();

    }

    public void SwitchSelect()
    {
        if(m_gameManager.isPc == true)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                if(m_index < m_interruptersList.Count-1)
                {
                    m_index++;
                    Select();
                    Debug.Log(m_index);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                m_index--;
                if(m_index <= 0)
                {
                    m_index = 0;
                }
                Select();
            }
        }
    }
}
