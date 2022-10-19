using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CliqueAndPlay : MonoBehaviour
{
    [SerializeField] GameObject botao;
    void Start()
    {
        Time.timeScale = 0;
    }

    public void Click()
    {
        Time.timeScale = 1f;
        botao.SetActive(false);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
