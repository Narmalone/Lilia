using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD;
using Debug = UnityEngine.Debug;

public class Test : MonoBehaviour
{
    private FMOD.Studio.VCA VCAcontroller;
    private FMOD.Studio.VCA VCAMusic;
    private FMOD.Studio.VCA VCAVoice;
    [Range(0, 1)]
    public float volumeSounds;
    
    [Range(0, 1)]
    public float volumeVoice;

    [Range(0, 1)] public float volumeMusique;

    [SerializeField] private AssetMenuScriptValue m_sounds;
    [SerializeField] private AssetMenuScriptValue m_musics;
    [SerializeField] private AssetMenuScriptValue m_voices;
    private void Awake()
    {
        VCAcontroller = RuntimeManager.GetVCA("vca:/Sounds");
        VCAMusic = RuntimeManager.GetVCA("vca:/Music");
        VCAVoice = RuntimeManager.GetVCA("vca:/Voices");
        
        Debug.Log(VCAcontroller.isValid());
    }
    

    //fonction appelé quand au début du jeu et quand on change dans les options
    
    //A faire: Remplacer l'audio manager par ça
    //J'ai vérifié ça se se met bien l'audio mais que à partir du moment ou on a ouvert les options une fois
    
    //J'ai remplacé la fonction OnValidate car elle marche qu'en Editor-only
    public void SetNewValue()
    {
        //Set la value des sons
        volumeSounds = m_sounds.value;
        VCAcontroller.setVolume(volumeSounds);
        
        //Set la value des musiques
        volumeMusique = m_musics.value;
        VCAMusic.setVolume(volumeMusique);
        
        //Set la value des voix
        volumeVoice = m_voices.value;
        VCAVoice.setVolume(volumeVoice);
    }
    
}
