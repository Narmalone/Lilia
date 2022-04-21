using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PortillonScript : MonoBehaviour
{
    [SerializeField] private LayerMask m_playerMask;
    [SerializeField] private GameManager m_gameManager;

    [SerializeField] private BoxCollider m_boxCollider;

    public Slider sliderInstance;
    
    private Animator m_animator;
    private string m_openAnim = "Open";

    [SerializeField, Tooltip("Vitesse à laquelle le joueur ouvre le portillon temps de secondes"), Range(0,5)]private float m_speedToOpen;
    [SerializeField, Tooltip("Vitesse à laquelle la valeur s'incrémente par seconde"),Range(0,1)]private float m_incrementValue;
    private void Awake()
    {
        if (m_animator == null)
        {
            m_animator = GetComponentInParent<Animator>();
        };
        
        sliderInstance.minValue = 0;
        sliderInstance.value = m_incrementValue;
        sliderInstance.maxValue = m_speedToOpen;
    }

    private void Start()
    {
        sliderInstance.gameObject.SetActive(false);
    }

    public void OnComplete()
    {
        m_animator.SetBool(m_openAnim,true);
        Animator.StringToHash(m_openAnim);
        m_boxCollider.enabled = false;
        Debug.Log("jouer l'anim");
    }

    public void UnlockPortillon()
    {
        Debug.Log("dans le unlock portillon");
        sliderInstance.gameObject.SetActive(true);
        if (m_incrementValue < m_speedToOpen)
        {
            m_incrementValue += m_incrementValue * Time.deltaTime;
            sliderInstance.value = m_incrementValue;
            Debug.Log(m_incrementValue);
        }
        else if (m_incrementValue >= m_speedToOpen)
        {
            OnComplete();
            sliderInstance.gameObject.SetActive(false);
            Debug.Log("Complété");
        }
      
    }
}
