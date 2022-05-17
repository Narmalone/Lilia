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


    private void Awake()
    {
        OnSelected();
        if(m_myAnim == null)
        {
            m_myAnim.GetComponent<Animator>();
        }
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
                m_myAnim.SetBool(m_nameAnim, true);
                Animator.StringToHash(m_nameAnim);
            }
        }
        else
        {
            gameObject.GetComponent<Renderer>().material.color = m_notSelectedColor;
            if (Input.GetKeyDown(KeyCode.J))
            {
                isActivated = true;
                m_myAnim.SetBool(m_nameAnim, false);
                m_puzzle.CheckSolution();
            }
        }
    }
}
