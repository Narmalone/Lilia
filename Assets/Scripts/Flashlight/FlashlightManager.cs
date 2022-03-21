using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightManager : MonoBehaviour
{
    [SerializeField]UiManager uiManager;

    [SerializeField, Tooltip("Référence de la torche")]private GameObject m_flashlight;
    [SerializeField] private Transform m_flashlightContainer;

    [SerializeField] private GameObject m_spotlightOfFlashlight;

    [SerializeField] private LayerMask m_playerMask;

    [SerializeField] private GameManager m_gameManager;
    private void Awake()
    {
        m_rbodyFlashlight = m_flashlight.GetComponent<Rigidbody>();
        m_spotlightOfFlashlight.GetComponent<Light>();
        m_gameManager = FindObjectOfType<GameManager>();
    }

    [SerializeField]PlayerController playerController;
    private void OnTriggerStay(Collider p_collide)
    {
        if (m_gameManager.isPc)
        {
            if ((m_playerMask.value & (1 << p_collide.gameObject.layer)) > 0)
            {

                if (playerController.m_flashlightIsPossessed == false)
                {
                    playerController.TakeFlashlight();
                    uiManager.takeObject();
                }
                else if (playerController.m_flashlightIsPossessed == true)
                {
                    uiManager.DisableUi();
                }
            }
        }
        else if (m_gameManager.isGamepad)
        {
            if ((m_playerMask.value & (1 << p_collide.gameObject.layer)) > 0)
            {

                if (playerController.m_flashlightIsPossessed == false)
                {
                    playerController.TakeFlashlight();
                    uiManager.takeObject();
                    Debug.Log("gamepad ui activée");
                }
                else if (playerController.m_flashlightIsPossessed == true)
                {
                    uiManager.DisableUi();
                }
            }
        }
    }
    private void OnTriggerExit(Collider p_collide)
    {
            if ((m_playerMask.value & (1 << p_collide.gameObject.layer)) > 0)
            {
                uiManager.DisableUi();
            }
    }

    [SerializeField] private Rigidbody m_rbodyFlashlight;

    public void PickItem()
    {
        if (m_gameManager.isPc)
        {
            m_rbodyFlashlight.useGravity = false;
            m_rbodyFlashlight.isKinematic = true;

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
