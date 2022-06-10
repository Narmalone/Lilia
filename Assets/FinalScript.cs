using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

public class FinalScript : MonoBehaviour
{
    [SerializeField] private GameObject IA;
    [SerializeField] private GameObject m_newWaypoint;
    [SerializeField] private PlayerController m_player;
    [SerializeField] private LayerMask m_playMask;
    [SerializeField] private GameObject m_lastPoint;
    [SerializeField] private GameObject m_TrigDoorActivate;
    [SerializeField] private Doudou m_doudou;
    [SerializeField] private UiManager m_uiManager;
    [SerializeField] private GameManager m_gameManager;
    [SerializeField, Tooltip("IA")] private GameObject m_finalPosition;
    [SerializeField, Tooltip("PlayerDansCabanne")] private Transform m_finalPlayerPosition;
    [SerializeField] private Transform m_cabanneDoudouTransform;
    [SerializeField] private Animator m_playerAnimator;
    [SerializeField] private RagdollScript m_ragdoll;

    [SerializeField] private GameObject m_RunAway;
    public bool canFinal = false;
    public bool CallMobNewWaypoint = false;
    public bool finalTriggered = false;
    public bool MobInPlace = false;
    public bool MobFinalPosition = false;

    private void Awake()
    {
        m_RunAway.gameObject.SetActive(false);
        finalTriggered = false;
        CallMobNewWaypoint = false;
        MobInPlace = false;
        canFinal = false;
        MobFinalPosition = false;
    }
    public void PlayAnim()
    {
        m_player.isCinematic = true;
        m_player.transform.position = m_finalPlayerPosition.position;
        m_player.transform.rotation = m_finalPlayerPosition.localRotation;
        m_playerAnimator.SetTrigger("InCabanne");
        StartCoroutine(CorourtineAnim());
    }
    IEnumerator CorourtineAnim()
    {
        yield return new WaitForSeconds(2f);
        m_gameManager.canPick = false;
        m_doudou.DropItem();
        m_player.m_doudouIsPossessed = false;
        m_player.isLeftHandFull = false;
        m_uiManager.DropDoudou();
        m_ragdoll.DisableRagdoll();
        m_playerAnimator.enabled = false;
        m_doudou.transform.position = m_cabanneDoudouTransform.position;
        m_player.GetComponentInChildren<MouseLock>().m_sound.EventInstance.setParameterByName("Parameter 1",1);
        yield return new WaitForSeconds(2f);
        m_player.isCinematic = false;
        CallMobNewWaypoint = true;
        TriggerEnd();
        StopCoroutine(CorourtineAnim());
    }
    private void OnTriggerEnter(Collider other)
    {
        if(finalTriggered == false)
        {
            if ((m_playMask.value & (1 << other.gameObject.layer)) > 0)
            {
                finalTriggered = true;
                PlayAnim();
                Debug.Log("lancer fonction Trigger end");
            }
        }
    }
    public void TriggerEnd()
    {
        if(finalTriggered == true)
        {
            m_player.NoVelocity();
            //IA.transform.position = m_newWaypoint.transform.position;
            IA.GetComponent<NavMeshAgent>().Warp(m_newWaypoint.transform.position);

        }
    }
    public void OnPlace()
    {
        MobInPlace = true;
        if(MobInPlace == true)
        {
            m_RunAway.SetActive(true);
            m_player.m_speed = 1.5f;
            m_TrigDoorActivate.SetActive(true);
        }
    }

    private void Update()
    {
        if(MobFinalPosition == false)
        {
            if (MobInPlace == true)
            {
                IA.GetComponent<NavMeshAgent>().Warp(m_lastPoint.transform.position);
                //IA.transform.position = m_lastPoint.transform.position;
            }
        }
        else
        {
            IA.GetComponent<NavMeshAgent>().Warp(m_finalPosition.transform.position);
            //IA.transform.position = m_finalPosition.transform.position;
        }
       
    }
}
