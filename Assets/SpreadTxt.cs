using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class SpreadTxt : MonoBehaviour
{
    [SerializeField] private LayerMask m_playerMask;
    [SerializeField] private TextMeshProUGUI m_txtToDisplay;
    [SerializeField] private PlayerController m_player;
    [SerializeField, TextArea(0, 4)] private string m_txt;

    private void Awake()
    {
        m_txtToDisplay.gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((m_playerMask.value & (1 << other.gameObject.layer)) > 0)
        {
            if(m_player.hasChair == false)
            {
                m_txtToDisplay.gameObject.SetActive(true);
                m_txtToDisplay.text = m_txt;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((m_playerMask.value & (1 << other.gameObject.layer)) > 0)
        {
            m_txtToDisplay.gameObject.SetActive(false);
        }
    }
}
