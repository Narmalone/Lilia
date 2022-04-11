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

    //----------------------------------------------- Fonctions li�es � la veilleuse ------------------------------------------//

    public void PickItem()
    {
        m_rbodyFlashlight.useGravity = false;
        m_rbodyFlashlight.isKinematic = true;

        flashlight.transform.SetParent(FlashlightContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(90f,180f,0f);

        flashlight.GetComponent<BoxCollider>().enabled = false;
        m_uiManager.DisableUi();

    }
    public void DropItem()
    {
        m_rbodyFlashlight.isKinematic = false;
        m_rbodyFlashlight.useGravity = true;


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
