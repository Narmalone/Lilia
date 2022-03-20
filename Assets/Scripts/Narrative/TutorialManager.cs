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

    private void Awake()
    {
        m_dialogueCount = 0;
        StartDialogue();
        m_speedText = 10f * Time.deltaTime;

    }

    private void StartDialogue()
    {
        m_index = 0;
        m_textComponent.text = string.Empty;
        StartCoroutine(TypeLine());
    }
    IEnumerator TypeLine()
    {
        Debug.Log(m_textComponent.text);

        foreach (char c in m_lines[m_index].ToCharArray())
        {
            m_textComponent.text += c;
            yield return new WaitForSeconds(m_speedText);
            Debug.Log(m_textComponent.text);

        }
    }
    private void NextLine()
    {
        if(m_index < m_lines.Length - 1)
        {
            m_index++;
            m_textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
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
        StopAllCoroutines();
        m_textComponent.text = m_lines[m_index];
    }
}
