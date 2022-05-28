using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonGeneratorPuzzle : MonoBehaviour
{
    public bool isActivated = true;
    [SerializeField] PuzzleGenerator m_puzzle;

    [SerializeField] private Color m_activatedColor;
    [SerializeField] private Color m_notSelectedColor;
    [SerializeField] private Color m_selectedColor;
    [SerializeField] private Animator m_myAnim;
    string m_nameAnim = "isActivate";
    string m_nameAnim_2 = "isDisable";

    Renderer m_thisRend;
    private void Awake()
    {
        m_thisRend = GetComponent<Renderer>();
        m_thisRend.material.SetFloat("_BooleanFloat", 1f);
        if (m_myAnim == null)
        {
            m_myAnim.GetComponent<Animator>();
        }
        SwitchAnim();
        OnSelected();
    }
    public void OnSelected()
    {
        if (isActivated == true)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                isActivated = false;
                m_puzzle.CheckSolution();
                SwitchAnim();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                isActivated = true;
                m_puzzle.CheckSolution();
                SwitchAnim();
            }
        }
    }
    private void LateUpdate()
    {
        if(gameObject != m_puzzle.m_currentSelected)
        {
            if (isActivated == false)
            {
                m_thisRend.material.SetColor("Color_Interaction", m_notSelectedColor);
            }
            else
            {
                m_thisRend.material.SetColor("Color_Interaction", m_activatedColor);
            }
        }
        else if(gameObject == m_puzzle.m_currentSelected)
        {
            m_thisRend.material.SetColor("Color_Interaction", m_selectedColor);
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
