using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScripts : MonoBehaviour
{
    [SerializeField] private float m_speedToUp = 1f;
    [SerializeField] private RectTransform m_thisTransform;
    private void Update()
    {
        if(gameObject.transform.localPosition.y < 4286)
        {
            gameObject.transform.position -= new Vector3(0f, -m_speedToUp, 0f) * Time.deltaTime;
            Debug.Log("au dessous de 4286");
        }

    }
}
