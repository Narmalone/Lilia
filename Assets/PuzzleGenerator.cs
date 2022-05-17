using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PuzzleGenerator : MonoBehaviour
{
    [SerializeField] PlayerController m_player;
    [SerializeField] Transform m_containerPlayer;
    [SerializeField] private GameManager m_gameManager;
    [SerializeField] private UiManager m_uiManager;
    [SerializeField] float m_rangeToNotOut = 0.3f;
    [SerializeField] private LayerMask m_playerMask;
 
    [SerializeField] public List<GameObject> m_interruptersList;
    [SerializeField] public List<ButtonGeneratorPuzzle> m_getSolution;
    [SerializeField] public List<ButtonGeneratorPuzzle> m_notSolution;
    [SerializeField] public List<ButtonGeneratorPuzzle> m_buttons;

    [SerializeField] public GameObject m_currentSelected;
    [NonSerialized] public GameObject m_lastObjSelected;
    [SerializeField] private Material m_objMat;
    [SerializeField] private Color m_selectedColor;
    [SerializeField] private Color m_notSelectedColor;

    private int m_index = 0;
    private int m_indexSolution = 0;
    private int m_indexNotSolution = 0;

    public bool isLocked = false;
    public bool isTrigger = false;

    public bool completeSolution = false;
    public bool notSolutionComplete = false;

    private void Awake()
    {
        m_index = 0;
        m_indexSolution = 0;
        m_indexNotSolution = 0;
        completeSolution = false;
        notSolutionComplete = false;
        m_gameManager = FindObjectOfType<GameManager>();
        m_objMat.color = m_notSelectedColor;
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
        
        
        if (isLocked == true)
        {
            
            if (isTrigger == true)
            {
                SwitchSelect();
                m_uiManager.DisableUi();
                if (Vector3.Distance(m_player.transform.position, m_containerPlayer.transform.position) > m_rangeToNotOut)
                {
                    m_player.transform.position = Vector3.MoveTowards(m_player.transform.position, m_containerPlayer.transform.position, 0.1f);
                    m_player.m_speed = 0f;
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
                    m_player.m_speed = 2f;
                }
            }
        }
    }

    public void Select()
    {
        if (m_currentSelected != null)
        {
            m_lastObjSelected = m_currentSelected;
            m_lastObjSelected.GetComponent<Renderer>().material.color = m_notSelectedColor;
        }
        m_currentSelected = m_interruptersList[m_index];
        m_currentSelected.GetComponent<Renderer>().material.color = m_selectedColor;
        m_buttons[m_index].OnSelected();
        CheckSolution();
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
                }
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                m_index--;
                if(m_index < 0)
                {
                    m_index = 0;
                }
                Select();
            }
        }
    }

    public void CheckSolution()
    {
        //index solution base 0
        if (m_getSolution[m_indexSolution].isActivated == true)
        {
            m_indexSolution++;
            completeSolution = true;
            Debug.Log(m_getSolution[m_getSolution.Count -1]);
        }
        else
        {
            completeSolution = false;
        } 
        
        if(m_notSolution[m_notSolution.Capacity - 1].isActivated == false)
        {
            notSolutionComplete = true;
            Debug.Log("tous les boutons sont désactivés");
        }
        else
        {
            notSolutionComplete = false;
            Debug.Log("il y'en a au - 1 qui est activé");
        }
        
        
       if(completeSolution == true && notSolutionComplete == true)
        {
            Debug.Log("puzzle ended");
        }
       
    }
    public void ResetNotSolution()
    {
        m_indexNotSolution = 0;
    }
}
