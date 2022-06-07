using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

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

    [SerializeField] private Animator m_playerAnim;

    private FirstPersonOcclusion m_occlusion;

    public EventReference m_fmodDringDring;

    private EventInstance m_eventInstance;

    [SerializeField]
    private Transform m_soundPlace;

    [SerializeField] private BoxCollider m_chairBox;
    [SerializeField] private BoxCollider m_activeBebe;

    private void Awake()
    {
        m_occlusion = FindObjectOfType<FirstPersonOcclusion>();
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
            m_activeBebe.enabled = true;
            m_chairBox.enabled = false;
            m_player.m_stopStress = true;
            m_player.NoNeedStress();
            m_playerAnim.SetTrigger("Anwser");
            m_ragdoll.DisableRagdoll();
            m_waypointMoveDoudou.isEventCalled = true;
            m_player.m_doudouIsPossessed = false;
            m_player.isLeftHandFull = false;
            m_doudou.m_callEvent = true;
            m_doudou.TakeBeforeChase = true;
            m_eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            m_clips[0].Play();
            m_clips[1].Play();
            m_appearsChamber.SwitchAppearing();
            isFirstAnswer = false;
            m_uiManager.DisableUi();
            StartCoroutine(StopPhoneCinematic());
            //EventInstance m_fmodInstanceDrag = RuntimeManager.CreateInstance(m_appearsChamber.m_fmodEventTremblement.Guid);
            //RuntimeManager.AttachInstanceToGameObject(m_fmodInstanceDrag, m_soundPlace);
            //m_fmodInstanceDrag.start();
            //m_occlusion.AddInstance(m_fmodInstanceDrag);
            //m_fmodInstanceDrag.release();
            Debug.Log("a répondu au téléphone");
        }
        else { return; }
        
    }
    IEnumerator StopPhoneCinematic()
    {
        yield return new WaitForSeconds(7f);
        m_playerAnim.SetTrigger("Reset");
        m_player.isCinematic = false;
        m_player.m_speed = 1.5f;
        StopCoroutine(StopPhoneCinematic());
    }
    public void StartPhoneSound()
    {
        if(StartPhone == false)
        {
            if(m_player.hasChair == false)
            {
                m_chairBox.enabled = true;
            }
            m_eventInstance = RuntimeManager.CreateInstance(m_fmodDringDring.Guid);
            RuntimeManager.AttachInstanceToGameObject(m_eventInstance, m_soundPlace);
            m_eventInstance.start();
            m_occlusion.AddInstance(m_eventInstance);
            //m_clips[0].Play();
            StartPhone = true;
        }
    }
}
