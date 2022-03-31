using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class TextSoundOption : MonoBehaviour
{
    public float m_soundVolume;

    public TextMeshProUGUI m_textSoundVolume;
    private Color m_selectedColor;
    private Color m_unselectedColor;

    public float m_sensibility;

    public Button[] m_soundButtonList;
    public Button[] m_controlsButtonList;
    private void Awake()
    {
        //COLOR//
        m_selectedColor = Color.yellow;
        m_unselectedColor = Color.white;

        //SOUNDS//
        m_textSoundVolume = GetComponentInChildren<TextMeshProUGUI>();
        m_soundVolume = 10f;

        //CONTROLS
        m_sensibility = 150f;

        if (m_soundButtonList.Length > 0)
        {
            m_textSoundVolume.GetComponent<TextMeshProUGUI>().text = m_soundVolume.ToString();
        }
        else if (m_controlsButtonList.Length > 0)
        {
            m_textSoundVolume.GetComponent<TextMeshProUGUI>().text = m_sensibility.ToString();
        }

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

        if (Input.GetKeyDown(KeyCode.Q))
        {

            if(m_soundButtonList.Length > 0 || m_controlsButtonList.Length > 0)
            {
                OnDecrease();
                Debug.Log("doit decrease");
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (m_soundButtonList.Length > 0 || m_controlsButtonList.Length > 0)
            {
                OnIncrease();
                Debug.Log("doit increase");
            }
        }
    }

    public void SetSelectedColor()
    {
        m_textSoundVolume.color = m_selectedColor;
    }
    public void SetUnselectedColor()
    {
        m_textSoundVolume.color = m_unselectedColor;
    }

    public void OnDecrease()
    {

        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            if(m_soundButtonList.Length > 0)
            {
                m_soundVolume -= 5f;
                UpdateTextVolume();

                if (m_soundVolume <= 0)
                {
                    m_soundVolume = 0f;
                    UpdateTextVolume();
                }
            }
            if(m_controlsButtonList.Length > 0)
            {
                m_sensibility -= 5f;
                UpdateSensi();
                Debug.Log("diminuer sensi");
                if (m_sensibility <= 0)
                {
                    m_sensibility = 0f;
                    UpdateSensi();
                }
            }
        }
    }

    public void OnIncrease()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {

            if (m_soundButtonList.Length > 0)
            {
                m_soundVolume += 5f;
                UpdateTextVolume();

                if (m_soundVolume >= 100)
                {
                    m_soundVolume = 100f;
                    UpdateTextVolume();
                }
            }
            if(m_controlsButtonList.Length > 0)
            {
                m_sensibility += 5f;
                UpdateSensi();
                Debug.Log("augmenter sensi");

                if (m_sensibility >= 400)
                {
                    m_sensibility = 400f;
                    UpdateSensi();
                }
            }
        }
    }

    public void UpdateTextVolume()
    {
        m_textSoundVolume.GetComponent<TextMeshProUGUI>().text = m_soundVolume.ToString();
    }
    public void UpdateSensi()
    {
        m_textSoundVolume.GetComponent<TextMeshProUGUI>().text = m_sensibility.ToString();
    }
}
