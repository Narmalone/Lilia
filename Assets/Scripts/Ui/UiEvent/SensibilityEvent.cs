using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SensibilityEvent : MonoBehaviour
{

    [SerializeField] private AssetMenuScriptValue m_uiEvent;
    [SerializeField] private TextMeshProUGUI m_txtToModify;

    private GameManager m_gameManager;

    Color m_selectedColor = Color.yellow;
    Color m_unselectedColor = Color.white;

    private void OnDisable()
    {
        if (m_uiEvent == null) return;

        m_uiEvent.doSetValue -= UpdateSensibility;
    }
    private void OnEnable()
    {
        if (m_uiEvent == null) return;

        m_uiEvent.doSetValue += UpdateSensibility;
    }
    private void Awake()
    {
        m_gameManager = FindObjectOfType<GameManager>();
        m_uiEvent.value = 200f;
        UpdateSensibility();
    }
    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            SetSelectedColor();
        }
        else if (EventSystem.current.currentSelectedGameObject != gameObject)
        {
            SetUnselectedColor();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            OnIncrease();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            OnDecrease();
        }
    }

    public void OnIncrease()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            m_uiEvent.value += 5;
            if (m_uiEvent.value >= 400)
            {
                m_uiEvent.value = 400;
            }
            UpdateSensibility();
        }
        else
        {
            return;
        }
    }
    public void OnDecrease()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            m_uiEvent.value -= 5;
            if (m_uiEvent.value <= 0)
            {
                m_uiEvent.value = 0;
            }
            UpdateSensibility();
        }
        else
        {
            return;
        }
    }
    public void UpdateSensibility()
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
