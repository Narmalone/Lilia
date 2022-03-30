using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class OptionManager : MonoBehaviour
{

    //----------------------------------------------- Valeures dans le son ------------------------------------------//

    //SFX
    public float m_sfxValue;
    public TextMeshProUGUI m_sfxTextValue;
    public GameObject m_SfxDecrease;


    private Color m_selectedColor = Color.yellow;
    private Color m_unselectedColor = Color.white;


    private void Awake()
    {
        m_sfxValue = 10f;
        m_SfxDecrease = GameObject.Find("SfxLess");
    }
    private void Start()
    {
        m_sfxTextValue.GetComponent<TextMeshProUGUI>().text = m_sfxValue.ToString();
        m_sfxTextValue.GetComponent<TextMeshProUGUI>().color = m_selectedColor;
    }

    private void Update()
    {
        OnDecrease();
    }

    public void OnDecrease()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            
        }
    }

    public void OnIncrease(float p_value, Image p_increase)
    {
        p_value += 5;
        p_value.ToString();
    }

    public void Salut()
    {

    }
}
