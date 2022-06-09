using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class AppearThings : MonoBehaviour
{
    [Tooltip("Pour optimiser mettre un seul objet parent contenant tous les objets mais tu peux mettre tous les objets que tu le souhaites :p")] public List<GameObject> m_liliasChamber;
    [Tooltip("Pour optimiser mettre un seul objet parent contenant tous les objets mais tu peux mettre tous les objets que tu le souhaites :p")] public List<GameObject> m_sonsChamber;

    [Tooltip("Liste d'objet qui spawn une fois que le second puzzle est finis(lighting)")] public List<GameObject> m_lateGame;
    [Tooltip("Liste d'objet qui spawn une fois que le joueur est sorti(tentacules bébé et doudou au milieu de la pièce)")] public List<GameObject> m_lastAppear;
    [SerializeField] PuzzleGenerator m_puzzle;

    [SerializeField] public Transform m_iaSpawner;

    public FMODUnity.EventReference m_fmodEventTremblement;

    public bool m_isAppear = false;

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
        foreach (GameObject p_obj in m_lastAppear)
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

    public void SpawnAfterCloseDoor()
    {
        foreach (GameObject p_obj in m_lastAppear)
        {
            p_obj.SetActive(true);
        }
    }
}
