using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  TMPro;
using UnityEngine.EventSystems;
public class ScreenStatus : MonoBehaviour
{
    [SerializeField] private AssetMenuScriptValue m_uiEvent;
    [SerializeField] private TextMeshProUGUI m_txtToModify;

    [SerializeField] Color m_selectedColor;
    Color m_unselectedColor = Color.black;
    public List<ScreenSize> screenSize = new List<ScreenSize>();
    
    public int screenCount;
    public void SetSelectedColor()
    {
        m_txtToModify.color = m_selectedColor;

    }
    public void SetUnselectedColor()
    {
        m_txtToModify.color = m_unselectedColor;

    }

    public int currentScreen;
    public bool isFullScreen;

    private void OnDisable()
    {
        m_uiEvent.doBool -= UpdateScreenSize;
    }

    private void OnEnable()
    {
        m_uiEvent.doBool += UpdateScreenSize;
    }

    private void Awake()
    {
        screenCount = 0;
        UpdateScreenSize();
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

    public void UpdateScreenSize()
    {
        m_txtToModify.GetComponent<TextMeshProUGUI>().text = screenSize[screenCount].ScreenStatus.ToString();
    }

    public void OnIncrease()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            screenCount++;
            if (screenCount >= 2)
            {
                screenCount = 2;
                Screen.fullScreenMode = FullScreenMode.Windowed;
                currentScreen = 2;
            }
            else if (screenCount == 1)
            {
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
            }
            UpdateScreenSize();
        }else{return;}
    }

    public void OnDecrease()
    {
        screenCount--;
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            if (screenCount == 1)
            {
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                currentScreen = 1;
            }
            else if (screenCount <= 0)
            {
                screenCount = 0;
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                currentScreen = 0;
            }
            UpdateScreenSize();
            
        }else{return;}

       
    }
}

[System.Serializable]
public class ScreenSize
{
    public string ScreenStatus;
}
