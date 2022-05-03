using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunRadio : MonoBehaviour
{
    [Header("References scripts")]
    [SerializeField] private GameManager m_gameManager;
    [SerializeField] private UiManager m_uiManager;
    [SerializeField] private CreateNarrativeEvent m_createNarrativeEvent;
    
    [Header("References Mask"), Space(10)]
    [SerializeField] private LayerMask m_playerMask;

    private void Awake()
    {
        if (m_gameManager == null)
        {
            m_gameManager = FindObjectOfType<GameManager>();
        }
        if (m_uiManager == null)
        {
            m_uiManager = FindObjectOfType<UiManager>();
        }
    }

    public void AnswerToCall()
    {
        Debug.Log("a répondu au téléphone");
    }
}
