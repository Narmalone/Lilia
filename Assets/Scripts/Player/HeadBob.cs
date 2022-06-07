using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [SerializeField]
    private float m_hauteur;

    private float m_goal;

    private PlayerController m_playerController;
    
    [NonSerialized]
    public float m_frequence;

    private float m_hauteurBase;

    [SerializeField]
    private AnimationCurve m_curve;

    [SerializeField]
    private AnimationCurve m_curve2;
    
    private float time;
    private bool m_changedValue;
    private float m_currentFrequence;

    // Start is called before the first frame update
    void Start()
    {
        m_playerController = FindObjectOfType<PlayerController>();
        time = 0f;
        m_hauteurBase = transform.localPosition.y;
        StartCoroutine(HeadBobbing());
        Debug.Log($"test de valeur de mort {Mathf.Lerp(10,5,0.5f)}");
        m_changedValue = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_playerController.velocity < 0.1)
        {
            StopCoroutine(HeadBobbing());
        }
        //m_currentFrequence = Mathf.Clamp(1 / m_playerController.velocity,0.01f,1);
        else
        {
            if (!m_changedValue)
            {
                m_frequence = 1 / m_playerController.velocity;
                StartCoroutine(HeadBobbing());
                m_changedValue = true;
                time = 0.001f;
            }
            UpdatePosition();
        }
    }

    private void UpdatePosition()
    {
        Debug.Log(m_frequence);
        time+=Time.deltaTime;
        var rajoutHauteur = 0f;
        
        if(m_goal == 0)
        {
            rajoutHauteur = Mathf.Lerp(m_hauteurBase,m_hauteurBase+m_hauteur,m_curve.Evaluate(Mathf.Clamp(time/(m_frequence/2),0,1)));
            //Debug.Log($"test de valeur de mort {m_hauteurBase}");
        }
        else if (m_goal>0)
        {
            rajoutHauteur = Mathf.Lerp(m_hauteurBase,m_hauteurBase+m_hauteur,m_curve2.Evaluate(Mathf.Clamp(time/(m_frequence/2),0,1)));
        }
                
        transform.localPosition= new Vector3(transform.localPosition.x,rajoutHauteur,transform.localPosition.z);
    }
    
    public IEnumerator HeadBobbing()
    {
        Debug.Log("Je suis dans la coroutine");
        time = 0.001f;
        m_goal = m_hauteur;
        yield return new WaitForSeconds(m_frequence/2);
        
        time = 0.001f;
        m_goal = 0;
        yield return new WaitForSeconds(m_frequence/2);
        m_changedValue = false;
    }
}
