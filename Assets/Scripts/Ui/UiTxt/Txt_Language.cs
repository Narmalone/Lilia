using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewText", menuName ="NewText", order = 4)]
public class Txt_Language : ScriptableObject
{
    [SerializeField,Tooltip("Quand c'est à 0 c'est en français à 1 c'est en anglais")] public List<string> m_Sentence;

    public int index;
}
