using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGenerator : MonoBehaviour
{
    [SerializeField] PlayerController m_player;
    [SerializeField] Transform m_containerPlayer;
    [SerializeField] float m_rangeToNotOut = 0.3f;
    
    public void LockPlayer()
    {
        if(Vector3.Distance(m_player.transform.position, m_containerPlayer.transform.position) > m_rangeToNotOut)
        m_player.transform.position = m_containerPlayer.transform.position;
    }
}
