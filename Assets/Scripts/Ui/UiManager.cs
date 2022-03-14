using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UiManager : MonoBehaviour
{
    [SerializeField]private GameObject m_takeLight;
    [SerializeField]private GameObject flashlight;

    private void Start()
    {
        m_takeLight.SetActive(false);
    }
    public void takeObject()
    {
        //Désactiver la Ui qui prend l'objet
        //Activer l'Ui en bas à droite
        //Debug.Log("prendre l'objet");

        m_takeLight.SetActive(true);

    }
    public void DisableUi()
    {
        m_takeLight.SetActive(false);
    }
}
