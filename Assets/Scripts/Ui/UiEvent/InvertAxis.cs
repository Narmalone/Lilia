using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class InvertAxis : MonoBehaviour
{
    [SerializeField] private AssetMenuScriptValue m_uiEvent;
    [SerializeField] private TextMeshProUGUI m_txtToModify;
    
    private GameManager m_gameManager;

    public float x;
    public float z;
    private void OnEnable()
    {
        m_uiEvent.doBool += ChangeAxis;
    }

    private void OnDisable()
    {
        m_uiEvent.doBool -= ChangeAxis;
    }

    public void ChangeAxis()
    {
        if (m_uiEvent.uiBool == true)
        {
            m_uiEvent.uiBool = false;
        }
        if (m_uiEvent.uiBool == false)
        {
            m_uiEvent.uiBool = true;
        }
    }
}
