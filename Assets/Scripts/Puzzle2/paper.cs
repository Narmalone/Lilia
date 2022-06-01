using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class paper : MonoBehaviour
{
    public List<GameObject> m_mustNotSelected;
    public List<GameObject> m_mustSelected;

    [SerializeField] private Material m_objMat;
    [SerializeField] private Color m_selectedColor;
    [SerializeField] private Color m_default;
    [SerializeField] private Color m_notSelectedColor;

    private int index = 0;
    private void Awake()
    {
        StartDisplayContent();
        m_default = Color.grey;
    }
    public void StartDisplayContent()
    {
        foreach(GameObject p_obj in m_mustSelected)
        {
            p_obj.GetComponent<Renderer>().material.color = m_default;
        }

        m_mustSelected[index].GetComponent<Renderer>().material.color = m_selectedColor;

        foreach(GameObject p_obj in m_mustNotSelected)
        {
            p_obj.GetComponent<Renderer>().material.color = m_notSelectedColor;
        }
    }
    //Fonction pour afficher la prochaine couleur
    public void NextDisplayContent()
    {
        if(m_mustSelected[m_mustSelected.Count -1])
        {
            index++;
            m_mustSelected[index].GetComponent<Renderer>().material.color = m_selectedColor;
        }
        else
        {
            Debug.Log("plus r dans le tableau");
        }

    }
}
