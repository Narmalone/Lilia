using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UiManager : MonoBehaviour
{
    [SerializeField]private GameObject m_takeLight;

    [SerializeField]private GameObject flashlight;

    public bool isHandle = false;
    public bool isActivated;

    private void Start()
    {
        DisableUi();
    }
    public void takeObject()
    {
        isHandle = false;
        if(isHandle == false)
        {
            m_takeLight.SetActive(true);
            Debug.Log(isHandle);
        }
        else
        {
            DisableUi();
        }
    }
    public void DisableUi()
    {
        isHandle = true;
        if (isHandle == true)
        {
            m_takeLight.SetActive(false);
            Debug.Log(isHandle);
        }
        else
        {
            Debug.LogError("Problème DisableUi ?", this);
        }
        
    }
}
