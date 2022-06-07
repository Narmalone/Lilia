using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBebeBerceau : MonoBehaviour
{
    //Bebe a remplacer a la fin
    [SerializeField] private LayerMask m_playerMask;
    [SerializeField] private Animator m_animBebe;
    [SerializeField] private GameObject IA;
    private void Awake()
    {
        GetComponent<BoxCollider>().enabled = false;
        m_animBebe.gameObject.SetActive(false);
        IA.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((m_playerMask.value & (1 << other.gameObject.layer)) > 0)
        {
            m_animBebe.gameObject.SetActive(true);
            m_animBebe.SetTrigger("LeaveBed");
            GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(MatchWithWalk());
        }
    }

    IEnumerator MatchWithWalk()
    {
        yield return new WaitForSeconds(4.8f);
        m_animBebe.gameObject.SetActive(false);
        IA.SetActive(true);
        StopCoroutine(MatchWithWalk());
    }
}
