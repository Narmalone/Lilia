using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UiManager : MonoBehaviour
{
    [SerializeField]private GameObject m_indicInteraction;
    [SerializeField]private GameObject m_UILampe;
    [SerializeField]private GameObject m_UIDoudou;

    private void Start()
    {
        m_indicInteraction.SetActive(false);
        m_UILampe.GetComponent<Image>().color = new Color32(100,100,100,255);
        m_UIDoudou.GetComponent<Image>().color = new Color32(100,100,100,255);
    }
    public void TakableObject()
    {
        m_indicInteraction.SetActive(true);
    }
    public void DisableUi()
    {
        m_indicInteraction.SetActive(false);
    }
    
    public void TakeLampe()
    {
        m_UILampe.GetComponent<Image>().color = new Color32(255,255,225,255);
    }
    
    public void DropLampe()
    {
        //D�sactiver la Ui qui prend l'objet
        //Activer l'Ui en bas � droite
        //Debug.Log("prendre l'objet");

        m_UILampe.GetComponent<Image>().color = new Color32(100,100,100,255);
    }
    
    public void TakeDoudou()
    {
        //D�sactiver la Ui qui prend l'objet
        //Activer l'Ui en bas � droite
        //Debug.Log("prendre l'objet");

        m_UIDoudou.GetComponent<Image>().color = new Color32(255,255,225,255);
    }
    
    public void DropDoudou()
    {
        //D�sactiver la Ui qui prend l'objet
        //Activer l'Ui en bas � droite
        //Debug.Log("prendre l'objet");

        m_UIDoudou.GetComponent<Image>().color = new Color32(100,100,100,255);
    }
}
