using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearThings : MonoBehaviour
{
    [Tooltip("Pour optimiser mettre un seul objet parent contenant tous les objets mais tu peux mettre tous les objets que tu le souhaites :p")] public List<GameObject> m_liliasChamber;
    [Tooltip("Pour optimiser mettre un seul objet parent contenant tous les objets mais tu peux mettre tous les objets que tu le souhaites :p")] public List<GameObject> m_sonsChamber;
    [Tooltip("Liste d'objet qui spawn une fois que le second puzzle est finis")] public List<GameObject> m_lateGame;

    [SerializeField] private GameObject m_iaPosition;
    [SerializeField] public Transform m_iaSpawner;

    public bool m_isAppear = false;
    public bool IAdontMove = false;

    private void Awake()
    {
        m_isAppear = false;
        foreach (GameObject p_obj in m_liliasChamber)
        {
            p_obj.SetActive(false);
        } 
        foreach (GameObject p_obj in m_sonsChamber)
        {
            p_obj.SetActive(true);
        } 
        foreach (GameObject p_obj in m_lateGame)
        {
            p_obj.SetActive(false);
        }

    }
    public void SwitchAppearing()
    {
        m_isAppear = true;
        if(m_isAppear == true)
        {
            foreach(GameObject p_obj in m_liliasChamber)
            {
                p_obj.SetActive(true);
                m_iaPosition.transform.position = m_iaSpawner.transform.position;
                IAdontMove = true;
            }
            foreach (GameObject p_obj in m_sonsChamber)
            {
                p_obj.SetActive(false);
            }
        }
    }

    public void LateGameAppear()
    {
        foreach(GameObject p_obj in m_lateGame)
        {
            p_obj.SetActive(true);
        }
    }
    private void Update()
    {
        if(IAdontMove == true)
        {
            m_iaPosition.transform.position = m_iaSpawner.transform.position;
        }
    }
}
