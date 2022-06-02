using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
public class EventVolume : MonoBehaviour
{
    public Volume volume;
    Vignette vignette;
    [SerializeField] private PlayerController m_player;
    [SerializeField] private Transform m_playerTr;
    [SerializeField] private Transform m_setPlayerPos;
    [SerializeField] private Doudou m_doudou;
    public bool isPlayerAwake = false;

    //[SerializeField] Image

    [Range(0,7)] public float m_maxValue = 1f;
    [Range(0, 5)] public float m_minValue = 0f;
    private float m_currentValue;
    [Range(0,10)] public float m_Speed;
    private int m_currentNB;
    [SerializeField] private int m_nbMax;
    private bool isOpen = false;
    private bool isOver = false;

    [SerializeField] private Animator m_imgBlikImage;
    private void Awake()
    {
        m_currentNB = 0;
        if (volume.profile.TryGet<Vignette>(out vignette))
        {
            vignette.intensity.max = m_maxValue;
        }
        isPlayerAwake = false;
    }
    private void Start()
    {
        if(isPlayerAwake == false)
        {
            //m_player.GetComponent<Animator>().SetTrigger("BeforeAwake");
            //isPlayerAwake = true;
            //StartCoroutine(NextCinematic());
        }
    }
    IEnumerator NextCinematic()
    {
        yield return new WaitForSeconds(10f);
        AwakePlayer();
    }
    IEnumerator EndCinematic()
    {
        yield return new WaitForSeconds(.1f);
        ResetTrigger();
    }
    public void AwakePlayer()
    {
        StopCoroutine(NextCinematic());
        m_player.GetComponent<Animator>().SetTrigger("AwakePlayer");
        StartCoroutine(EndCinematic());
    }
    public void ResetTrigger()
    {
        StopCoroutine(EndCinematic());
        m_player.GetComponent<Animator>().SetTrigger("Reset");
    }
    void Update()
    {
        ClignementDesYeux();
    }
    public void ClignementDesYeux()
    {
        if (isOver == false)
        {
            if (isOpen == false)
            {
                m_currentValue = vignette.intensity.value;
                vignette.intensity.value = Mathf.MoveTowards(m_currentValue, m_maxValue, m_Speed * Time.deltaTime);

                if (m_currentValue >= m_maxValue)
                {
                    isOpen = true;
                }
            }
            else if (isOpen == true)
            {
                m_currentValue = vignette.intensity.value;
                vignette.intensity.value = Mathf.MoveTowards(m_currentValue, m_minValue, m_Speed * Time.deltaTime);
                if (m_currentValue <= m_minValue)
                {
                    isOpen = false;
                    m_currentNB++;
                }
                if (m_currentNB >= m_nbMax)
                {
                    m_player.isCinematic = false;
                    isOver = true;
                }
            }
        }
        else
        {
            return;
        }
    }

    public void FadeInFadeOut()
    {
        m_imgBlikImage.SetBool("FadeActive", true);
    }
    public void TriggerScreamer()
    {

    }
}
