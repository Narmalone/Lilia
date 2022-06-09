using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FMODUnity;
using FMOD.Studio;
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

    [SerializeField] private StudioEventEmitter m_eventEmitterCry;
    
    [SerializeField] private GameObject m_RunAway;
    [SerializeField]
    private EventReference m_fmodEventChair;
    [SerializeField]
    private Transform m_soundPlace;
    [SerializeField] private FirstPersonOcclusion m_occlusion;
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
            other.gameObject.GetComponentInChildren<MouseLock>().m_sound.EventInstance.setParameterByName("Parameter 1",0);
            m_RunAway.SetActive(false);
            m_gameManager.canPick = true;
            StartCoroutine(CorouBeforeSpawn());
        }
    }
    //dsds
    IEnumerator CorouBeforeSpawn()
    {
        yield return new WaitForSeconds(3f);
        m_RunAway.SetActive(false);
        EventInstance m_fmodInstance = RuntimeManager.CreateInstance(m_fmodEventChair.Guid);
        RuntimeManager.AttachInstanceToGameObject(m_fmodInstance, m_soundPlace);
        m_fmodInstance.start();
        m_occlusion.AddInstance(m_fmodInstance);
        m_fmodInstance.release();
        IaUnable.SetActive(false);
        IAEndAnim.SetActive(true);
        m_bebePls.SetTrigger("LastAnim");
        m_appear.SpawnAfterCloseDoor();
        m_DoorCanOpen.isActivable = true;
        m_final.canFinal = true;
        m_lastBoxDoor.SetActive(true);
        m_ColliderMobToGive.SetActive(true);
        m_eventEmitterCry.Play();
        m_occlusion.AddInstance(m_eventEmitterCry.EventInstance);
        StopCoroutine(CorouBeforeSpawn());
    }

}
