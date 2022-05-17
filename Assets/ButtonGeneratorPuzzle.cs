using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonGeneratorPuzzle : MonoBehaviour
{
    public bool isActivated = true;
    [SerializeField] PuzzleGenerator m_puzzle;

    [SerializeField] private Color m_activatedColor;
    [SerializeField] private Color m_notSelectedColor;
    [SerializeField] private Animator m_myAnim;
    string m_nameAnim = "isActivate";
    string m_nameAnim_2 = "isDisable";


    private void Awake()
    {
        OnSelected();
        if(m_myAnim == null)
        {
            m_myAnim.GetComponent<Animator>();
        }
        SwitchAnim();
    }
    public void OnSelected()
    {
        if(isActivated == true)
        {
            
            gameObject.GetComponent<Renderer>().material.color = m_activatedColor;

            if (Input.GetKeyDown(KeyCode.J))
            {
                isActivated = false;
                m_puzzle.CheckSolution();
                SwitchAnim();
            }
        }
        else
        {
            gameObject.GetComponent<Renderer>().material.color = m_notSelectedColor;
            if (Input.GetKeyDown(KeyCode.J))
            {
                isActivated = true;
                m_puzzle.CheckSolution();
                SwitchAnim();
            }
        }
    }

    public void SwitchAnim()
    {
        if(isActivated == true)
        {
            m_myAnim.SetTrigger(m_nameAnim_2);
            Animator.StringToHash(m_nameAnim_2);
        }
        else
        {
            m_myAnim.SetTrigger(m_nameAnim);
            Animator.StringToHash(m_nameAnim);
        }
    }
}
