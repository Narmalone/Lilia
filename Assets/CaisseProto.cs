using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CaisseProto : MonoBehaviour
{

    [SerializeField] private Event1 m_triggerEvent;
    [SerializeField] private GameObject m_thisGameObject;
    private PlayerController m_player;
    private Rigidbody m_rbody;
    private UiManager m_uiManager;
    private BoxCollider m_boxCollider;
    [SerializeField] private Transform m_twoHandsContainer;
    [SerializeField] private LayerMask m_playerMask;

    public bool onHand;
    public bool canTake;

    private void Awake()
    {
        m_player = FindObjectOfType<PlayerController>();
        m_rbody = GetComponent<Rigidbody>();
        m_uiManager = FindObjectOfType<UiManager>();
        m_boxCollider = GetComponent<BoxCollider>();

        onHand = false;
        canTake = false;
    }
    private void OnEnable()
    {
        m_triggerEvent.onTriggered += HandleTriggerEvent;
    }
    private void OnDisable()
    {
        m_triggerEvent.onTriggered -= HandleTriggerEvent;
    }
    public void HandleTriggerEvent(Vector3 p_newPos)
    {
        if (onHand == false)
        {
            CanTake();
            canTake = true;
            m_uiManager.TakableObject();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        m_uiManager.DisableUi();
        if ((m_playerMask.value & (1 << other.gameObject.layer)) > 0)
        {
            m_uiManager.DisableUi();
            canTake = false;
        }
        Debug.Log("trigger exit");
    }
    private void Update()
    {
        CanTake();
        OnHand();
    }
    public void CanTake()
    {
        if (Input.GetKeyDown(KeyCode.E) && m_boxCollider.isTrigger == true && canTake == true)
        {
            m_thisGameObject.transform.SetParent(m_twoHandsContainer);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(90f, 180f, 0f);
            m_rbody.useGravity = false;
            m_rbody.isKinematic = true;
            onHand = true;
            Debug.Log("prendre l'objet");
        }
    }
    public void OnHand()
    {
        if (Input.GetKeyDown(KeyCode.G) && onHand == true)
        {
            m_thisGameObject.transform.parent = null;
            m_rbody.useGravity = true;
            m_rbody.isKinematic = false;
            onHand = false;
            Debug.Log("lâcher la caisse");
        }
    }
}
