using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD;
using Debug = UnityEngine.Debug;

[System.Serializable]
public class Sounds : MonoBehaviour
{
    public StudioEventEmitter clip;
    public string m_name;
    public string VcaName;
    [FMODUnity.ParamRef]
    FMOD.Studio.EventInstance m_eventInstance;
    [SerializeField] private AssetMenuScriptValue m_uiEvent;
    private FMOD.Studio.VCA VCAcontroller;
    [Range(0, 1)]
    public float volume;

    private void Awake()
    {
        VCAcontroller = FMODUnity.RuntimeManager.GetVCA("vca:/" + VcaName);
    }
    public void SetVolume()
    {
        volume = m_uiEvent.value;
        VCAcontroller.setVolume(volume);
    }
}
