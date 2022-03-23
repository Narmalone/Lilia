using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StressManager : MonoBehaviour
{
    [SerializeField, Tooltip("Stress actuel du personnage")]private float m_currentStress;
    [SerializeField, Tooltip("Augmentation du stress du joueur")] private float m_upgradeStress;

    private bool isCinematic;

    private void Start()
    {
        isCinematic = false;
    }
    private void Update()
    {
        if (isCinematic == true)
        {
            m_upgradeStress = 0;
        }
        else
        {
            m_currentStress += m_upgradeStress;
            m_upgradeStress = 0.001f;
        }
        //Debug.Log(m_currentStress);
    }
}
