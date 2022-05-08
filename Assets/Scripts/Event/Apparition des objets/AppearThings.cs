using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearThings : MonoBehaviour
{
    [Tooltip("Pour optimiser mettre un seul objet parent contenant tous les objets mais tu peux mettre tous les objets que tu le souhaites :p")]public List<GameObject> m_objList;

    public bool m_isAppear = false;

    private void Awake()
    {
        m_isAppear = false;
        foreach (GameObject p_obj in m_objList)
        {
            p_obj.SetActive(false);
        }
    }
    private void Update()
    {
        SwitchAppearing();
    }
    public void SwitchAppearing()
    {
        if(m_isAppear == true)
        {
            foreach(GameObject p_obj in m_objList)
            {
                p_obj.SetActive(true);
            }
        }
    }
}
