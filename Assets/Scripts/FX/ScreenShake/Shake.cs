using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Shake : MonoBehaviour
{
    [SerializeField]private float m_shakeTimeRemaining, m_shakePower;

    private Vector3 m_startingPosition;

    private void Awake()
    {
        m_startingPosition = transform.localPosition;
    }

    private void LateUpdate()
    {
        if (m_shakeTimeRemaining > 0)
        {
            m_shakeTimeRemaining -= Time.deltaTime;
            float xAmount = Random.Range(-1f, 1f) * m_shakePower;
            float yAmount = Random.Range(-1f, 1f) * m_shakePower;
            //transform.localPosition += 
            transform.localPosition= m_startingPosition + new Vector3(xAmount, yAmount, 0f);
        }
        else
        {
            transform.localPosition = m_startingPosition;
        }
    }

    public void StartShake(float p_length, float p_power)
    {
        m_shakeTimeRemaining = p_length;
        m_shakePower = p_power;
    }
}
