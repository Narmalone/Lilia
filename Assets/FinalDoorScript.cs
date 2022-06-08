using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FinalDoorScript : MonoBehaviour
{
    [SerializeField] private LayerMask m_playerMask;
    [SerializeField] private Animator m_EndDoorAnim;
    [SerializeField] private AppearThings m_appear;
    [SerializeField] private Doudou m_doudou;
    [SerializeField] private Transform m_midWaypoint;
    [SerializeField] private BoxCollider m_thisBox;
    [SerializeField] private GameObject m_lastBoxDoor;
    [SerializeField] private Doors m_DoorCanOpen;
    [SerializeField] private FinalScript m_final;
    [SerializeField] private RagdollScript m_ragdoll;
    [SerializeField] private GameObject DoorFinal;
    [SerializeField] private GameObject IaUnable;
    [SerializeField] private GameObject IAEndAnim;
    [SerializeField] private GameManager m_gameManager;
    [SerializeField] private Animator m_bebePls;
    [SerializeField] private GameObject m_ColliderMobToGive;

    [SerializeField] private TextMeshProUGUI m_RunAway;
    private void Awake()
    {
        gameObject.SetActive(false);
        m_DoorCanOpen.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((m_playerMask.value & (1 << other.gameObject.layer)) > 0)
        {

            m_doudou.transform.position = m_midWaypoint.position;
            m_ragdoll.ActivateRagdoll();
            m_final.MobFinalPosition = true;
            m_EndDoorAnim.SetTrigger("isFinalClose");
            DoorFinal.layer = 13;
            m_thisBox.enabled = false;
            m_RunAway.gameObject.SetActive(false);
            m_gameManager.canPick = true;
            StartCoroutine(CorouBeforeSpawn());
        }
    }
    //dsds
    IEnumerator CorouBeforeSpawn()
    {
        yield return new WaitForSeconds(3f);
        IaUnable.SetActive(false);
        IAEndAnim.SetActive(true);
        m_bebePls.SetTrigger("LastAnim");
        m_appear.SpawnAfterCloseDoor();
        m_DoorCanOpen.isActivable = true;
        m_final.canFinal = true;
        m_lastBoxDoor.SetActive(true);
        m_ColliderMobToGive.SetActive(true);
        StopCoroutine(CorouBeforeSpawn());
    }

}
