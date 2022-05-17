using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonGeneratorPuzzle : MonoBehaviour
{
    public bool isActivated = true;
    [SerializeField] PuzzleGenerator m_puzzle;

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
            if (Input.GetKeyDown(KeyCode.J))
            {
                isActivated = true;
                m_puzzle.CheckSolution();
            }
        }
    }
}
