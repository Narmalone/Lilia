using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelinePlayerScript : MonoBehaviour
{
    [SerializeField] private List<PlayableDirector> m_director;
    [SerializeField] private PlayerController m_player;
    public List<TimelineAsset> m_timelines;
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
    public void StartTimeline(int index)
    {
        TimelineAsset selectedAsset;
        if(m_timelines.Count <= index)
        {
            selectedAsset = m_timelines[m_timelines.Count - 1];
        }
        else
        {
            selectedAsset = m_timelines[index];
        }
        m_director[0].Play(selectedAsset);
        isActivated = true;
    }
    public void Chronometre()
    {
        if(isActivated == true)
        {
            if (incrementDelta < m_director[0].duration)
            {
                incrementDelta += 1f * Time.deltaTime;
                m_player.m_speed = 0f;
            }
            else
            {
                m_director[0].Stop();
                m_player.m_speed = 2f;
            }
        }
        else { return; }
       
    }
}
