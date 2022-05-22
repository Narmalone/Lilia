using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class disparais : MonoBehaviour
{
    [SerializeField] Slider m_disapear;
    [SerializeField] TextMeshProUGUI m_disapear_2;

    private void Awake()
    {
        m_disapear_2.gameObject.SetActive(false);
        m_disapear.gameObject.SetActive(false);
    }
}
