using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScriptAnim : MonoBehaviour
{
    [SerializeField] private PlayerController m_player;
    [SerializeField] private Animator m_playerAnims;


    //DoudouWalkOnly, LampeWalkOnly, Doudou&LampWalk, Default s
    private void Update()
    {
        if (m_player.m_velocity.x == 0)
        {
            m_playerAnims.speed = 0;
        }
        else
        {
            Debug.Log("en train de bouger");
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
}
