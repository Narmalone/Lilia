using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScriptAnim : MonoBehaviour
{
    [SerializeField] private PlayerController m_player;
    [SerializeField] private GameObject m_walksHands;
    [SerializeField] private GameObject m_pushands;
    [SerializeField] private Animator m_playerAnims;
    [SerializeField] private Animator m_playerOpening;
    public bool canPlayAnim = true;
    //DoudouWalkOnly, LampeWalkOnly, Doudou&LampWalk, Default s

    private void Awake()
    {
        m_playerAnims.SetTrigger("Default");
        m_playerAnims.gameObject.SetActive(false);
        m_playerOpening.gameObject.SetActive(false);
        canPlayAnim = true;
    }
    private void Update()
    {
        if (m_player.move.x == 0)
        {
            m_playerAnims.speed = 0.1f;
            if (m_player.m_doudouIsPossessed == false && m_player.m_flashlightIsPossessed == false)
            {
                m_playerAnims.SetTrigger("Default");
                m_walksHands.SetActive(false);
            }
            if (m_player.m_doudouIsPossessed == true && m_player.m_flashlightIsPossessed == false)
            {
                m_walksHands.SetActive(true);
                m_playerAnims.SetTrigger("DoudouWalkOnly");
            }
            if (m_player.m_doudouIsPossessed == false && m_player.m_flashlightIsPossessed == true)
            {
                m_walksHands.SetActive(true);
                m_playerAnims.SetTrigger("LampeWalkOnly");
            }
            if (m_player.m_doudouIsPossessed == true && m_player.m_flashlightIsPossessed == true)
            {
                m_playerAnims.SetTrigger("Doudou&LampWalk");
            }
           
        }
        else
        {
            m_playerAnims.speed = 1;
          
            if (m_player.m_doudouIsPossessed == true && m_player.m_flashlightIsPossessed == false)
            {
                m_walksHands.SetActive(true);
                m_playerAnims.SetTrigger("DoudouWalkOnly");
            }
            if (m_player.m_doudouIsPossessed == false && m_player.m_flashlightIsPossessed == true)
            {
                m_walksHands.SetActive(true);
                m_playerAnims.SetTrigger("LampeWalkOnly");
            }
            if (m_player.m_doudouIsPossessed == true && m_player.m_flashlightIsPossessed == true)
            {
                m_playerAnims.SetTrigger("Doudou&LampWalk");
            }
            if (m_player.m_doudouIsPossessed == false && m_player.m_flashlightIsPossessed == false)
            {
                m_playerAnims.SetTrigger("Default");
                m_walksHands.SetActive(false);
            }
        }
       
    }

    //PushLeftPortillon, PushRightPortillon, PushArmoire, Default
    public void PlayAnimPlayerToPortillons()
    {
        if(canPlayAnim == true)
        {
            m_playerOpening.gameObject.SetActive(true);
            if (m_player.isLeftHandFull == false && m_player.isRightHandFull == false)
            {
                m_playerOpening.SetTrigger("PushLeftPortillon");
            }
            else if (m_player.isLeftHandFull == true && m_player.isRightHandFull == false)
            {
                m_playerOpening.SetTrigger("PushRightPortillon");
            }
            else if (m_player.isRightHandFull == true && m_player.isLeftHandFull == false)
            {
                m_playerOpening.SetTrigger("PushLeftPortillon");
            }
            StartCoroutine(VisibleHands());
        }
    }

    IEnumerator VisibleHands()
    {
        yield return new WaitForSeconds(2f);
        m_pushands.SetActive(false);
        StopCoroutine(VisibleHands());
    }
    
    IEnumerator PushHands()
    {
        yield return new WaitForSeconds(3f);
        m_pushands.SetActive(false);
        StopCoroutine(PushHands());
    }

    public void PlayAnimToArmoire()
    {
        m_pushands.SetActive(true);
        m_playerOpening.SetTrigger("PushArmoire");
        StartCoroutine(PushHands());
    }
}
