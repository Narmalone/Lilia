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
    [SerializeField] private PlayerController m_player;
    [SerializeField] private AudioManagerScript m_audioScript;
    [SerializeField] private AppearThings m_appearsChamber;
    [SerializeField] RagdollScript m_ragdoll;
    public bool isPlay = false;
    public bool isFirstAnswer = true;
    public bool StartPhone = false;
    [Header("References Mask"), Space(10)]
    [SerializeField] private LayerMask m_playerMask;
    [SerializeField, Tooltip("0 = phone ring, 1 = décrocher, 2 dialogue")] private StudioEventEmitter[] m_clips;


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
            m_player.m_myAnim.SetBool("PhoneAnswering", true);
            m_ragdoll.DisableRagdoll();
            m_waypointMoveDoudou.isEventCalled = true;
            m_player.m_doudouIsPossessed = false;
            m_player.isLeftHandFull = false;
            m_doudou.m_callEvent = true;
            m_doudou.TakeBeforeChase = true;
            m_clips[0].Stop();
            m_clips[1].Play();
            m_clips[2].Play();
            m_appearsChamber.SwitchAppearing();
            isFirstAnswer = false;
            m_uiManager.DisableUi();
        }
        else { return; }
        
    }
    public void StartPhoneSound()
    {
        if(StartPhone == false)
        {
            m_clips[0].Play();
            StartPhone = true;
        }
    }
}
