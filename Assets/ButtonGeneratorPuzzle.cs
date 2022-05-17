using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonGeneratorPuzzle : MonoBehaviour
{
    public bool isActivated = true;
    [SerializeField] PuzzleGenerator m_puzzle;

    [SerializeField] private Color m_activatedColor;

    private void Awake()
    {
        OnSelected();
    }
    public void OnSelected()
    {
        if(isActivated == true)
        {
            
            gameObject.GetComponent<Renderer>().material.color = Color.green;
            
            if (Input.GetKeyDown(KeyCode.J))
            {
                isActivated = false;
                m_puzzle.CheckSolution();
            }
        }
        else
        {
            gameObject.GetComponent<Renderer>().material.color = Color.black;
            if (Input.GetKeyDown(KeyCode.J))
            {
                isActivated = true;
                m_puzzle.CheckSolution();
            }
        }
    }
}
