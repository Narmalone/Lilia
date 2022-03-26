using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionManager : MonoBehaviour
{

    //----------------------------------------------- Valeures dans le son ------------------------------------------//

    //SFX
    public float m_sfxValue;
    public Button m_sfxTextValue;
    public GameObject m_SfxDecrease;


    //Voice volume


    //Music volume


    private void Awake()
    {
        m_sfxValue = 10f;
        m_SfxDecrease = GameObject.Find("SfxLess");
    }
    private void Start()
    {
        m_sfxTextValue.GetComponentInChildren<TextMeshProUGUI>().text = m_sfxValue.ToString();
    }
    public void OnDecrease(float p_value, GameObject p_decrease)
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            p_value -= 5;
            p_value.ToString();
            p_decrease.gameObject.GetComponent<Image>().color = Color.cyan;
            Debug.Log("Q appuyé");
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
