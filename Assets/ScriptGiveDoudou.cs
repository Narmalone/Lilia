using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptGiveDoudou : MonoBehaviour
{
    [SerializeField] private LayerMask m_playerMask;
    [SerializeField] private PlayerController m_player;
    [SerializeField] private UiManager m_uiManager;
    [SerializeField] private Animator m_animGiveDoudou;
    [SerializeField] private GameObject m_flashRend;
    [SerializeField] private MenuManager m_menuManager;
  
    private void OnTriggerEnter(Collider other)
    {
        if ((m_playerMask.value & (1 << other.gameObject.layer)) > 0)
        {
            if(m_player.m_doudouIsPossessed == true)
            {
                m_uiManager.TakableDoudou();
                if (Input.GetMouseButtonDown(0))
                {
                    m_animGiveDoudou.gameObject.SetActive(true);
                    m_player.NoNeedStress();
                    m_player.NoVelocity();
                    m_player.isCinematic = true;
                    m_animGiveDoudou.SetTrigger("GiveDoudou");
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
        yield return new WaitForSeconds(5f);
        m_menuManager.OnCredits();
    }
}
