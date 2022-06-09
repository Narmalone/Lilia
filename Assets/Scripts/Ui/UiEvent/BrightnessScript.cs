using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BrightnessScript : MonoBehaviour
{
    [SerializeField] private AssetMenuScriptValue m_uiEvent;
    [SerializeField] private TextMeshProUGUI m_txtToModify;

    [SerializeField] Color m_selectedColor;
    Color m_unselectedColor = Color.black;

    private void OnDisable()
    {
        if (m_uiEvent == null) return;

        m_uiEvent.doSetValue -= UpdateBrightnessValue;
    }
    private void OnEnable()
    {
        if (m_uiEvent == null) return;

        m_uiEvent.doSetValue += UpdateBrightnessValue;
    }
    private void Awake()
    {
        //RenderSettings.ambientLight = new Color(m_uiEvent.value, m_uiEvent.value, m_uiEvent.value);
        m_uiEvent.value = 1f;
        UpdateBrightnessValue();
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
            m_uiEvent.value += 0.1f;
            if (m_uiEvent.value >= 1f)
            {
                m_uiEvent.value = 1f;
            }
            UpdateBrightnessValue();
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
            m_uiEvent.value -= 0.1f;
            if (m_uiEvent.value <= 0f)
            {
                m_uiEvent.value = 0f;
            }
            UpdateBrightnessValue();
        }
        else
        {
            return;
        }
    }
    public void UpdateBrightnessValue()
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
