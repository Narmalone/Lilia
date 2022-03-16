using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doudou : MonoBehaviour
{
    [SerializeField]UiManager uiManager;

    [SerializeField, Tooltip("R�f�rence de la torche")]private GameObject m_doudou;
    [SerializeField] private Transform m_emplacementDoudou;
    private Rigidbody m_rbDoudou;
    private void Awake()
    {
        m_doudou.GetComponent<BoxCollider>();
        m_rbDoudou = m_doudou.GetComponent<Rigidbody>();
        m_emplacementDoudou.GetComponent<Light>();
    }

    [SerializeField]PlayerController playerController;
    private void OnTriggerStay(Collider p_collide)
    {
        uiManager.takeObject();
        playerController.TakeDoudou();


    }
    private void OnTriggerExit(Collider p_collide)
    {
        uiManager.DisableUi();
    }

    private float m_yRotation = 0f;

    public void PickItem()
    {
        //m_rFlashlight.useGravity = false;
        //m_rFlashlight.isKinematic = true;

        m_doudou.transform.SetParent(m_emplacementDoudou);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(90f,180f,0f);

        //trouver un moyen d'orienter l'objet en fonction de l� ou on regarde
        //m_yRotation = Mathf.Clamp(m_yRotation, -90f, 90f);
        //transform.LookAt(FlashlightContainer, Vector3.left);
        Debug.Log(m_emplacementDoudou.transform);
        m_doudou.GetComponent<BoxCollider>().enabled = false;
        uiManager.DisableUi();

    }

    public void DropItem()
    {
        //m_rFlashlight.isKinematic = false;
        //m_rFlashlight.useGravity = true;


        m_doudou.transform.parent = null;
        Debug.Log("Drop l'item");
      
    }

    public void UseDoudou()
    {
    }
}
