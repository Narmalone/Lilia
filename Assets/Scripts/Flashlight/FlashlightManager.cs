using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightManager : MonoBehaviour
{
    [SerializeField]UiManager m_uiManager;

    [SerializeField, Tooltip("Rï¿½fï¿½rence de la torche")]private GameObject flashlight;
    [SerializeField] private Transform FlashlightContainer;
    [SerializeField, Tooltip("Référence de la torche")]private GameObject m_flashlight;

    [SerializeField, Tooltip("Mettre le transform du conteneur du joueur FlashlightContainer")] private Transform m_flashlightContainer;

    [SerializeField] private GameObject m_spotlightOfFlashlight;

    [SerializeField] private LayerMask m_playerMask;

    [SerializeField] private GameManager m_gameManager;
    private void Awake()
    {
        m_rbodyFlashlight = m_flashlight.GetComponent<Rigidbody>();
        m_spotlightOfFlashlight.GetComponent<Light>();
        m_gameManager = FindObjectOfType<GameManager>();
    }

    [SerializeField]PlayerController m_playerController;
    private void OnTriggerStay(Collider p_collide)
    {
        uiManager.TakableObject();
        playerController.TakeFlashlight();
        if (m_gameManager.isPc)
        {
            if ((m_playerMask.value & (1 << p_collide.gameObject.layer)) > 0)
            {

                if (m_playerController.m_flashlightIsPossessed == false)
                {
                    m_playerController.TakeFlashlight();
                    m_uiManager.takeObject();
                }
                else if (m_playerController.m_flashlightIsPossessed == true)
                {
                    m_uiManager.DisableUi();
                }
            }
        }
        else if (m_gameManager.isGamepad)
        {
            if ((m_playerMask.value & (1 << p_collide.gameObject.layer)) > 0)
            {

                if (m_playerController.m_flashlightIsPossessed == false)
                {
                    m_playerController.TakeFlashlight();
                    m_uiManager.takeObject();
                    Debug.Log("gamepad ui activée");
                }
                else if (m_playerController.m_flashlightIsPossessed == true)
                {
                    m_uiManager.DisableUi();
                }
            }
        }
    }
    private void OnTriggerExit(Collider p_collide)
    {
            if ((m_playerMask.value & (1 << p_collide.gameObject.layer)) > 0)
            {
                m_uiManager.DisableUi();
            }
    }

    [SerializeField] private Rigidbody m_rbodyFlashlight;

    public void PickItem()
    {
        if (m_gameManager.isPc)
        {
            m_rbodyFlashlight.useGravity = false;
            m_rbodyFlashlight.isKinematic = true;

            //trouver un moyen d'orienter l'objet en fonction de lï¿½ ou on regarde
            //m_yRotation = Mathf.Clamp(m_yRotation, -90f, 90f);
            //transform.LookAt(FlashlightContainer, Vector3.left);
            Debug.Log(FlashlightContainer.transform);
            flashlight.GetComponent<BoxCollider>().enabled = false;
            uiManager.DisableUi();
            m_flashlight.transform.SetParent(m_flashlightContainer);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(90f, 180f, 0f);
        }
        else if (m_gameManager.isGamepad)
        {
            m_rbodyFlashlight.useGravity = false;
            m_rbodyFlashlight.isKinematic = true;

            m_flashlight.transform.SetParent(m_flashlightContainer);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(90f, 180f, 0f);
        }
        
    }

    public void DropItem()
    {
        if (m_gameManager.isPc)
        {
            m_rbodyFlashlight.isKinematic = false;
            m_rbodyFlashlight.useGravity = true;


            m_flashlight.transform.parent = null;
        }
        else if (m_gameManager.isGamepad)
        {
            m_rbodyFlashlight.isKinematic = false;
            m_rbodyFlashlight.useGravity = true;
            m_flashlight.transform.parent = null;
        }

    }

}
