using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NarrativeData", menuName = "NarrativeData", order = 2)]
public class CreateNarrativeEvent : ScriptableObject
{
    [Header("Variables")]
    [Tooltip("vitesse � laquelle le texte FadeIn et FadeOut par seconde"), Range(0f, 1f)]public float speedToDisplayValue;
    public bool actionComplete;
    public bool isWaitingAction;
    public bool isFirstTime;
    
    [Header("Dialog"), Space(10)]
    [Tooltip("Sentence � taper"), TextArea(3, 10)]public string[] lines;
    public int index;

    public delegate void SetValue();
    public delegate void SetBool();

    public SetValue doSetValue;
    public SetValue doSetBool;

    private void Awake()
    {
        actionComplete = false;
    }
    public void Raise()
    {
        doSetValue?.Invoke();
    }
    public void RaiseBool()
    {
        doSetValue?.Invoke();
    }
}
