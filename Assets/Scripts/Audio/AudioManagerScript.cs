using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using System;


public class AudioManagerScript : MonoBehaviour
{
    FMOD.Studio.VCA m_vcaSound;
    FMOD.Studio.VCA m_vcaMusic;
    FMOD.Studio.VCA m_vcaVoices;

    [Range(0, 1)] public float volumeSound;
    [Range(0, 1)] public float volumeMusics;
    [Range(0, 1)] public float volumeVoices;
    [SerializeField] private AssetMenuScriptValue m_sounds;
    [SerializeField] private AssetMenuScriptValue m_musics;
    [SerializeField] private AssetMenuScriptValue m_voices;
    private void Awake()
    {
        m_vcaSound = RuntimeManager.GetVCA("vca:/Sounds");
        m_vcaMusic = RuntimeManager.GetVCA("vca:/Music");
        m_vcaVoices = RuntimeManager.GetVCA("vca:/Voices");

    }
    private void Start()
    {
        SetNewVolume();
    }
    public void SetNewVolume()
    {
        //Set sounds
        volumeSound = m_sounds.value;
        m_vcaSound.setVolume(volumeSound);

        volumeMusics = m_musics.value;
        m_vcaMusic.setVolume(volumeMusics);

        volumeVoices = m_voices.value;
        m_vcaVoices.setVolume(volumeVoices);
    }
}
