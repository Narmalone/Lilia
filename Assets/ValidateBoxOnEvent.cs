using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ValidateBoxOnEvent : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI m_txtToDisplay;
    [SerializeField] private LayerMask m_MaskReceiver;
    private void OnTriggerEnter(Collider other)
    {
        if ((m_MaskReceiver.value & (1 << other.gameObject.layer)) > 0)
        {
            m_txtToDisplay.gameObject.SetActive(true);
            Debug.Log("un event a été trigger ?");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        
    }
}
