using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_textComponent;
    [SerializeField, TextArea(0, 4), Tooltip("Remplir le texte")] private string[] m_lines;

    [SerializeField] private float m_speedText;
    private int m_dialogueCount;
    private int m_index;

    private bool m_changeAlpha; 

    private void Awake()
    {
        m_dialogueCount = 0;
        StartDialogue();
        m_speedText = 0.3f;
        m_changeAlpha = false;
    }
    private void Update()
    {

    }
    private void StartDialogue()
    {
        m_index = 0;
        m_textComponent.text = string.Empty;
    }
    private void SpawnDialog()
    {
        if(m_changeAlpha == false)
        {
            m_textComponent.alpha += 0.01f;
            m_changeAlpha = true;
        }
    }

    private void NextLine()
    {
        if(m_index < m_lines.Length - 1)
        {
            m_index++;
            m_textComponent.text = string.Empty;
            Debug.Log("prochaine ligne");
        }
        else
        {
            Debug.LogError("pb nextline", this);
        }
    }
    private void NextDialog()
    {
        m_dialogueCount++;
        NextLine();
        Debug.Log("doit passer au prochain dialogue");
        m_textComponent.text = m_lines[m_index];
    }
}
