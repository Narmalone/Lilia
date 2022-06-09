using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ControllerEvent : MonoBehaviour
{
    [SerializeField] private AssetMenuScriptValue m_uiEvent;
    [SerializeField] private TextMeshProUGUI m_txtToModify;

    private GameManager m_gameManager;

    [SerializeField] Color m_selectedColor;
    Color m_unselectedColor = Color.black;

    private void OnDisable()
    {
        if (m_uiEvent == null) return;
        m_uiEvent.doBool -= ChangeController;
    }
    private void OnEnable()
    {
        if (m_uiEvent == null) return;
        m_uiEvent.doBool += ChangeController;
    }
    private void Awake()
    {
        m_gameManager = FindObjectOfType<GameManager>();
        m_uiEvent.uiBool = false;
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
        Debug.Log(m_uiEvent.uiBool);
        
    }
    public void ChangeController()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            if (m_gameManager.isPc == true && m_gameManager.isGamepad == false && m_uiEvent.uiBool == false)
            {
                m_txtToModify.text = "Gamepad";
                m_gameManager.isPc = false;
                m_gameManager.isGamepad = true;
                m_uiEvent.uiBool = true;
                Debug.Log("Gamepad");
            }
            else if(m_gameManager.isPc == false && m_gameManager.isGamepad == true && m_uiEvent.uiBool == true)
            {
                m_txtToModify.text = "Pc";
                m_gameManager.isPc = true;
                m_gameManager.isGamepad = false;
                m_uiEvent.uiBool = false;
                Debug.Log("Doit afficher PC");
            } 
        }
        else
        {
            return;
        }
    }
    public void OnIncrease()
    {
        ChangeController();
    }
    public void OnDecrease()
    {
        ChangeController();
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
