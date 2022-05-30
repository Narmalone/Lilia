using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

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
    
    private Renderer m_thisRend;

    [SerializeField]
    private EventReference m_fmodEventSwitchOn;
    
    [SerializeField]
    private EventReference m_fmodEventSwitchOff;
    
    private void Awake()
    {
        m_thisRend = GetComponent<Renderer>();
        m_thisRend.material.SetFloat("_BooleanFloat", 0f);
        if (m_myAnim == null)
        {
            m_myAnim.GetComponent<Animator>();
        }
        SwitchAnim();
    }
    public void OnSelected()
    {
        if (isActivated == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                isActivated = false;
                m_puzzle.CheckSolution();
                SwitchAnim();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E))
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
            m_thisRend.material.SetFloat("_BooleanFloat", 0f);

            if (isActivated == false)
            {
                m_thisRend.material.SetColor("Color_obj", m_notSelectedColor);
            }
            else
            {
                m_thisRend.material.SetColor("Color_obj", m_activatedColor);
            }
        }
        else if(gameObject == m_puzzle.m_currentSelected)
        {
            m_thisRend.material.SetFloat("_BooleanFloat", 1f);
            m_thisRend.material.SetColor("Color_obj", m_selectedColor);
        }
    }
    public void SwitchAnim()
    {
        if(isActivated == true)
        {
            m_myAnim.SetTrigger(m_nameAnim_2);
            Animator.StringToHash(m_nameAnim_2);
            RuntimeManager.PlayOneShotAttached(m_fmodEventSwitchOff.Guid, gameObject);
        }
        else
        {
            m_myAnim.SetTrigger(m_nameAnim);
            Animator.StringToHash(m_nameAnim);
            RuntimeManager.PlayOneShotAttached(m_fmodEventSwitchOn.Guid, gameObject);
        }
    }
}
