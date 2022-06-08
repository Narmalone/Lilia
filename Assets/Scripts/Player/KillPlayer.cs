using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class KillPlayer : MonoBehaviour
{

    [SerializeField] private LayerMask m_playerMask;
    [SerializeField] private LayerMask m_doudouMask;
    [SerializeField] private LayerMask m_portillonMask;
    [SerializeField] private MenuManager m_menuManager;
    [SerializeField] private GameManager m_gameManager;
    [SerializeField] RespawnMe m_respawn;
    [SerializeField] private FinalScript m_final;
    private Collider m_currentPortillon;

    [SerializeField] private GameObject m_screamerObj;
    [SerializeField] private Animator m_screamerBebe;
    [SerializeField] private StudioEventEmitter m_screamerSound;

    private void Awake()
    {
        m_screamerObj.SetActive(false);
       if (m_menuManager == null)
        {
            m_menuManager = FindObjectOfType<MenuManager>();
        }
        if (m_gameManager == null)
        {
            m_gameManager = FindObjectOfType<GameManager>();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((m_portillonMask.value & (1 << other.gameObject.layer)) > 0)
        {
            if(other.gameObject.GetComponent<Doors>().isDoor == false)
            {
                other.gameObject.GetComponent<Doors>().OnComplete();
                Debug.Log("ouvre le portillon");
            }
        }
        if(m_final.finalTriggered == false)
        {
            if ((m_playerMask.value & (1 << other.gameObject.layer)) > 0)
            {
                Debug.Log("GameOver");
                m_screamerObj.SetActive(true);
                m_screamerBebe.SetTrigger("Screamer");
                m_screamerSound.Play();
                m_menuManager.OnDeath();
                m_gameManager.isDead = true;
                m_respawn.makeRespawn = true;
                StartCoroutine(CorouBeforeDeath());
            }
            if ((m_doudouMask.value & (1 << other.gameObject.layer)) > 0)
            {
                m_screamerObj.SetActive(true);
                m_screamerBebe.SetTrigger("Screamer");
                m_screamerSound.Play();
                m_menuManager.OnDeath();
                m_gameManager.isDead = true;
                m_respawn.makeRespawn = true;
                StartCoroutine(CorouBeforeDeath());
            }
        }
    }

    IEnumerator CorouBeforeDeath()
    {
        yield return new WaitForSeconds(2.3f);
        m_screamerObj.SetActive(false);
        StopCoroutine(CorouBeforeDeath());
    }
}
