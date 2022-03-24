using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightManager : MonoBehaviour
{


    //----------------------------------------------- R�f�rences de classes ------------------------------------------//

    [SerializeField]UiManager m_uiManager;
    [SerializeField] PlayerController m_playerController;
    [SerializeField] private GameManager m_gameManager;
    [SerializeField] private LayerMask m_playerMask;

    //----------------------------------------------- Par rapport � la veilleuse ------------------------------------------//


    [SerializeField, Tooltip("R�f�rence de la torche")]private GameObject flashlight;
    [SerializeField] private Transform FlashlightContainer;

    [SerializeField]private GameObject m_spotlightOfFlashlight;
    
    [SerializeField] private Rigidbody m_rbodyFlashlight;

    private void Awake()
    {
        flashlight.GetComponent<BoxCollider>();
        m_rbodyFlashlight = flashlight.GetComponent<Rigidbody>();
        m_spotlightOfFlashlight.GetComponent<Light>();
    }


    //----------------------------------------------- Les OnTrigger ------------------------------------------//

    private void OnTriggerStay(Collider p_collide)
    {
        if (m_gameManager.isPc)
        {

            if ((m_playerMask.value & (1 << p_collide.gameObject.layer)) > 0)
            {
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
                    Debug.Log("gamepad ui activ�e");
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

    //----------------------------------------------- Fonctions li�es � la veilleuse ------------------------------------------//

    public void PickItem()
    {
        //m_rFlashlight.useGravity = false;
        //m_rFlashlight.isKinematic = true;

        flashlight.transform.SetParent(FlashlightContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(90f,180f,0f);

        //trouver un moyen d'orienter l'objet en fonction de l� ou on regarde
        //m_yRotation = Mathf.Clamp(m_yRotation, -90f, 90f);
        //transform.LookAt(FlashlightContainer, Vector3.left);
        Debug.Log(FlashlightContainer.transform);
        flashlight.GetComponent<BoxCollider>().enabled = false;
        m_uiManager.DisableUi();

    }
    public void DropItem()
    {
        //m_rFlashlight.isKinematic = false;
        //m_rFlashlight.useGravity = true;


        flashlight.transform.parent = null;
        Debug.Log("Drop l'item");
      
    }

    private int flashCount = 0;
    public bool switchLight = false;



    public void UseFlashlight()
    {
        if(flashCount == 0 && switchLight == false)
        {
            flashCount++;
            m_spotlightOfFlashlight.GetComponent<Light>().intensity = 3;
            switchLight = true;

        }
        else if (flashCount == 1 && switchLight == true)
        {
            m_spotlightOfFlashlight.GetComponent<Light>().intensity = 0;
            flashCount--;
            switchLight = false;
        }
    }
}
