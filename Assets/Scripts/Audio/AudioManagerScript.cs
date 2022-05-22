using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using System;

public class AudioManagerScript : MonoBehaviour
{
    public Sounds[] sounds;
    public Music[] musics;
    public Voices[] voices;
    private FirstPersonOcclusion m_occlusion;

    private void Awake()
    {
        m_occlusion = FindObjectOfType<FirstPersonOcclusion>();
    }

    public void Play(string name)
    {
        Sounds s = Array.Find(sounds, sounds => sounds.m_name == name);
        s.clip.Play();
        m_occlusion.AddInstance(s.clip.EventInstance);
    }
    public void Stop(string name)
    {
        Sounds s = Array.Find(sounds, sounds => sounds.m_name == name);
        s.clip.Stop(); 
    }
    public void SetNewValue()
    {
        foreach(Sounds s in sounds)
        {
            s.SetVolume();
        }
        foreach(Music m in musics)
        {
            m.SetVolume();
        }
        foreach(Voices v in voices)
        {
            v.SetVolume();
        }
    }
    public void PlayMusic(string name)
    {
        Music m = Array.Find(musics, musics => musics.m_name == name);
        m.clip.Play();
        m_occlusion.AddInstance(m.clip.EventInstance);
    }
    public void PlayVoices(string name)
    {
        Voices v = Array.Find(voices, voices => voices.m_name == name);
        v.clip.Play();
        m_occlusion.AddInstance(v.clip.EventInstance);
    }
}
