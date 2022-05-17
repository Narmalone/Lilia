using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testanim : MonoBehaviour
{
    [SerializeField] private Animator m_animatorPourTest;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            m_animatorPourTest.SetTrigger("activate");
        }
    }
}
