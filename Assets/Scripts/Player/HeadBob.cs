using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [SerializeField]
    private float m_hauteur;

    private float m_goal;
    
    private float m_frequence;

    private float m_hauteurBase;

    [SerializeField]
    private AnimationCurve m_curve;

    private float time;


    // Start is called before the first frame update
    void Start()
    {
        time = 0f;
        m_hauteurBase = transform.localPosition.y;
        StartCoroutine(HeadBobbing(1));
        Debug.Log($"test de valeur de mort {Mathf.Lerp(10,5,0.5f)}");
    }

    // Update is called once per frame
    void Update()
    {
        time+=Time.deltaTime;
        var rajoutHauteur = 0f;
        if (m_goal>0)
        {
            //rajoutHauteur = Mathf.Lerp(transform.localPosition.y,m_hauteurBase+m_goal,m_curve.Evaluate(time/m_frequence));
        }
        else
        {
            //rajoutHauteur = Mathf.Lerp(transform.localPosition.y,m_hauteurBase+m_goal,m_curve.Evaluate(1f - time/m_frequence));
            Debug.Log($"test de valeur de mort {m_curve.Evaluate(time/(m_frequence/2))}");
        }
        
        
        
        transform.localPosition= new Vector3(transform.localPosition.x,rajoutHauteur,transform.localPosition.z);

    }

    public IEnumerator HeadBobbing(float p_frequence)
    {
        while (true)
        {
            time = 0f;
            m_goal = m_hauteur;
            yield return new WaitForSeconds(p_frequence/2);
            time = 0f;
            m_goal = 0;
            yield return new WaitForSeconds(p_frequence/2);
        }
    }
}
