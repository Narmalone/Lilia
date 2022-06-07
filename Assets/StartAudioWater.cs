using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using TMPro;
public class StartAudioWater : MonoBehaviour
{
    [SerializeField, Tooltip("0 = Eve wake up, 1 = Mathis wake up, 2 = Eve: je perd les eaux, 3 = Mathis: ok on pars, 4 = porte qui claque, 5 = voiture qui part, 6 = joueur sort du lit")] private StudioEventEmitter[] m_clips;
    [SerializeField] private TextMeshProUGUI m_introText;
    [SerializeField, TextArea(0,4)] private string[] m_textList;
    private void Start()
    {
        m_clips[0].Play();
        m_introText.text = m_textList[0];
        StartCoroutine(StartIntroCourou());
    }
    IEnumerator StartIntroCourou()
    {
        m_introText.gameObject.SetActive(true);
        yield return new WaitForSeconds(5.8f);
        m_clips[1].Play();
        m_introText.text = m_textList[1];
        yield return new WaitForSeconds(2.6f);
        m_clips[2].Play();
        m_introText.text = m_textList[2];
        yield return new WaitForSeconds(1.6f);
        m_clips[3].Play();
        m_introText.text = m_textList[3];
        yield return new WaitForSeconds(6f);
        m_clips[4].Play();
        yield return new WaitForSeconds(1f);
        m_clips[5].Play();
        m_introText.text = m_textList[4];
        yield return new WaitForSeconds(7f);
        m_introText.gameObject.SetActive(false);
        m_clips[6].Play();
    }
}
