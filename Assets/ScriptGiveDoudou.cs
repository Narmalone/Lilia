using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptGiveDoudou : MonoBehaviour
{
    [SerializeField] private LayerMask m_playerMask;
    [SerializeField] private PlayerController m_player;
    [SerializeField] private UiManager m_uiManager;
    private void OnTriggerEnter(Collider other)
    {
        if ((m_playerMask.value & (1 << other.gameObject.layer)) > 0)
        {
            if(m_player.m_doudouIsPossessed == true)
            {
                m_uiManager.TakableDoudou();
                if (Input.GetMouseButtonDown(0))
                {
                    //Jouer l'animation de Maxime
                }
            }
        }

    }
}
