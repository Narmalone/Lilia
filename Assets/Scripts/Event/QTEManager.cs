using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(SphereCollider))]
public class QTEManager : MonoBehaviour
{
    [SerializeField] private GameObject m_containerPerso;

    [SerializeField] private SphereCollider m_sphereCol;

    [SerializeField][Range(1,10)] private float m_rangeCol;

    [SerializeField] private LayerMask m_layerPlayer;

    private GameObject m_playerGO;

    private bool m_qteStarted;
    
    private bool m_qtePlaying;

    public class KeyTxt
    {
        private KeyCode m_keycode;

        private string m_keyName;
    }
    
    [SerializeField] private KeyTxt[] m_keysQTE;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!m_containerPerso)
        {
            Debug.Log("HEY, tu n'as pas mis le containerPerso dans l'inspecteur",this);
        }
    }

    private void OnValidate()
    {
        m_sphereCol.radius = m_rangeCol;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_qteStarted == true)
        {
            if (Vector3.Distance(transform.position, m_containerPerso.transform.position) > 1)
            {
                Vector3.MoveTowards(m_playerGO.transform.position,m_containerPerso.transform.position,0.1f);
            }
            else
            {
                QTEStart();
                m_qteStarted = false;
                m_qtePlaying = true;
            }
        }
        
    }

    private void QTEStart()
    {
        
    }

    private void OnTriggerStay(Collider p_other)
    {
        
        if ((m_layerPlayer.value & (1 << p_other.gameObject.layer))>0)
        {
            m_playerGO = p_other.gameObject;
            if (Input.GetKey(KeyCode.E) && m_qteStarted == false && m_qtePlaying == true)
            {
                m_qteStarted = true;
            }
        }
    }
}
