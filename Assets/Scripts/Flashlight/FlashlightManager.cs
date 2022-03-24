using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightManager : MonoBehaviour
{

    //----------------------------------------------- Références de classes ------------------------------------------//

    [SerializeField]UiManager m_uiManager;
    [SerializeField] PlayerController m_playerController;
    [SerializeField] private GameManager m_gameManager;
    [SerializeField] private LayerMask m_playerMask;

    //----------------------------------------------- Par rapport à la veilleuse ------------------------------------------//

    [SerializeField, Tooltip("Référence de la torche")]private GameObject m_flashlight;

    [SerializeField, Tooltip("Mettre le transform du conteneur du joueur FlashlightContainer")] private Transform m_flashlightContainer;

    [SerializeField] private GameObject m_spotlightOfFlashlight;

    [SerializeField] private Rigidbody m_rbodyFlashlight;

    private void Awake()
    {
        m_rbodyFlashlight = m_flashlight.GetComponent<Rigidbody>();
        m_spotlightOfFlashlight.GetComponent<Light>();
        m_gameManager = FindObjectOfType<GameManager>();
    }

    //----------------------------------------------- Les OnTrigger ------------------------------------------//

    private void OnTriggerStay(Collider p_collide)
    {
        if (m_gameManager.isPc)
        {

            if ((m_playerMask.value & (1 << p_collide.gameObject.layer)) > 0)
            {
                Debug.Log("dans la condition bitch");

                if (m_playerController.m_flashlightIsPossessed == false)
                {
                    m_playerController.TakeFlashlight();
                    m_uiManager.UiTakeFlashlight();
                }
                else if (m_playerController.m_flashlightIsPossessed == true)
                {
                    m_uiManager.UiDisableFlashlight();
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
                    m_uiManager.UiTakeFlashlight();
                    Debug.Log("gamepad ui activée");
                }
                else if (m_playerController.m_flashlightIsPossessed == true)
                {
                    m_uiManager.UiDisableFlashlight();
                }
            }
        }
    }
    private void OnTriggerExit(Collider p_collide)
    {
            if ((m_playerMask.value & (1 << p_collide.gameObject.layer)) > 0)
            {
                m_uiManager.UiDisableFlashlight();
            }
    }

    //----------------------------------------------- Fonctions liées à la veilleuse ------------------------------------------//

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
