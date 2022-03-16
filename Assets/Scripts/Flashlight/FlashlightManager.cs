using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightManager : MonoBehaviour
{
    [SerializeField]UiManager uiManager;

    [SerializeField, Tooltip("Référence de la torche")]private GameObject flashlight;
    [SerializeField] private Transform FlashlightContainer;

    [SerializeField]private GameObject m_spotlightOfFlashlight;

    [SerializeField] LayerMask _playerMask;

    private void Awake()
    {
        flashlight.GetComponent<BoxCollider>();
        m_rFlashlight = flashlight.GetComponent<Rigidbody>();
        m_spotlightOfFlashlight.GetComponent<Light>();
    }

    [SerializeField]PlayerController playerController;
    private void OnTriggerStay(Collider p_collide)
    {
        if ((_playerMask.value & (1 << p_collide.gameObject.layer)) > 0)
        {
            playerController.takeFlashlight();
            Debug.Log("colliderdétected");
            uiManager.takeObject();
        }


    }
    private void OnTriggerExit(Collider p_collide)
    {
        if ((_playerMask.value & (1 << p_collide.gameObject.layer)) > 0)
        {
            uiManager.DisableUi();
            Debug.Log("Désactive l'Ui");
        }
       
    }

    [SerializeField] private Rigidbody m_rFlashlight;

    public void PickItem()
    {
        m_rFlashlight.useGravity = false;
        m_rFlashlight.isKinematic = true;

        flashlight.transform.SetParent(FlashlightContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(90f,180f,0f);


        bool m_disableCollider = flashlight.GetComponent<BoxCollider>().enabled = false;
        Debug.Log(m_disableCollider);
        uiManager.DisableUi();

    }

    public void DropItem()
    {
        m_rFlashlight.isKinematic = false;
        m_rFlashlight.useGravity = true;


        flashlight.transform.parent = null;      
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
