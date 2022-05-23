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

    [Range(0,7)] public float m_maxValue = 1f;
    [Range(0, 5)] public float m_minValue = 0f;
    private float m_currentValue;
    [Range(0,10)] public float m_Speed;
    private int m_currentNB;
    [SerializeField] private int m_nbMax;
    private bool isOpen = false;
    private bool isOver = false;
    private void Awake()
    {
        m_currentNB = 0;
        if (volume.profile.TryGet<Vignette>(out vignette))
        {
            vignette.intensity.max = m_maxValue;
        }
    }

    void Update()
    {
        ClignementDesYeux();

    }
    public void ClignementDesYeux()
    {
        if (isOver == false)
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
                if (m_currentValue <= m_minValue)
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

    public void TriggerScreamer()
    {

    }
}
