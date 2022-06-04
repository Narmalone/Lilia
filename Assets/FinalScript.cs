using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalScript : MonoBehaviour
{
    [SerializeField] private GameObject IA;
    [SerializeField] private GameObject m_newWaypoint;
    [SerializeField] private PlayerController m_player;
    [SerializeField] private LayerMask m_playMask;
    [SerializeField] private GameObject m_lastPoint;
    [SerializeField] private GameObject m_TrigDoorActivate;
    public bool finalTriggered = false;
    public bool MobInPlace = false;

    private void Awake()
    {
        finalTriggered = false;
        MobInPlace = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(finalTriggered == false)
        {
            if ((m_playMask.value & (1 << other.gameObject.layer)) > 0)
            {
                finalTriggered = true;
                TriggerEnd();
                Debug.Log("lancer fonction Trigger end");
            }
        }
    }
    public void TriggerEnd()
    {
        if(finalTriggered == true)
        {
            m_player.m_speed = 0f;
            m_player.NoNeedStress();
            IA.transform.position = m_newWaypoint.transform.position;
            Debug.Log("le joueur ne peut plus bouger");
        }
    }
    public void OnPlace()
    {
        MobInPlace = true;
        if(MobInPlace == true)
        {
            m_player.m_speed = 1.5f;
            m_TrigDoorActivate.SetActive(true);
        }
    }

    private void Update()
    {
        if(MobInPlace == true)
        {
            IA.transform.position = m_lastPoint.transform.position;
        }
    }
}
