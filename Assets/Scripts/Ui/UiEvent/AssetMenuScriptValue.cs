using UnityEngine;


[CreateAssetMenu(fileName ="UiData", menuName ="UiData", order = 1)]
public class AssetMenuScriptValue : ScriptableObject
{
    public float value;
    public int CheckPoints;
    public bool uiBool;

    public enum Enumerate
    {
        _first = 0,
        _second = 1,
        _third = 2
    }

     

    //bool something
    public delegate void SetBool();

    public SetBool doBool;

    //float value
    public delegate void SetValue();

    public SetValue doSetValue;

    //EnumSomething

    public void Raise()
    {
        doSetValue?.Invoke();
    }
    public void RaiseBool()
    {
        doBool?.Invoke();
    }
}