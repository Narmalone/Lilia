using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class ScriptGiveDoudou : MonoBehaviour
{
    //
    [SerializeField] private LayerMask m_playerMask;
    [SerializeField] private PlayerController m_player;
    [SerializeField] private UiManager m_uiManager;
    [SerializeField] private Animator m_animGiveDoudou;
    [SerializeField] private GameObject m_walkhands;
    [SerializeField] private MenuManager m_menuManager;
    [SerializeField] private GameManager m_gameManager;
    [SerializeField] private Animator m_playerAnimator;
    [SerializeField] private StudioEventEmitter m_eventEmitterCrying;
    [SerializeField] private StudioEventEmitter m_endVoice;
    [SerializeField] private AISM m_ia;
    private bool hasgiven = false;

    private void Awake()
    {
        hasgiven = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if ((m_playerMask.value & (1 << other.gameObject.layer)) > 0)
        {
            if(m_player.m_doudouIsPossessed == true)
            {
                if (hasgiven == false)
                {
                    m_uiManager.TakableObject();
                }
                else
                {
                    m_uiManager.DisableUi();
                }
               
                if (Input.GetKeyDown(KeyCode.E))
                {
                    m_animGiveDoudou.gameObject.SetActive(true);
                    m_walkhands.SetActive(false);
                    m_animGiveDoudou.SetTrigger("GiveDoudou");
                    m_playerAnimator.SetTrigger("GiveElDoudou");
                    m_eventEmitterCrying.StopInstance();
                    if (hasgiven == false)
                    {
                        StartCoroutine(StartCredits());
                        hasgiven = true;
                    }
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if ((m_playerMask.value & (1 << other.gameObject.layer)) > 0)
        {
            m_uiManager.DisableUi();
        }
    }
    //sd
    IEnumerator StartCredits()
    {
        m_player.isCinematic = true;
        m_uiManager.DisableUi();
        m_ia.canRespiration = false;
        m_ia.m_fmodInstanceRespiration.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        m_endVoice.Play();
        yield return new WaitForSeconds(5f);
        m_gameManager.PlayerNotIngame();
        m_menuManager.OnCredits();
    }
}
