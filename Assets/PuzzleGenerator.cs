using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
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
    [SerializeField] public paper m_paper;
    [SerializeField] public GameObject m_currentSelected;
    [NonSerialized] public GameObject m_lastObjSelected;

    [SerializeField] AppearThings m_appear;

    [SerializeField] private TextMeshProUGUI m_txtCancelAction;
    [SerializeField] private List<GameObject> m_toActive;
    private int m_index = 0;
    private int m_indexSolution = 0;
    private int m_indexNotSolution = 0;
    private int m_indexPaper = 0;

    public bool isLocked = false;
    public bool isTrigger = false;

    public bool completeSolution = false;
    public bool notSolutionComplete = false;

    [SerializeField]private Material m_myMat;

    Renderer rend;

    private void Awake()
    {
        m_index = 0;
        m_indexSolution = 0;
        m_indexNotSolution = 0;
        m_indexPaper = 0;
        completeSolution = false;
        notSolutionComplete = false;
        m_gameManager = FindObjectOfType<GameManager>();
        foreach(GameObject p_obj in m_toActive)
        {
            p_obj.SetActive(false);
        }
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
    private void Update()
    {
        if(isTrigger == true)
        {
            LockedPlayer();
        }
    }
    public void LateUpdate()
    {
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
                m_txtCancelAction.gameObject.SetActive(true);
                foreach (GameObject p_obj in m_toActive)
                {
                    p_obj.SetActive(true);
                }
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
                    m_txtCancelAction.gameObject.SetActive(false);
                    foreach (GameObject p_obj in m_toActive)
                    {
                        p_obj.SetActive(false);
                    }
                }
            }
        }
    }

    public void Select()
    {
        if (m_currentSelected != null)
        {
            m_lastObjSelected = m_currentSelected;
        }

        m_lastObjSelected = m_currentSelected;
        m_currentSelected = m_interruptersList[m_index];

        m_buttons[m_index].OnSelected();
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
        NextIndicePaper();

        if (m_getSolution[0].isActivated == true && m_getSolution[1].isActivated == true && m_getSolution[2].isActivated == true && m_getSolution[3].isActivated == true && m_getSolution[4].isActivated == true)
        {
            
            completeSolution = true;
            Debug.Log("les 3 sont actifs");
        }
        else
        {
            completeSolution = false;
        }

        if (m_notSolution[0].isActivated == false && m_notSolution[1].isActivated == false && m_notSolution[2].isActivated == false && m_notSolution[3].isActivated == false && m_notSolution[4].isActivated == false)
        {
            if (m_notSolution[5].isActivated == false && m_notSolution[6].isActivated == false && m_notSolution[7].isActivated == false && m_notSolution[8].isActivated == false)
            {
                notSolutionComplete = true;
            }
            else
            {
                notSolutionComplete = false;
            }
        }
        else
        {
            notSolutionComplete = false;
        }

        if (completeSolution == true && notSolutionComplete == true)
        {
            Debug.Log("puzzle ended");
            isLocked = false;
            isTrigger = false;
            m_player.m_speed = 2f;
            m_appear.LateGameAppear();
        }       
    }
    public void NextIndicePaper()
    {
        if(m_indexPaper < m_getSolution.Count - 1)
        {
            if (m_getSolution[m_indexPaper].isActivated == true)
            {
                m_paper.NextDisplayContent();
                m_indexPaper++;
                Debug.Log("dans la condition");
            }
        }
    }
}
