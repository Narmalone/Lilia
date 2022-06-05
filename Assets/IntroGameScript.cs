using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroGameScript : MonoBehaviour
{
    [SerializeField] private PlayerController m_player;

    private void Awake()
    {
        m_player.isCinematic = true;
    }
}
