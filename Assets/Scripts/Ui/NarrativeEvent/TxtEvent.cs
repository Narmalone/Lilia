using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TxtEvent : MonoBehaviour
{
    [SerializeField, Tooltip("R�f�rence du texte que vous souhaitez afficher")] public TextMeshProUGUI m_txtToDisplay; //Texte à modifier
    [SerializeField] CreateNarrativeEvent m_createNarrativeEvent;   //référence du script narrative event
    
    private float desiredAlpha; //Alpha désiré entre 0 et 1
    private float currentAlpha; //Alpha actuelle

    private void Awake()
    {
        m_createNarrativeEvent.index = 0;
        desiredAlpha = 1;
    }
    private void Start()
    {
        StartCoroutine(DisplaySentence());
    }
    private void Update()
    {
        OnFade();
        m_txtToDisplay.alpha = currentAlpha;
    }
    
    //fonction de l'apparation/disparition
    public void OnFade()
    {
        //Si le joueur n'a pas encore complété l'action
        if (m_createNarrativeEvent.actionComplete == false)
        {
            //L'alpha va à 1
            desiredAlpha = 1;
            currentAlpha = Mathf.MoveTowards(currentAlpha, desiredAlpha, m_createNarrativeEvent.speedToDisplayValue * Time.deltaTime);
        }

        else if (m_createNarrativeEvent.actionComplete == true)
        {
            //L'alpha va à 0//
            desiredAlpha = 0;
            currentAlpha = Mathf.MoveTowards(currentAlpha, desiredAlpha,m_createNarrativeEvent.speedToDisplayValue * Time.deltaTime);
            
            if (currentAlpha <= desiredAlpha)
            {
                ClearLines();
                NextSentence();
                m_createNarrativeEvent.actionComplete = false;
            }
        }
    }
    
    //Fonction clear le texte
    public void ClearLines()
    {
        m_txtToDisplay.text = string.Empty;
    }
    
    //fonction prochaine ligne
    public void NextSentence()
    {
        if (m_createNarrativeEvent.index < m_createNarrativeEvent.lines.Length -1)
        {
            ClearLines();
            m_createNarrativeEvent.index++;
            StartCoroutine(DisplaySentence());
        }
    }

    //Coroutine de l'affichage de la ligne
    IEnumerator DisplaySentence()
    {
        m_txtToDisplay.text = m_createNarrativeEvent.lines[m_createNarrativeEvent.index];
        for (int i = 0; i < m_createNarrativeEvent.index; i++)
        {
            yield return new WaitForSeconds(.1f);
        }
    }
}
