using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDoorScript : MonoBehaviour
{
    [SerializeField] private LayerMask m_playerMask;
    [SerializeField] private Animator m_EndDoorAnim;
    [SerializeField] private AppearThings m_appear;
    [SerializeField] private Doudou m_doudou;
    [SerializeField] private Transform m_midWaypoint;
    private void Awake()
    {
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((m_playerMask.value & (1 << other.gameObject.layer)) > 0)
        {
            m_EndDoorAnim.SetTrigger("isFinalClose");
            StartCoroutine(CorouBeforeSpawn());
        }
    }

    IEnumerator CorouBeforeSpawn()
    {
        yield return new WaitForSeconds(0.7f);
        m_doudou.transform.position = m_midWaypoint.position;
        m_appear.SpawnAfterCloseDoor();
        gameObject.SetActive(false);
    }
}
