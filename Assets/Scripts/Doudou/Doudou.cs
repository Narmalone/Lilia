using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doudou : MonoBehaviour
{
    [SerializeField]UiManager uiManager;
    [SerializeField] private LayerMask m_stairsMask;
    [SerializeField, Tooltip("R�f�rence de la torche")]private GameObject m_doudou;
    [SerializeField] private Transform m_emplacementDoudou;
    private BoxCollider m_boxDoudouColider;
    private Rigidbody m_rbDoudou;
    [SerializeField] private float m_stepOffset = 0.2f;
    public bool m_callEvent = false;

    [SerializeField] WaypointsEvent m_waypointsEvent;

    private void Awake()
    {
        m_boxDoudouColider = m_doudou.GetComponent<BoxCollider>();
        m_rbDoudou = m_doudou.GetComponent<Rigidbody>();
        m_callEvent = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if ((m_stairsMask.value & (1 << other.gameObject.layer)) > 0)
        {
            if (m_callEvent == true)
            {
                Debug.Log("dans la condition de l'event");
                m_doudou.transform.localPosition = new Vector3(m_doudou.transform.localPosition.x, m_doudou.transform.localPosition.y + m_stepOffset, m_doudou.transform.localPosition.z);
                m_rbDoudou.isKinematic = true;
                m_rbDoudou.useGravity = false;
                m_boxDoudouColider.enabled = false;
            }
        }
            Debug.Log("On collision enter");
    }

 
    private float m_yRotation = 0f;

    public void PickItem()
    {
        m_rbDoudou.isKinematic = true;
        m_rbDoudou.useGravity = false;
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
