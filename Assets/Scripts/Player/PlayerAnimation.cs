using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator m_animator;
    
    [NonSerialized]
    public bool m_stoppedMoving;
    
    public AnimationClip m_animation;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!m_animator) m_animator = GetComponent<Animator>();
        m_animator.Play("WakeUp");
        m_animation = m_animator.GetCurrentAnimatorClipInfo(0)[0].clip;
        m_stoppedMoving = true;
        StartCoroutine(StartMoving());
    }

    public IEnumerator StartMoving()
    {
        yield return new WaitForSeconds(m_animation.length);
        m_stoppedMoving = false;
    }
}
