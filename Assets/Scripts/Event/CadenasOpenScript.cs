using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class CadenasOpenScript : MonoBehaviour
{
    [SerializeField] private Animator m_animDoor;
    [SerializeField] private BoxCollider m_doorBox;
    [SerializeField] private PlayerController m_player;
    string m_name = "isOpen";
    [SerializeField] private StudioEventEmitter[] m_clip;
    [SerializeField] private GameManager m_gameManager;
    [SerializeField] private KeyScript m_key;
    [SerializeField] private LayerMask m_layerPlayer;
    [SerializeField] private Renderer m_CadenasRenderer;
    [SerializeField] private Transform m_Cadenas;
    [SerializeField] private UiManager m_uiManager;
    
    private bool hasOpenened;
    private void Awake()
    {
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
                m_uiManager.TakableObject();
                m_CadenasRenderer.material.SetFloat("_BooleanFloat", 1f);
            }
            else
            {
                m_clip[1].Play();
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if ((m_layerPlayer.value & (1 << other.gameObject.layer)) > 0)
        {
            if (m_gameManager.gotKey == true)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    m_doorBox.enabled = false;
                    m_uiManager.DisableUi();
                    m_player.m_stopStress = true;
                    StartCoroutine(OuverturePorte());
                    m_gameManager.gotKey = false;
                    m_key.m_keyUi.SetActive(false);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((m_layerPlayer.value & (1 << other.gameObject.layer)) > 0)
        {
            m_uiManager.DisableUi();
            m_CadenasRenderer.material.SetFloat("_BooleanFloat", 0f);
        }
    }

    private IEnumerator OuverturePorte()
    {
        yield return new WaitForSeconds(0.2f);
        m_CadenasRenderer.material.SetFloat("_BooleanFloat", 0f);
        m_clip[0].Play();
        m_animDoor.SetBool(m_name, true);
        m_Cadenas.position = new Vector3(0f, 0f, 200f);
    }
}
