using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class AudioTV : MonoBehaviour
{
    [SerializeField]
    private EventReference m_fmodEventStress;

    private FirstPersonOcclusion m_occlusion;
    
    // Start is called before the first frame update
    void Start()
    {
        m_occlusion = FindObjectOfType<FirstPersonOcclusion>();
        EventInstance m_fmodInstancePas = RuntimeManager.CreateInstance(m_fmodEventStress);
        RuntimeManager.AttachInstanceToGameObject(m_fmodInstancePas, GetComponent<Transform>());
        m_fmodInstancePas.start();
        m_occlusion.AddInstance(m_fmodInstancePas);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
