using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class CadenasOpenScript : MonoBehaviour
{
    [SerializeField] private Animator m_animKey;
    [SerializeField] private Animator m_animDoor;
    [SerializeField] private Animator m_animCadenas;
    [SerializeField] private FMODUnity.EventReference m_fmodEvent;

    private FMOD.Studio.EventInstance m_fmodInstance;

    string m_name = "isOpen";

    [SerializeField] private GameManager m_gameManager;
    [SerializeField] private LayerMask m_layerPlayer;
    private void Awake()
    {
        m_fmodInstance = FMODUnity.RuntimeManager.CreateInstance(m_fmodEvent);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(m_fmodInstance,  GetComponent<Transform>(), GetComponent<Rigidbody>());
        
        
        if(m_gameManager == null)
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
                m_animCadenas.SetBool(m_name, true);
                m_animKey.SetBool(m_name, true);
                
                Animator.StringToHash(m_name);
                StartCoroutine(OuverturePorte());
            }
        }      
    }

    private IEnumerator OuverturePorte()
    {
        yield return new WaitForSeconds(0.5f);
        m_animDoor.SetBool(m_name, true);
    }
}
