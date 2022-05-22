using System.Collections.Generic;
using System.Data.Common;
using FMOD;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using Debug = UnityEngine.Debug;

public class FirstPersonOcclusion : MonoBehaviour
{
    [Header("FMOD Event")]
    List<EventInstance> Audios = new List<EventInstance>();
    List<EventDescription> AudioDes = new List<EventDescription>();
    private StudioListener Listener;

    [Header("Occlusion Options")]
    [SerializeField]
    [Range(0f, 10f)]
    private float SoundOcclusionWidening = 1f;
    [SerializeField]
    [Range(0f, 10f)]
    private float PlayerOcclusionWidening = 1f;
    [SerializeField]
    private LayerMask OcclusionLayer;
    
    private float ListenerDistance;
    private float lineCastHitCount = 0f;
    private Color colour;
    private float MaxDistance;


    private void Start()
    {
        Listener = FindObjectOfType<StudioListener>();
        
    }

    public void AddInstance(EventInstance p_instance)
    {
        Debug.Log($"Je rentre dans la fonction d'add");
        Audios.Add(p_instance);
        EventDescription ya;
        p_instance.getDescription(out ya);
        AudioDes.Add(ya);
    }

    public void RemoveInstance(int index)
    {
        AudioDes.Remove(AudioDes[index]);
        Audios.Remove(Audios[index]);
    }
    
    private void FixedUpdate()
    {
        //Debug.Log($"nombre d'audio: {Audios.Count}");
        for (int i = 0; i < Audios.Count; i++)
        {
            bool virtu;
            PLAYBACK_STATE pbs;
            Audios[i].isVirtual(out virtu);
            Audios[i].getPlaybackState(out pbs);
            Audios[i].get3DAttributes(out ATTRIBUTES_3D lessgo);
            
            AudioDes[i].getMinMaxDistance(out float yoink,out MaxDistance);
            ListenerDistance = Vector3.Distance(new Vector3(lessgo.position.x,lessgo.position.y,lessgo.position.z), Listener.transform.position);

            if (!virtu && pbs == PLAYBACK_STATE.PLAYING && ListenerDistance <= MaxDistance)
                OccludeBetween(new Vector3(lessgo.position.x,lessgo.position.y,lessgo.position.z), Listener.transform.position,i);
            else if (pbs == PLAYBACK_STATE.STOPPED)
            {
                RemoveInstance(i);
                Debug.Log($"je remove un son");
            }
            lineCastHitCount = 0f;
        }
    }

    private void OccludeBetween(Vector3 sound, Vector3 listener,int index)
    {
        
        Vector3 SoundLeft = CalculatePoint(sound, listener, SoundOcclusionWidening, true);
        Vector3 SoundRight = CalculatePoint(sound, listener, SoundOcclusionWidening, false);

        Vector3 SoundAbove = new Vector3(sound.x, sound.y + SoundOcclusionWidening, sound.z);
        Vector3 SoundBelow = new Vector3(sound.x, sound.y - SoundOcclusionWidening, sound.z);

        Vector3 ListenerLeft = CalculatePoint(listener, sound, PlayerOcclusionWidening, true);
        Vector3 ListenerRight = CalculatePoint(listener, sound, PlayerOcclusionWidening, false);

        Vector3 ListenerAbove = new Vector3(listener.x, listener.y + PlayerOcclusionWidening * 0.5f, listener.z);
        Vector3 ListenerBelow = new Vector3(listener.x, listener.y - PlayerOcclusionWidening * 0.5f, listener.z);

        CastLine(SoundLeft, ListenerLeft);
        CastLine(SoundLeft, listener);
        CastLine(SoundLeft, ListenerRight);

        CastLine(sound, ListenerLeft);
        CastLine(sound, listener);
        CastLine(sound, ListenerRight);

        CastLine(SoundRight, ListenerLeft);
        CastLine(SoundRight, listener);
        CastLine(SoundRight, ListenerRight);
        
        CastLine(SoundAbove, ListenerAbove);
        CastLine(SoundBelow, ListenerBelow);

        if (PlayerOcclusionWidening == 0f || SoundOcclusionWidening == 0f)
        {
            colour = Color.blue;
        }
        else
        {
            colour = Color.green;
        }

        SetParameter(index);
    }

    private Vector3 CalculatePoint(Vector3 a, Vector3 b, float m, bool posOrneg)
    {
        float x;
        float z;
        float n = Vector3.Distance(new Vector3(a.x, 0f, a.z), new Vector3(b.x, 0f, b.z));
        float mn = (m / n);
        if (posOrneg)
        {
            x = a.x + (mn * (a.z - b.z));
            z = a.z - (mn * (a.x - b.x));
        }
        else
        {
            x = a.x - (mn * (a.z - b.z));
            z = a.z + (mn * (a.x - b.x));
        }
        return new Vector3(x, a.y, z);
    }

    private void CastLine(Vector3 Start, Vector3 End)
    {
        Vector3 direction =End - Start;
        Ray ray = new Ray(Start, direction);
        RaycastHit[] hit;
        hit  = Physics.RaycastAll(ray, Vector3.Distance(Start,End), OcclusionLayer);
        //Debug.DrawRay(Start, direction);
        int maxCollider = 0;
        if (hit.Length > 0)
        {
            for (int i = 0; i < hit.Length;i++)
            {
                if(MaxDistance < hit[i].distance)
                    Debug.Log($"Maxdistance : {MaxDistance} distance du hit : {hit[i].distance}");
                
                if (maxCollider < 3)
                {
                    maxCollider++;
                    lineCastHitCount++;
                }
            }

            switch (maxCollider)
            {
               case 1:
                   Debug.DrawLine(Start, End, Color.yellow);
                   break;
               
               case 2:
                   Debug.DrawLine(Start, End, new Color(1f, 0.5f, 0f));
                   break;
               
               case 3 :
                   Debug.DrawLine(Start, End, Color.red);
                   break;
            }
        }
        else Debug.DrawLine(Start, End, colour);
    }

    private void SetParameter(int index)
    {
        
        Audios[index].setParameterByName("Occlusion", lineCastHitCount / 33);
    }
}