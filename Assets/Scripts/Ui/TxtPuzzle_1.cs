using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class TxtPuzzle_1 : MonoBehaviour
{
    [Header("Références"), Space(10)]
    [SerializeField] private QTEManager m_qte;
    [SerializeField] private TextMeshProUGUI m_thisTxt;
    [SerializeField] private AudioManagerScript m_audioScript;
    [SerializeField] private Animator m_myAnim;
    public List<Transform> m_randomPos;
    public RectTransform m_thisObj;
    public bool isNewPos = false;
    public bool isSucess = false;
    public float m_timeWhenSucess = 2f;
    string m_nameAnim = "IsPunched";
    [Header("Variables en rapport avec la croissance du text"), Space(10)]
    [SerializeField, Tooltip("Taimme maximale de la fontSize quand elle grossi")] public float m_upperFontSize;
    [SerializeField, Tooltip("Taille minime de la fontSize quand elle rappetissi")] public float m_lowerFontSize;
    [SerializeField, Tooltip("vitesse à laquelle la fontSize grandi ou rapeti en s")] public float m_speedToGrow = 5f;
    public bool m_switchToward = false;

    [SerializeField] private Animator m_txtAnimator;
    private void OnEnable()
    {
        UpdateText();
    }
    private void Update()
    {
        TouchToPress();
    }
    private void Awake()
    {
        m_thisTxt = GetComponent<TextMeshProUGUI>();
        gameObject.SetActive(false);
    }
    public void TouchToPress()
    {
        if(m_switchToward == false)
        {
            m_txtAnimator.SetTrigger("FadeIn");
            m_switchToward = true;
        }
        else
        {
            m_txtAnimator.SetTrigger("FadeOut");
            m_switchToward = false;
        }
    }
    public void UpdateText()
    {
        m_thisTxt.text = m_qte.m_keycodesQTE[m_qte.m_index].ToString();
        m_myAnim.SetBool(m_nameAnim, false);
    }

    public void SuccessQte()
    {
        if(isSucess == true)
        {
            m_myAnim.SetBool(m_nameAnim, true);
            //m_audioScript.Play("hitMeuble");
            isSucess = false;
        }
    }
    public void SetNewPosition()
    {
        isNewPos = false;
        isSucess = false;
        if (isNewPos == false)
        {
            m_thisObj.localPosition = m_randomPos[Random.Range(0, m_randomPos.Count)].transform.localPosition;
            isNewPos = true;
            isSucess = true;
            SuccessQte();
        }
        Debug.Log(m_randomPos);
    }
}
