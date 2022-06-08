using System.Collections;
using System.Collections.Generic;
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
    private void OnTriggerStay(Collider other)
    {
        if ((m_playerMask.value & (1 << other.gameObject.layer)) > 0)
        {
            if(m_player.m_doudouIsPossessed == true)
            {
                m_uiManager.TakableObject();
                if (Input.GetKeyDown(KeyCode.E))
                {
                    m_animGiveDoudou.gameObject.SetActive(true);
                    m_walkhands.SetActive(false);
                    StartCoroutine(StartCredits());
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

    IEnumerator StartCredits()
    {
        m_playerAnimator.SetTrigger("GiveElDoudou");
        m_player.isCinematic = true;
        m_uiManager.DisableUi();
        yield return new WaitForSeconds(5f);
        m_gameManager.PlayerNotIngame();
        m_menuManager.OnCredits();
    }
}
