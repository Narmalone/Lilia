using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StressManager : MonoBehaviour
{
    //[SerializeField, Tooltip("Stress actuel du personnage")]private float m_currentStress;
    //[SerializeField, Tooltip("Augmentation du stress du joueur")] private float m_upgradeStress;

    private bool isCinematic;

    [SerializeField] 
    private Slider m_slider;

    private int m_currentValue;
    public void SetMaxHealth(int p_health)
    {
        m_slider.maxValue = p_health;
        m_slider.value = p_health;
    }

    public void SetStress(int p_health)
    {
        m_currentValue = p_health;
    }
    
    private void Start()
    {
        isCinematic = false;
    }


    private void Update()
    {
        if (m_slider.value != m_currentValue)
        {
            m_slider.value = Mathf.Lerp(m_slider.value,m_currentValue,10f*Time.deltaTime);
        }
        
        /*if (isCinematic == true)
        {
            m_upgradeStress = 0;
        }
        else
        {
            m_currentStress += m_upgradeStress;
            m_upgradeStress = 0.001f;
        }*/
        //Debug.Log(m_currentStress);
    }
}
