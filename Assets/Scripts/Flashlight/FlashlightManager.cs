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

    private void Awake()
    {
        flashlight.GetComponent<BoxCollider>();
        m_rFlashlight = flashlight.GetComponent<Rigidbody>();
        m_spotlightOfFlashlight.GetComponent<Light>();
    }

    [SerializeField]PlayerController playerController;
    private void OnTriggerStay(Collider p_collide)
    {
        uiManager.takeObject();
        playerController.takeFlashlight();


    }
    private void OnTriggerExit(Collider p_collide)
    {
        uiManager.DisableUi();
    }

    private float m_yRotation = 0f;

    [SerializeField] private Rigidbody m_rFlashlight;

    public void PickItem()
    {
        //m_rFlashlight.useGravity = false;
        //m_rFlashlight.isKinematic = true;

        flashlight.transform.SetParent(FlashlightContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(90f,180f,0f);

        //trouver un moyen d'orienter l'objet en fonction de là ou on regarde
        //m_yRotation = Mathf.Clamp(m_yRotation, -90f, 90f);
        //transform.LookAt(FlashlightContainer, Vector3.left);
        Debug.Log(FlashlightContainer.transform);
        flashlight.GetComponent<BoxCollider>().enabled = false;
        uiManager.DisableUi();

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
