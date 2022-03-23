﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoudouManager : MonoBehaviour
{

    //----------------------------------------------- Références de classes ------------------------------------------//

    UiManager m_uiManager;
    GameManager m_gameManager;
    PlayerController m_playerController;
    [SerializeField] private LayerMask m_playerMask;

    //----------------------------------------------- Par rapport au doudou ------------------------------------------//
    [SerializeField, Tooltip("Référence du doudou")] private GameObject m_doudou;
    [SerializeField] private Transform m_doudouContainer;
    [SerializeField] private Rigidbody m_rbDoudou;

    private RaycastHit m_hit;

    private void Awake()
    {
        m_doudou.GetComponent<BoxCollider>();
        m_rbDoudou = m_doudou.GetComponent<Rigidbody>();
        m_gameManager = FindObjectOfType<GameManager>();
        m_uiManager = FindObjectOfType<UiManager>();
        m_playerController = FindObjectOfType<PlayerController>();
    }


    //----------------------------------------------- Si le joueur trigger le doudou ------------------------------------------//

    private void OnTriggerStay(Collider p_collide)
    {
        if (m_gameManager.isPc)
        {
            if ((m_playerMask.value & (1 << p_collide.gameObject.layer)) > 0 && Physics.Raycast(transform.position, -Vector3.up, out m_hit, 100f))
            {

                if (m_playerController.m_doudouIsPossessed == false)
                {
                    m_playerController.TakeDoudou();
                    m_uiManager.TakeObject();
                }
                else if (m_playerController.m_doudouIsPossessed == true)
                {
                    m_uiManager.DisableUi();
                }
            }
        }
        else if (m_gameManager.isGamepad)
        {
            if ((m_playerMask.value & (1 << p_collide.gameObject.layer)) > 0)
            {

                if (m_playerController.m_doudouIsPossessed == false)
                {
                    m_playerController.TakeDoudou();
                    m_uiManager.TakeObject();
                }
                else if (m_playerController.m_doudouIsPossessed == true)
                {
                    m_uiManager.DisableUi();
                }
            }
        }

    }
    private void OnTriggerExit(Collider p_collide)
    {
        if ((m_playerMask.value & (1 << p_collide.gameObject.layer)) > 0)
        {
            m_uiManager.DisableUi();
        }
    }

    //----------------------------------------------- Fonctions de prises ou drop du doudou ------------------------------------------//

    public void PickItem()
    {
        if (m_gameManager.isPc)
        {
            m_rbDoudou.useGravity = false;
            m_rbDoudou.isKinematic = true;

            m_doudou.transform.SetParent(m_doudouContainer);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(90f, 180f, 0f);
        }
        else if (m_gameManager.isGamepad)
        {
            m_rbDoudou.useGravity = false;
            m_rbDoudou.isKinematic = true;

            m_rbDoudou.transform.SetParent(m_doudouContainer);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(90f, 180f, 0f);
        }

    }

    public void DropItem()
    {

        if (m_gameManager.isPc)
        {
            m_rbDoudou.isKinematic = false;
            m_rbDoudou.useGravity = true;


            m_doudou.transform.parent = null;
        }
        else if (m_gameManager.isGamepad)
        {
            m_rbDoudou.isKinematic = false;
            m_rbDoudou.useGravity = true;

            m_doudou.transform.parent = null;
        }

    }
}