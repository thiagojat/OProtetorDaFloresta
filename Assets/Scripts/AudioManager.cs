using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource[] audioSources;

    [SerializeField] List<GameObject> persecutionList = new List<GameObject>();
    void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this; 
        DontDestroyOnLoad(gameObject);
    }

    public void AddEnemyToList(GameObject enemy)
    {
        if (!persecutionList.Contains(enemy))
        {
            if(persecutionList.Count == 0)
            {
                audioSources[1].Play();
            }
            persecutionList.Add(enemy);
        }
    }

    public void RemoveEnemyFromList(GameObject enemy)
    {
        if (persecutionList.Contains(enemy))
        {
            persecutionList.Remove(enemy);
            if (persecutionList.Count == 0)
            {
                audioSources[1].Stop();
            }
        }
    }

    void Start()
    {
        audioSources[0].Play();
    }

    public void SwitchMusic(AudioSources newAudio)
    {
        foreach(AudioSource source in audioSources)
        {
            source.Stop();
        }
        audioSources[(int)newAudio].Play();
    }


}

public enum AudioSources{ AmbienceAudio, PersecutionAudio };
