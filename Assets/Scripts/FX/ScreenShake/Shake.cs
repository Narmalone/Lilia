using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Shake : MonoBehaviour
{
    #region variables
    public bool camShakeActive = true; //on or off
    [SerializeField, Tooltip("Position ou le shake se lance")] private Transform m_originalPos;
    [Range(0, 1)]
    [SerializeField] float trauma;
    [SerializeField, Range(0,30), Tooltip("Puissance du Shake")] float m_shakePower = 16; //the power of the shake
    [SerializeField, Range(0,1), Tooltip("Portée du mouvement entre 0 et 1 du perlin noise")] float m_shakeRange = 0.8f; //the range of movment
    [SerializeField, Tooltip("Puissance de rotation")] float m_shakeRotation = 17f; //the rotational power
    [SerializeField, Range(0,1), Tooltip("Valeur à laquelle la profondeur se multiplue")] float m_shakeDepthMultiplier = 0.6f; //the depth multiplier
    [SerializeField, Range(0,3), Tooltip("Vitesse à laquelle le shake apparaît/disparaît")] float m_shakeDecay = 1.3f; //how quickly the shake falls off

    float timeCounter = 0; //counter stored for smooth transition
    #endregion

    #region accessors
    public float Trauma //accessor is used to keep trauma within 0 to 1 range
    {
        get
        {
            return trauma;
        }
        set
        {
            trauma = Mathf.Clamp01(value);
        }
    }
    #endregion

    #region methods
    //Get a perlin float between -1 & 1, based off the time counter.
    float GetFloat(float seed)
    {
        return (Mathf.PerlinNoise(seed, timeCounter) - 0.5f) * 2f;
    }

    //use the above function to generate a Vector3, different seeds are used to ensure different numbers
    Vector3 GetVec3()
    {
        return new Vector3(
            GetFloat(1),
            GetFloat(10),
            //deapth modifier applied here
            GetFloat(100) * m_shakeDepthMultiplier
            );
    }

    private void Awake()
    {
        camShakeActive = false;
    }

    private void Update ()
    {
        if (camShakeActive && Trauma > 0)
        {
            //increase the time counter (how fast the position changes) based off the traumaMult and some root of the Trauma
            timeCounter += Time.deltaTime * Mathf.Pow(Trauma,0.3f) * m_shakePower;
            //Bind the movement to the desired range
            Vector3 newPos = GetVec3() * m_shakeRange * Trauma;;
            transform.localPosition = newPos;
            //rotation modifier applied here
            transform.localRotation = Quaternion.Euler(newPos * m_shakeRotation);
            //decay faster at higher values
            Trauma -= Time.deltaTime * m_shakeDecay * (Trauma + 0.3f);
        }
        else
        {
            //lerp back towards default position and rotation once shake is done
            Vector3 newPos = Vector3.Lerp(m_originalPos.transform.localPosition, m_originalPos.transform.localPosition, Time.deltaTime);
            transform.localPosition = newPos;
            transform.localRotation = Quaternion.Euler(newPos * m_shakeRotation);
        }
    }
    #endregion
}
