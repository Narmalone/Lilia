using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using FMODUnity;
using FMOD.Studio;

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
    [SerializeField] private GameObject m_FinalCollider;
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
    
    private FirstPersonOcclusion m_occlusion;
    
    [SerializeField]
    private Transform m_soundPlace;
    
    [SerializeField]
    private Transform m_soundLumiere;

    public Renderer m_thisRend;

    [SerializeField]
    private EventReference m_fmodEventAmpoule;
    
    //[SerializeField]
    //private EventReference m_fmodEventChair;

    private void Awake()
    {
        m_occlusion = FindObjectOfType<FirstPersonOcclusion>();
        m_index = 0;
        m_indexSolution = 0;
        m_indexNotSolution = 0;
        m_indexPaper = 0;
        completeSolution = false;
        notSolutionComplete = false;
        m_FinalCollider.SetActive(false);
        m_gameManager = FindObjectOfType<GameManager>();
        foreach(GameObject p_obj in m_toActive)
        {
            p_obj.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if ((m_playerMask.value & (1 << other.gameObject.layer)) > 0)
        {
            if (m_player.isLeftHandFull == false && m_player.isRightHandFull == false)
            {
                m_uiManager.TakableObject();
                isTrigger = true;
                m_thisRend.material.SetFloat("_BooleanFloat", 1f);
            }
            else
            {
                m_uiManager.DropSomethingBefore();
                m_uiManager.AnimUi();
            }
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
            m_player.NoVelocity();
            if (isTrigger == true)
            {
                SwitchSelect();
                m_uiManager.DisableUi();
                m_player.inCompteur = true;
                m_player.transform.position = Vector3.MoveTowards(m_player.transform.position, m_containerPlayer.transform.position, 0.1f);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if ((m_playerMask.value & (1 << other.gameObject.layer)) > 0)
        {
            if (m_gameManager.isPc == true)
            {
                isTrigger = false;
                m_uiManager.DisableUi();
                m_thisRend.material.SetFloat("_BooleanFloat", 0f);
            }
        }
    }
    public void LockedPlayer()
    {
        if (isLocked == false)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                m_thisRend.material.SetFloat("_BooleanFloat", 0f);
                m_player.NoVelocity();
                isLocked = true;
                m_gameManager.canDrop = false;
                m_txtCancelAction.gameObject.SetActive(true);
                foreach (GameObject p_obj in m_toActive)
                {
                    p_obj.SetActive(true);
                }
            }
        }
        else
        {
            StartCoroutine(canUnlock());
            Select();
        }


    }
    IEnumerator canUnlock()
    {
        yield return new WaitForSeconds(0.2f);
        UnlockPlayer();
    }
    public void UnlockPlayer()
    {
        StopCoroutine(canUnlock());
        if(m_gameManager.isPc == true)
        {
            if(isLocked == true)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    m_player.inCompteur = false;
                    m_gameManager.canDrop = true;
                    isLocked = false;
                    isTrigger = false;
                    m_player.m_speed = 1.5f;
                    m_txtCancelAction.gameObject.SetActive(false);
                    m_thisRend.material.SetFloat("_BooleanFloat", 0f);
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
            m_txtCancelAction.gameObject.SetActive(false);
            isLocked = false;
            m_player.inCompteur = false;
            m_gameManager.canDrop = true;
            isTrigger = false;
            m_thisRend.material.SetFloat("_BooleanFloat", 0f);
            m_player.m_speed = 1.5f;
            m_FinalCollider.SetActive(true);
            m_appear.LateGameAppear();
            gameObject.layer = default;
            enabled = false;
            //EventInstance m_fmodInstance = RuntimeManager.CreateInstance(m_fmodEventChair.Guid);
            //RuntimeManager.AttachInstanceToGameObject(m_fmodInstance, m_soundPlace);
            //m_fmodInstance.start();
            //m_occlusion.AddInstance(m_fmodInstance);
            //m_fmodInstance.release();
            EventInstance m_fmodInstance = RuntimeManager.CreateInstance(m_fmodEventAmpoule.Guid);
            RuntimeManager.AttachInstanceToGameObject(m_fmodInstance, m_soundLumiere);
            m_fmodInstance.start();
            m_occlusion.AddInstance(m_fmodInstance);
            m_fmodInstance.release();
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
