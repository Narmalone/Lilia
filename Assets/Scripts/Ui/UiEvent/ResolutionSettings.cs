using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ResolutionSettings : MonoBehaviour
{
    [SerializeField] private AssetMenuScriptValue m_uiEvent;
    [SerializeField] private TextMeshProUGUI m_txtToModify;
    [SerializeField] private ScreenStatus m_scriptScreenStatus;

    [SerializeField] Color m_selectedColor;
    Color m_unselectedColor = Color.black;
    
    public List<ResolutionTab> resolution = new List<ResolutionTab>();
    public int indexResolution;
    
    public void SetSelectedColor()
    {
        m_txtToModify.color = m_selectedColor;

    }
    public void SetUnselectedColor()
    {
        m_txtToModify.color = m_unselectedColor;

    }

    private void OnDisable()
    {
        m_uiEvent.doBool -= UpdateResolution;
    }

    private void OnEnable()
    {
        m_uiEvent.doBool += UpdateResolution;
    }

    private void Awake()
    {
        UpdateResolution();
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

    public void UpdateResolution()
    {
        m_txtToModify.GetComponent<TextMeshProUGUI>().text = resolution[indexResolution].width.ToString() + "X" +
                             resolution[indexResolution].height.ToString();
        if (m_scriptScreenStatus.currentScreen == 0)
        {
            Screen.SetResolution(resolution[indexResolution].width, resolution[indexResolution].height, FullScreenMode.FullScreenWindow);
        }
        else if (m_scriptScreenStatus.currentScreen == 1)
        {
            Screen.SetResolution(resolution[indexResolution].width, resolution[indexResolution].height, FullScreenMode.MaximizedWindow);
        }
        else if (m_scriptScreenStatus.currentScreen == 2)
        {
            Screen.SetResolution(resolution[indexResolution].width, resolution[indexResolution].height, FullScreenMode.Windowed);
        }
    }

    public void OnIncrease()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            indexResolution++;
            if (indexResolution > resolution.Count - 1)
            {
                indexResolution = resolution.Count -1;
            }

            UpdateResolution();
        }else{return;}
    }

    public void OnDecrease()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            indexResolution--;
            if (indexResolution <= 0)
            {
                indexResolution = 0;
            }
            UpdateResolution();
        }else{return;}
        
    }
} 



[System.Serializable]
public class ResolutionTab
{
    public int width, height;
}
