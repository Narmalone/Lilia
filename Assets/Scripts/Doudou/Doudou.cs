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
    

    private float m_yRotation = 0f;

    public void PickItem()
    {
        m_rbDoudou.isKinematic = true;
        m_rbDoudou.useGravity = false;
        /*
        m_doudou.transform.SetParent(m_emplacementDoudou);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0f,-0f,0f);
        */
        m_doudou.transform.position = new Vector3(1100f, 1100f, 1100f);
        m_doudou.GetComponent<BoxCollider>().enabled = false;
        uiManager.DisableUi();

    }

    public void DropItem()
    {
        m_doudou.transform.localPosition = m_emplacementDoudou.transform.position;
        m_doudou.transform.SetParent(m_emplacementDoudou);
        m_doudou.transform.localRotation = Quaternion.Euler(0f,-0f,0f);
        
        m_rbDoudou.isKinematic = false;
        m_rbDoudou.useGravity = true;
        m_doudou.transform.parent = null;      
    }
}
