using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
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
    [SerializeField, Tooltip("0 = activer l'interrupteur, 1 = le désactiver")] private StudioEventEmitter[] m_clip;
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
            if (Input.GetMouseButtonDown(0))
            {
                isActivated = false;
                m_puzzle.CheckSolution();
                SwitchAnim();
                m_clip[0].Play();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                isActivated = true;
                m_puzzle.CheckSolution();
                SwitchAnim();
                m_clip[1].Play();
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
            gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            gameObject.transform.localRotation = Quaternion.Euler(-180f, 0f, 0f);
        }
    }
}
