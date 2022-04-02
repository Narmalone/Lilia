using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Event", menuName = "EventCreator")]

public class Event1 : ScriptableObject
{
    public delegate void TriggeredDelegate(Vector3 p_position);

    public event TriggeredDelegate onTriggered;

    public void Raise(Vector3 p_pos)
    {
        onTriggered?.Invoke(p_pos);
    }
}
