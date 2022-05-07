using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random=UnityEngine.Random;


[RequireComponent(typeof(SphereCollider))]
public class QTEManager : MonoBehaviour
{
    [SerializeField] private SphereCollider m_sphereCol;

    [SerializeField][Range(1,10)] private float m_rangeCol = 0f;
    
    [SerializeField][Range(0,3)] private float m_tempsEntreQTE = 0f;
    
    [SerializeField] private int m_nombreQTE;
    
    private int m_currentNumberQTE = 0;
    
    [SerializeField] private LayerMask m_layerPlayer;

    [SerializeField]private GameObject m_playerGO;

    [SerializeField] private GameObject m_containerPerso;

    private bool m_qteStarted = false;
    
    private bool m_startedCoroutine = false;

    [SerializeField] private KeyCode[] m_keycodesQTE;

    private int m_index;
    
    // Start is called before the first frame update
    void Start()
    {
        m_index = Random.Range(0,m_keycodesQTE.Length);
        
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
            if (m_playerGO.transform.position != m_containerPerso.transform.position)
            {
                m_playerGO.transform.position = Vector3.MoveTowards(m_playerGO.transform.position,m_containerPerso.transform.position,0.1f);
                Debug.Log("pk ca marche pas ta mere la tchoin");
            }
            else if (m_currentNumberQTE < m_nombreQTE)
            {
                Debug.Log("dans le else if connard");
                if (m_startedCoroutine == false)
                {
                    Debug.Log($"Press key {m_keycodesQTE[m_index]}");
                    if (Input.GetKey(m_keycodesQTE[m_index]))
                    {
                        m_startedCoroutine = true;
                        m_currentNumberQTE++;
                        m_index = Random.Range(0, m_keycodesQTE.Length);
                        StartCoroutine(CoroutineWait());
                    }
                }
            }
            
            else if (m_currentNumberQTE >= m_nombreQTE)
            {
                m_qteStarted = false;
                m_currentNumberQTE = 0;
            }
        }
        
    }

    IEnumerator CoroutineWait()
    {
        yield return new WaitForSeconds(m_tempsEntreQTE);
        m_startedCoroutine = false;
    }

    private void OnTriggerStay(Collider p_other)
    {
        if ((m_layerPlayer.value & (1 << p_other.gameObject.layer))>0)
        {
            m_playerGO = p_other.gameObject;
            if (Input.GetKey(KeyCode.E) && m_qteStarted == false)
            {
                Debug.Log("Yes");
                m_qteStarted = true;
            }
        }
    }
}
