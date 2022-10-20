using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clarao : MonoBehaviour
{
    [SerializeField] AudioSource shotAudio;
    void Start()
    {
        shotAudio.Play();
        StartCoroutine(StartFade());
        GameStatsHandler.instance.mode = GameMode.LostGame;
    }

    private void OnDisable()
    {
        GameStatsHandler.instance.LoseGame(0);
    }
    IEnumerator Fade()
    {
        print("entrou no fade");
        for (float f = 1f; f >= 0; f -= Time.deltaTime)
        {
            Color c = GetComponent<Image>().color;
            c.a = f;
            GetComponent<Image>().color = c;
            yield return null;
        }
        gameObject.SetActive(false);
    }
    IEnumerator StartFade()
    {
        yield return new WaitForSecondsRealtime(1);
        StartFadeMethod();  
    }

    private void StartFadeMethod()
    {
        print("comec");
        StartCoroutine(Fade());
    }
}
