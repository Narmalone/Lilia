using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScriptAnim : MonoBehaviour
{
    [SerializeField] private PlayerController m_player;
    [SerializeField] private Animator m_playerAnims;
    [SerializeField] private Animator m_playerOpening;

    //DoudouWalkOnly, LampeWalkOnly, Doudou&LampWalk, Default s

    private void Awake()
    {
        m_playerAnims.gameObject.SetActive(false);
        m_playerOpening.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (m_player.move.x == 0)
        {
            m_playerAnims.speed = 0;
        }
        else
        {
            m_playerAnims.speed = 1;
            if (m_player.m_doudouIsPossessed == false && m_player.m_flashlightIsPossessed == false)
            {
                m_playerAnims.SetTrigger("Default");
                m_playerAnims.gameObject.SetActive(false);
            }
            else if (m_player.m_doudouIsPossessed == true && m_player.m_flashlightIsPossessed == false)
            {
                m_playerAnims.gameObject.SetActive(true);
                m_playerAnims.SetTrigger("DoudouWalkOnly");
            }
            else if (m_player.m_doudouIsPossessed == false && m_player.m_flashlightIsPossessed == true)
            {
                m_playerAnims.gameObject.SetActive(true);
                m_playerAnims.SetTrigger("LampeWalkOnly");
            }
            else if (m_player.m_doudouIsPossessed == true && m_player.m_flashlightIsPossessed == true)
            {
                m_playerAnims.SetTrigger("Doudou&LampWalk");
            }
        }
       
    }

    //PushLeftPortillon, PushRightPortillon, PushArmoire, Default
    public void PlayAnimPlayerToPortillons()
    {
        m_playerOpening.gameObject.SetActive(true);
        if(m_player.m_doudouIsPossessed == false && m_player.m_flashlightIsPossessed == false)
        {
            m_playerOpening.SetTrigger("PushLeftPortillon");
        }
        else if(m_player.m_doudouIsPossessed == true && m_player.m_flashlightIsPossessed == false)
        {
            m_playerOpening.SetTrigger("PushRightPortillon");
        } 
        else if(m_player.m_flashlightIsPossessed == true && m_player.m_doudouIsPossessed == false)
        {
            m_playerOpening.SetTrigger("PushLeftPortillon");
        }
        StartCoroutine(VisibleHands());
    }

    IEnumerator VisibleHands()
    {
        yield return new WaitForSeconds(2f);
        m_playerOpening.gameObject.SetActive(false);
        StopCoroutine(VisibleHands());
    }
}
