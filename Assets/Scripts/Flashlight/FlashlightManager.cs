using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightManager : MonoBehaviour
{
    UiManager uiManager;

    [SerializeField, Tooltip("Référence de la torche")]private GameObject flashlight;

    private void Awake()
    {
        flashlight.GetComponent<BoxCollider>();
    }
    private void Update()
    {
        
    }

    private void OnTriggerStay(Collider p_collide)
    {
        uiManager.takeObject();
    }
}
