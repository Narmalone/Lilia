using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Txt_From_Scriptable : MonoBehaviour
{
    [SerializeField] private Txt_Language m_txtLanguage;

    [SerializeField] private TextMeshProUGUI m_txtToModify;

    private void Awake()
    {
        m_txtToModify = GetComponent<TextMeshProUGUI>();
    }

    public void OnChangeLanguage()
    {
        if (m_txtLanguage.index == 0)
        {
            ClearText();
            m_txtToModify.text = m_txtLanguage.m_Sentence[0];
        }
        else if (m_txtLanguage.index == 1)
        {
            ClearText();
            m_txtToModify.text = m_txtLanguage.m_Sentence[1];
        }
    }

    public void ClearText()
    {
        m_txtToModify.text = String.Empty;
    }

}
