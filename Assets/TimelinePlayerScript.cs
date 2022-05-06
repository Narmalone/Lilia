using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelinePlayerScript : MonoBehaviour
{
    [SerializeField] private PlayableDirector m_director;
    [SerializeField] private PlayerController m_player;
    public bool isActivated = false;
    public float incrementDelta;
    private void Awake()
    {
        incrementDelta = 0f;
    }
    private void Update()
    {
        Chronometre();
    }
    public void StartTimeline()
    {
        m_director.Play();
        isActivated = true;
    }
    public void Chronometre()
    {
        if(isActivated == true)
        {
            if (incrementDelta < m_director.duration)
            {
                incrementDelta += 1f * Time.deltaTime;
                m_player.m_speed = 0f;
            }
            else
            {
                m_director.Stop();
                m_player.m_speed = 2f;
            }
            Debug.Log(m_director.time);
        }
        else { return; }
       
    }
}
