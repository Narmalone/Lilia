using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FunRadio : MonoBehaviour
{
    [Header("References scripts")]
    [SerializeField] private GameManager m_gameManager;
    [SerializeField] private UiManager m_uiManager;
    [SerializeField] private CreateNarrativeEvent m_createNarrativeEvent;
    [SerializeField] private WaypointsEvent m_waypointMoveDoudou;
    [SerializeField] private Doudou m_doudou;
    [SerializeField] private AudioManagerScript m_audioScript;
    [SerializeField] private AppearThings m_appearsChamber;
    public bool isPlay = false;
    public bool isFirstAnswer = true;
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
        if(isFirstAnswer == true)
        {
            m_waypointMoveDoudou.isEventCalled = true;
            m_audioScript.Stop("PhoneEvent");
            m_audioScript.Play("HangUp");
            m_audioScript.PlayVoices("DialogPhone");
            m_appearsChamber.SwitchAppearing();
            m_doudou.TakeBeforeChase = true;
            Debug.Log("a répondu au téléphone");
            isFirstAnswer = false;
        }
        else { return; }
        
    }
}
