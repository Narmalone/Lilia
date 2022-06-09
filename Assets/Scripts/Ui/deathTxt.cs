using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class deathTxt : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_txtToModify;

    [SerializeField] private float m_desiredAlpha = 1f; //Alpha d�sir� entre 0 et 1
    [SerializeField] private float m_currentAlpha = 0f; //Alpha actuelle
    [SerializeField] private float speedToDisplayValue = 0.8f;

    public bool isGood = false;
    
    private void OnEnable()
    {
        m_desiredAlpha = 1f;
        m_currentAlpha = 0f;
        m_txtToModify.alpha = m_currentAlpha;
    }
    private void OnDisable()
    {
        m_desiredAlpha = 1f;
        m_currentAlpha = 0f;
        m_txtToModify.alpha = m_currentAlpha;
    }
    private void Update()
    {
        OnFade();
        m_txtToModify.alpha = m_currentAlpha;
    }
    public void OnFade()
    {
        if(isGood == false)
        {
            m_desiredAlpha = 1;
            Debug.Log("Move le txt doit aller de l'avant");
            m_currentAlpha = Mathf.MoveTowards(m_currentAlpha, m_desiredAlpha, speedToDisplayValue * Time.deltaTime);
            if (m_currentAlpha >= m_desiredAlpha)
            {
                isGood = true;
            }
        }
        else
        {
            m_desiredAlpha = 0;
            m_currentAlpha = Mathf.MoveTowards(m_currentAlpha, m_desiredAlpha, speedToDisplayValue * Time.deltaTime);
            if(m_currentAlpha <= m_desiredAlpha)
            {
                isGood = false;
            }
        }
    }
}
