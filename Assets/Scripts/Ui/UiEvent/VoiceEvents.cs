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
    [SerializeField] private AudioManagerScript m_audioScript;
    public float valueToDisplay;
    private GameManager m_gameManager;

    Color m_selectedColor = Color.yellow;
    Color m_unselectedColor = Color.white;

    private void OnEnable()
    {
        if (m_uiEvent == null) return;

    }
    private void OnDisable()
    {
        if (m_uiEvent == null) return;

    }
    private void Awake()
    {
        m_gameManager = FindObjectOfType<GameManager>();
        m_audioScript = FindObjectOfType<AudioManagerScript>();
        m_uiEvent.value = 0.5f;
        valueToDisplay = 50f;
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
            m_uiEvent.value += 0.1f;
            valueToDisplay += 10f;
            if (m_uiEvent.value >= 1f)
            {
                m_uiEvent.value = 1f;
            }
            if (valueToDisplay >= 100f)
            {
                valueToDisplay = 100f;
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
            m_uiEvent.value -= 0.1f;
            valueToDisplay -= 10f;
            if (m_uiEvent.value <= 0f)
            {
                m_uiEvent.value = 0f;
            }
            if (valueToDisplay <= 0f)
            {
                valueToDisplay = 0f;
            }
            UpdateValueToString();
            m_audioScript.SetNewValue();
        }
        else { return; }       
    }
    public void UpdateValueToString()
    {
        m_txtToModify.GetComponent<TextMeshProUGUI>().text = valueToDisplay.ToString();
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
