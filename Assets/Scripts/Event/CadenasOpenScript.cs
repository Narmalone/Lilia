using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class CadenasOpenScript : MonoBehaviour
{
    [SerializeField] private Animator m_animDoor;
    [SerializeField] private Animator m_animCadenas;
    [SerializeField] private BoxCollider m_doorBox;
    [SerializeField] private FMODUnity.EventReference m_fmodEvent;

    private FMOD.Studio.EventInstance m_fmodInstance;

    string m_name = "isOpen";

    [SerializeField] private GameManager m_gameManager;
    [SerializeField] private KeyScript m_key;
    [SerializeField] private LayerMask m_layerPlayer;
    private void Awake()
    {
        m_fmodInstance = FMODUnity.RuntimeManager.CreateInstance(m_fmodEvent);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(m_fmodInstance,  GetComponent<Transform>(), GetComponent<Rigidbody>());

        m_doorBox.enabled = true;
        if (m_gameManager == null)
        {
            m_gameManager = FindObjectOfType<GameManager>();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((m_layerPlayer.value & (1 << other.gameObject.layer)) > 0)
        {
            if (m_gameManager.gotKey == true)
            {
                m_fmodInstance.start();
                m_doorBox.enabled = false;
                m_animCadenas.SetBool(m_name, true);
                StartCoroutine(OuverturePorte());
                m_gameManager.gotKey = false;
                m_key.m_keyUi.SetActive(false);
                m_key.gameObject.transform.position = new Vector3(0f, 0f, 200f);
            }
        }      
    }

    private IEnumerator OuverturePorte()
    {
        yield return new WaitForSeconds(0.5f);
        m_animDoor.SetBool(m_name, true);
    }
}
