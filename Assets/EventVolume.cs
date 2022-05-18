using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EventVolume : MonoBehaviour
{
    public Volume volume;
    Vignette vignette;
    [SerializeField] private PlayerController m_player;

    [Range(0,1)] public float m_maxValue = 1;
    [Range(0, 1)] public float m_minValue = 0;
    private float m_currentValue;
    [Range(0,3)] public float m_Speed;
    private int m_currentNB;
    [SerializeField] private int m_nbMax;
    private bool isOpen = false;
    private bool isOver = false;
    private void Start()
    {
        m_currentNB = 0;
        if (volume.profile.TryGet<Vignette>(out vignette))
        {
            //vignette.color.value = new Color(255f, 25f, 25f);
        }
    }

    void Update()
    {
        if(isOver == false)
        {
            if (isOpen == false)
            {
                m_currentValue = vignette.intensity.value;
                vignette.intensity.value = Mathf.MoveTowards(m_currentValue, m_maxValue, m_Speed * Time.deltaTime);
                if (m_currentValue >= m_maxValue)
                {
                    isOpen = true;
                }
            }
            else if (isOpen == true)
            {
                m_currentValue = vignette.intensity.value;
                vignette.intensity.value = Mathf.MoveTowards(m_currentValue, m_minValue, m_Speed * Time.deltaTime);
                if (m_currentValue <= 0f)
                {
                    isOpen = false;
                    m_currentNB++;
                }
                Debug.Log(m_currentNB);
                if (m_currentNB >= m_nbMax)
                {
                    m_player.isCinematic = false;
                    isOver = true;
                }
            }
        }
        else
        {
            return;
        }
     
    }
}
