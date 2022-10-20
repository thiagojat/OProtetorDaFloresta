using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameStatsHandler : MonoBehaviour
{
    #region Singleton
    public static GameStatsHandler instance;

    void Awake()
    {
        instance = this;
    }
    #endregion

    public GameObject[] UIPanels;

    public List<EnemyAIHandler> enemies;
    public GameMode mode;
    [SerializeField] private string lastSceneName;

    private void LateUpdate()
    {
        if (enemies.Count <= 0)
        {
            WonGame();
        }

        CheckEnemyList();
    }

    private void CheckEnemyList()
    {
        if (enemies.Count > 0)
        {
            foreach (EnemyAIHandler enemy in enemies)
            {
                if(enemy == null) enemies.Remove(enemy);
            }

        }
    }

    public void LoseGame(int i)
    {
        //i = 1; player foi morto
        //i = 2; lobo foi alcançãdo
        LostGame(i);
    }

    private void LostGame(int i)
    {
        print("lostgame");
        mode = GameMode.LostGame;
        UIPanels[i].SetActive(true);
        Time.timeScale = 0;
    }

    private void WonGame()
    {
        if (SceneManager.GetActiveScene().name == lastSceneName)
        {
            mode = GameMode.WonGame;
            int i = 0;
            if (SceneManager.GetActiveScene().name == "Fase2") i = 1; else i = 2;
            UIPanels[i].SetActive(true);
            Time.timeScale = 0f;

        }
        else
        {
            NextLevel();
        }
    }

    private void NextLevel()
    {
        StartCoroutine(DelayBeetwenScenes());
    }

    IEnumerator DelayBeetwenScenes()
    {
        print("esperando delay");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}

public enum GameMode { Menu, Normal, LostGame, WonGame }
