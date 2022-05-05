using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
public class ChangeLanguage : MonoBehaviour
{
    [SerializeField] private AssetMenuScriptValue m_uiEvent;
    [SerializeField] private Txt_Language m_txtLanguage;
    [SerializeField] private TextMeshProUGUI m_txtToModify;

    private GameManager m_gameManager;
    Color m_selectedColor = Color.yellow;
    Color m_unselectedColor = Color.white;
    private void Awake()
    {
        m_gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            SetSelectedColor();
            if (Input.GetKeyDown(KeyCode.D))
            {
                OnIncrease();
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                OnDecrease();
            }
        }
        else if (EventSystem.current.currentSelectedGameObject != gameObject)
        {
            SetUnselectedColor();
        }
    }

    public void OnIncrease()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            m_txtLanguage.index++;
            if (m_txtLanguage.index > m_txtLanguage.m_Sentence.Count - 1)
            {
                m_txtLanguage.index = m_txtLanguage.m_Sentence.Count - 1;
            }
            UpdateLanguage();

        }
        else{return;}
    }
    public void OnDecrease()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            m_txtLanguage.index--;
            if (m_txtLanguage.index <= 0)
            {
                m_txtLanguage.index = 0;
            }
            UpdateLanguage();
        }
        else{return;}
    }
    public void UpdateLanguage()
    {
        m_txtToModify.text = m_txtLanguage.m_Sentence[m_txtLanguage.index]; 
    }
    public void SetSelectedColor()
    {
        m_txtToModify.color = m_selectedColor;

    }
    public void SetUnselectedColor()
    {
        m_txtToModify.color = m_unselectedColor;

    }
}
