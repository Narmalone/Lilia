using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using FMODUnity;
using Random=UnityEngine.Random;


[RequireComponent(typeof(BoxCollider))]
public class QTEManager : MonoBehaviour
{
    [SerializeField] private BoxCollider m_boxCol;

    [SerializeField][Range(1,10)] private float m_rangeCol = 0f;
    
    [SerializeField][Range(0,3)] private float m_tempsEntreQTE = 0f;
    [SerializeField] private PlayerController m_player;
    [SerializeField] private GameObject m_img;
    [SerializeField] private TextMeshProUGUI m_txtToModify;
    [SerializeField] private TextMeshProUGUI m_txtPushTheBox;
    [SerializeField] private TextMeshProUGUI m_txtCancelAction;
    [SerializeField] private TxtPuzzle_1 m_txtPuzzle;

    [SerializeField] private PlayerController m_playerController;
    [SerializeField] public int m_nombreQTE;
    [SerializeField] private UiManager m_uiManager;
    private int m_currentNumberQTE = 0;
    
    [SerializeField] private LayerMask m_layerPlayer;

    [SerializeField]private GameObject m_playerGO;

    [SerializeField] private GameObject m_containerPerso;

    private bool m_qteStarted = false;
    
    private bool m_startedCoroutine = false;

    [SerializeField] public KeyCode[] m_keycodesQTE;

    public int m_index;

    public bool m_qteIsOver = false;
    public bool isInQte = false;
    public bool canDoQte = false;
    private Ray m_ray;

    private RaycastHit m_hit,m_pastHit;

    private int m_nombre_de_départ_qte;

    private Renderer m_thisRend;

    [SerializeField] private StudioEventEmitter m_clip;
    [SerializeField] private PlayerScriptAnim m_pcAnim;
    private void Awake()
    {
        isInQte = false;
        canDoQte = false;
        m_txtPushTheBox.gameObject.SetActive(false);
        m_img.gameObject.SetActive(false);
        m_txtCancelAction.gameObject.SetActive(false);
        m_thisRend = GetComponent<Renderer>();
    }
    void Start()
    {
        m_index = Random.Range(0,m_keycodesQTE.Length);
        
        if (!m_containerPerso)
        {
            Debug.Log("HEY, tu n'as pas mis le containerPerso dans l'inspecteur",this);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
      
        m_ray = m_playerController.m_cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        m_playerController.m_ray = m_ray;
        Debug.DrawRay(m_ray.origin,m_ray.direction, Color.red);
        if (Physics.Raycast(m_ray, out m_hit, 1, ~m_layerPlayer))
        {
            OnRayCastHit(m_hit.collider);
            m_pastHit = m_hit;
        }
        else
        {
            if (m_pastHit.collider != null) OnRaycastExit(m_pastHit.collider);
        }
        
        if (m_qteStarted == true)
        {
            if (m_currentNumberQTE < m_nombreQTE)
            {
                m_img.SetActive(true);
                m_txtPuzzle.UpdateText();
                m_txtCancelAction.gameObject.SetActive(true);
                StartCoroutine(canUnlock());
                if (m_startedCoroutine == false)
                {
                    if (Input.GetKey(m_keycodesQTE[m_index]))
                    {
                        m_clip.Play();
                        m_txtPuzzle.SetNewPosition();
                        m_startedCoroutine = true;
                        m_currentNumberQTE++;
                        m_index = Random.Range(0, m_keycodesQTE.Length);
                        m_pcAnim.PlayAnimToArmoire();
                        StartCoroutine(CoroutineWait());
                    }
                }
            }
            
            else if (m_currentNumberQTE >= m_nombreQTE)
            {
                m_qteStarted = false;
                m_currentNumberQTE = 0;
                m_qteIsOver = true;
                isInQte = false;
                StopCoroutine(CoroutineWait());
                m_img.SetActive(false);
                m_txtCancelAction.gameObject.SetActive(false);
                m_txtToModify.gameObject.SetActive(false);
                m_uiManager.DisableUi();
                StartCoroutine(canUnlock());
            }
        }
        
        
    }
    private void LateUpdate()
    {
        if (m_qteStarted == true)
        {
            if (Vector3.Distance(m_playerGO.transform.position, m_containerPerso.transform.position) > 0.2f)
            {
                m_playerGO.transform.position = Vector3.MoveTowards(m_playerGO.transform.position, m_containerPerso.transform.position, 0.1f);
                Debug.Log($"bouger le joueur vers le container : {Vector3.Distance(transform.position, m_containerPerso.transform.position)}");
            }
        }
    }

    IEnumerator CoroutineWait()
    {
        yield return new WaitForSeconds(m_tempsEntreQTE);
        m_startedCoroutine = false;
    }
    
    private void OnRayCastHit(Collider other)
    {
        //Debug.Log("Je suis dans le raycast pour le qte");
        if (ReferenceEquals( gameObject, other.gameObject) && other.isTrigger)
        {
           // Debug.Log("Je vise le meuble");
            if (m_qteIsOver == false && m_txtPushTheBox.gameObject.activeInHierarchy == false)
            {
                if(isInQte == false)
                {
                    if(canDoQte == true)
                    {
                        m_thisRend.GetComponent<Renderer>().material.SetFloat("_BooleanFloat", 1f);
                        if (m_player.isLeftHandFull == false && m_player.isRightHandFull == false)
                        {
                            m_uiManager.TakableObject();
                            if (Input.GetKeyDown(KeyCode.E) && m_qteStarted == false)
                            {
                                if (m_qteIsOver == false)
                                {
                                    if (canDoQte == true)
                                    {
                                        Debug.Log(m_nombre_de_départ_qte++);
                                        m_txtCancelAction.gameObject.SetActive(false);
                                        m_uiManager.DisableUi();
                                        m_txtToModify.gameObject.SetActive(true);
                                        m_qteStarted = true;
                                        isInQte = true;
                                    }
                                }

                            }
                        }
                        else
                        {
                            m_uiManager.DropSomethingBefore();
                            m_uiManager.AnimUi();
                        }
                    }
                }
            }
        }
        else
        {
            OnRaycastExit(m_pastHit.collider);
        }
    }
    
    private void OnRaycastExit(Collider other)
    {
        if (ReferenceEquals( gameObject, m_pastHit.collider?.gameObject) && other.isTrigger)
        {
            Debug.Log("Je ne vise plus le meuble");
            m_uiManager.DisableUi();
            m_uiManager.StopRaycastBefore();
            m_thisRend.GetComponent<Renderer>().material.SetFloat("_BooleanFloat", 0f);
            m_txtPushTheBox.gameObject.SetActive(false);
        }

    }

    IEnumerator canUnlock()
    {
        yield return new WaitForSeconds(0.2f);
        DelockPos();
    }
    public void DelockPos()
    {
        StopCoroutine(canUnlock());
        if (Input.GetKeyDown(KeyCode.E))
        {
            m_img.SetActive(false);
            m_txtCancelAction.gameObject.SetActive(false);
            m_txtToModify.gameObject.SetActive(false);
            m_qteStarted = false;
            isInQte = false;
        }
    }

}
