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
    [SerializeField] private Light m_lightReference;

    //----------------------------------------------- Par rapport � la veilleuse ------------------------------------------//


    [SerializeField, Tooltip("R�f�rence de la torche")]private GameObject flashlight;
    [SerializeField] private Transform FlashlightContainer;    
    [SerializeField] private Rigidbody m_rbodyFlashlight;

    private void Awake()
    {
        flashlight.GetComponent<BoxCollider>();
        m_rbodyFlashlight = flashlight.GetComponent<Rigidbody>();
        m_lightReference.gameObject.SetActive(false);
    }

    //----------------------------------------------- Fonctions li�es � la veilleuse ------------------------------------------//

    public void PickItem()
    {
        m_rbodyFlashlight.useGravity = false;
        m_rbodyFlashlight.isKinematic = true;

        /*
        flashlight.transform.SetParent(FlashlightContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(90f,180f,0f);
        */
        flashlight.transform.position = new Vector3(1000f, 1000f, 1000f);

        flashlight.GetComponent<BoxCollider>().enabled = false;
        m_uiManager.DisableUi();
        m_lightReference.gameObject.SetActive(true);

    }
    public void DropItem()
    {
        m_rbodyFlashlight.isKinematic = false;
        m_rbodyFlashlight.useGravity = true;
        flashlight.transform.position = FlashlightContainer.transform.position;
        flashlight.transform.SetParent(FlashlightContainer);
        flashlight.transform.localRotation = Quaternion.Euler(0f, -0f, 0f);
        flashlight.transform.parent = null;
        Debug.Log("Drop l'item");
        m_lightReference.gameObject.SetActive(false);

    }
}
