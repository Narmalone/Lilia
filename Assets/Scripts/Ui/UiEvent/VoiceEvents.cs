using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VoiceEvents : MonoBehaviour
{
    [SerializeField] private AssetMenuScriptValue m_uiEvent;
    [SerializeField] private TextMeshProUGUI m_txtToModify;

    private GameManager m_gameManager;

    Color m_selectedColor = Color.yellow;
    Color m_unselectedColor = Color.white;

    private void OnEnable()
    {
        if (m_uiEvent == null) return;

        m_uiEvent.doSetValue += UpdateValueToString;
    }
    private void OnDisable()
    {
        if (m_uiEvent == null) return;

        m_uiEvent.doSetValue -= UpdateValueToString;
    }
    private void Awake()
    {
        m_gameManager = FindObjectOfType<GameManager>();
        m_uiEvent.value = 50;
        UpdateValueToString();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            OnInrease();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            OnDicrease();
        }

        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            SetSelectedColor();
        }
        else if (EventSystem.current.currentSelectedGameObject != gameObject)
        {
            SetUnselectedColor();
        }
    }

    public void OnInrease()
    {
        if(EventSystem.current.currentSelectedGameObject == gameObject)
        {
            m_uiEvent.value += 5;
            if (m_uiEvent.value >= 100)
            {
                m_uiEvent.value = 100;
            }
            UpdateValueToString();
        }
        else
        {
            return;
        }
       
    }

    public void OnDicrease()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            m_uiEvent.value -= 5;
            if (m_uiEvent.value <= 0)
            {
                m_uiEvent.value = 0;
            }
            UpdateValueToString();
        }
        else { return; }       
    }
    public void UpdateValueToString()
    {
        m_txtToModify.GetComponent<TextMeshProUGUI>().text = m_uiEvent.value.ToString();
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
