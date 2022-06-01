using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    [SerializeField] AudioManagerScript m_audio;
    [SerializeField] private LayerMask m_player;

    public bool playAssiette = false;
    private void OnTriggerEnter(Collider other)
    {
        if ((m_player.value & (1 << other.gameObject.layer)) > 0)
        {
            if(playAssiette == true)
            {
                PlayAssiette();
            }
        }
    }
    public void PlayAssiette()
    {
        //m_audio.Play("Assiette");
    }
}
