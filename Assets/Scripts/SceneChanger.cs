using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] float sceneDuration;
    [SerializeField] string sceneName;

    private void Update()
    {
        sceneDuration-=Time.deltaTime;
        if(sceneDuration <= 0)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
