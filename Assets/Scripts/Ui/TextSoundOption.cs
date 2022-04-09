using System;
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

    private GameManager m_gameManager;

    private MouseLock m_mouseLock;

    public static TextSoundOption instance;

    public float m_sensibility;
    public float m_setNewSensibility;
    public Button[] m_soundButtonList;
    public Button[] m_controlsButtonList;

    [SerializeField, Tooltip("Référence du txt controller")]private TextMeshProUGUI m_txtController;

    [SerializeField] private TextMeshProUGUI m_txtResolution;
    public List<ResolutionParameters> resolutions = new List<ResolutionParameters>();
    private int resolutionCount;
    private bool isFullscreen;

    [SerializeField] private TextMeshProUGUI m_txtLuminosity;
    private float m_brightness;
   
    private void Awake()
    {
        //DontDestroyOnLoad();
        instance = this;
        //COLOR//
        m_selectedColor = Color.yellow;
        m_unselectedColor = Color.white;

        //SOUNDS//
        m_textSoundVolume = GetComponentInChildren<TextMeshProUGUI>();
        m_soundVolume = 10f;

        //CONTROLS
        m_mouseLock = FindObjectOfType<MouseLock>();
        if (m_mouseLock != null)
        {
            m_mouseLock.mouseSensitivity = m_sensibility;
            Debug.Log(m_sensibility, this);
        }
        else
        {
            return;
        }
        if (m_soundButtonList.Length > 0)
        {
            m_textSoundVolume.GetComponent<TextMeshProUGUI>().text = m_soundVolume.ToString();
        }
        else if (m_controlsButtonList.Length > 0)
        {
            m_textSoundVolume.GetComponent<TextMeshProUGUI>().text = m_sensibility.ToString();
        }

        m_gameManager = FindObjectOfType<GameManager>();

        if (m_txtController == null)
        {
            Debug.Log("il n'ya pas de txt controller");
        }

        resolutionCount = 0;
        if (m_txtResolution == null)
        {
            Debug.Log("pas de txt résolution");
        }
        Screen.SetResolution(1920,1080, true);


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

            if(m_soundButtonList.Length > 0 || m_controlsButtonList.Length > 0 || m_txtController !=null || m_txtResolution !=null)
            {
                OnDecrease();
                //Debug.Log("doit decrease");
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (m_soundButtonList.Length > 0 || m_controlsButtonList.Length > 0 ||m_txtController !=null || m_txtResolution !=null)
            {
                OnIncrease();
                //Debug.Log("doit increase");
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
                if (m_sensibility <= 0)
                {
                    m_sensibility = 0f;
                    UpdateSensi();
                }
            }

            if (m_txtController != null)
            {
                Debug.Log(("mtxtcontroller n'est pas à null"));
                if (m_gameManager.isGamepad == true)
                {
                    m_txtController.text = "PC";
                    m_txtController.fontSize = 35;
                    m_gameManager.isGamepad = false;
                    m_gameManager.isPc = true;
                    Debug.Log((m_txtController.text));
                }
                else if (m_gameManager.isPc == true)
                {
                    m_txtController.text = "Gamepad";
                    m_txtController.fontSize = 35;
                    m_gameManager.isGamepad = true;
                    m_gameManager.isPc = false;
                    Debug.Log((m_txtController.text));
                }
            }

            if (m_txtResolution != null)
            {
                resolutionCount--;
                if (resolutionCount <= 0)
                {
                    resolutionCount = 0;
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
                Debug.Log(m_sensibility);
                UpdateSensi();

                if (m_sensibility >= 400)
                {
                    m_sensibility = 400f;
                    UpdateSensi();
                }
            }
            
            if (m_txtController != null)
            {
                Debug.Log(("mtxtcontroller n'est pas à null"));
                if (m_gameManager.isGamepad == true)
                {
                    m_txtController.text = "PC";
                    m_txtController.fontSize = 35;
                    m_gameManager.isGamepad = false;
                    m_gameManager.isPc = true;
                    Debug.Log((m_txtController.text));
                }
                else if (m_gameManager.isPc == true)
                {
                    m_txtController.text = "Gamepad";
                    m_txtController.fontSize = 35;
                    m_gameManager.isGamepad = true;
                    m_gameManager.isPc = false;
                    Debug.Log((m_txtController.text));
                }
            }
            
            if (m_txtResolution != null)
            {
                resolutionCount++;
                if (resolutionCount == 1)
                {
                    Screen.SetResolution(640, 480, false);
                    UpdateResolution();
                }
                else if (resolutionCount == 2)
                {
                    Screen.SetResolution(1280, 720, false);
                    UpdateResolution();

                }
                else if (resolutionCount == 3)
                {
                    Screen.SetResolution(1920, 1080, true);
                    UpdateResolution();
                    resolutionCount = 0;
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
        m_setNewSensibility = m_sensibility;
        m_textSoundVolume.GetComponent<TextMeshProUGUI>().text = m_sensibility.ToString();
    }

    public void UpdateResolution()
    {
        m_txtResolution.GetComponent<TextMeshProUGUI>().text = Screen.resolutions[resolutionCount].ToString();
        Debug.Log(m_txtResolution.text);
    }

    private void OnDisable()
    {
        m_mouseLock.mouseSensitivity = m_sensibility;
        Debug.Log(m_mouseLock.mouseSensitivity, this);
    }
}

[System.Serializable]
public class ResolutionParameters
{
    public int Width, Height;
}
