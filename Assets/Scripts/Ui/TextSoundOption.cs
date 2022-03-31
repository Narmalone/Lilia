using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TextSoundOption : MonoBehaviour
{
    public float m_soundVolume;

    public TextMeshProUGUI m_textSoundVolume;

    private Color m_selectedColor;
    private Color m_unselectedColor;

    public float m_sensibility;

    private void Awake()
    {
        //COLOR//
        m_selectedColor = Color.yellow;
        m_unselectedColor = Color.white;

        //SOUNDS//
        m_textSoundVolume = GetComponentInChildren<TextMeshProUGUI>();
        m_soundVolume = 10f;
        m_textSoundVolume.GetComponent<TextMeshProUGUI>().text = m_soundVolume.ToString();

        //CONTROLS
        m_sensibility = 150f;

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
            OnDecrease();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            OnIncrease();
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
            m_soundVolume -= 5f;
            UpdateTextVolume();

            if(m_soundVolume <= 0)
            {
                m_soundVolume = 0f;
                UpdateTextVolume();
            }
        }
        else if (EventSystem.current.currentSelectedGameObject != gameObject)
        {
            Debug.Log("sur un autre gameobject");
        }

        if (EventSystem.current.currentSelectedGameObject == GameObject.Find("B_Sensibility"))
        {
            m_sensibility -= 5f;
            UpdateTextControls();

            if (m_sensibility <= 0)
            {
                m_sensibility = 0f;
                UpdateTextControls();
            }
        }
    }

    public void OnIncrease()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            m_soundVolume += 5f;
            UpdateTextVolume();

            if (m_soundVolume >= 100)
            {
                m_soundVolume = 100f;
                UpdateTextVolume();
            }
        }
        else if (EventSystem.current.currentSelectedGameObject != gameObject)
        {
            Debug.Log("sur un autre gameobject");
        }


        if (EventSystem.current.currentSelectedGameObject == GameObject.Find("B_Sensibility"))
        {
            m_sensibility += 5f;
            UpdateTextControls();

            if (m_sensibility >= 400)
            {
                m_sensibility = 400f;
                UpdateTextControls();
            }
        }
    }

    public void UpdateTextVolume()
    {
        m_textSoundVolume.GetComponent<TextMeshProUGUI>().text = m_soundVolume.ToString();
    }
    public void UpdateTextControls()
    {
        m_textSoundVolume.GetComponent<TextMeshProUGUI>().text = m_sensibility.ToString();
    }
}
