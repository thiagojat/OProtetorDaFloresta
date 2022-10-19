using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
using Random = UnityEngine.Random;

public class EnemyAIHandler : MonoBehaviour
{
    public Curupira.EnemyGFXScript enemyGFX;
    AIDestinationSetter destSetter;
    AIPath aipath;
    Seeker seeker;
    [SerializeField] private GameObject clarao;
    [SerializeField] private EnemyStates startState;
    public EnemyStates curState;
    FieldOfView fov;
    PlayerController playerController;
    Animal animal;
    private Transform playerTransform;
    private Transform animalTransform;
    GameStatsHandler gameManager;
    [SerializeField] private Transform[] randomSpots;
    [SerializeField] private int curPath;
    private int lastPath;
    [SerializeField] int maxPaths;
    [SerializeField] private float life;
    [SerializeField] private float seekVelocity;
    [SerializeField] private float huntVelocity;
    [SerializeField] private float timeSeeing;
    [SerializeField] private float maxTimeToSee;
    [SerializeField] private float timeLostPlayer;
    [SerializeField] private float maxTimeLostPlayer;

    [SerializeField] private Sprite[] statusFeedback;

    private void Start()
    {
        gameManager = GameStatsHandler.instance;
        gameManager.enemies.Add(this);
        fov = GetComponent<FieldOfView>();
        destSetter = GetComponent<AIDestinationSetter>();
        aipath = GetComponent<AIPath>();
        seeker = GetComponent<Seeker>();    

        playerController = PlayerController.instance;
        animal = Animal.instance;
        playerTransform = playerController.gameObject.transform;
        if(animal != null) animalTransform = animal.transform;

        switch (startState) 
        {
            case EnemyStates.Patroling:
                lastPath = Random.Range(0, randomSpots.Length);
                destSetter.target = randomSpots[lastPath];
                break;
            case EnemyStates.SeekingPlayer:
                destSetter.target = playerTransform;
                break;
        }
        

    }

    private void Update()
    {
        if(Vector2.Distance(gameObject.transform.position, playerTransform.position) >= fov.radius)
        {
            print("ta no raio");
            SpriteRenderer spriteRenderer = gameObject.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = true;
            spriteRenderer.sprite = GetStatusSprite();
        }

        if(curState == EnemyStates.SeekingPlayer && fov.canSeePlayer)
        {

            timeLostPlayer = 0;
            timeSeeing+=Time.deltaTime;
        }else if(curState == EnemyStates.SeekingPlayer && !fov.canSeePlayer)
        {
            timeSeeing = 0;
            timeLostPlayer += Time.deltaTime; 
        }

        if(timeSeeing > maxTimeToSee)
        {
            print("perdeu o jogo");

            clarao.SetActive(true);
            StartCoroutine(ClaraoDelay());
        }

        if(timeLostPlayer > maxTimeLostPlayer)
        {
            print("perdeu o player");
            StartPatrolling();
        }

        if(life <= 0)
        {
            Death();
        }

        if (aipath.reachedDestination)
        {
            if(curState == EnemyStates.Hunting)
            {
                gameManager.LoseGame(2);
            }else if(curState == EnemyStates.Patroling)
            {
                if(curPath == maxPaths)
                {
                    SeekAnimal();
                }
                else
                {
                    curPath++;
                    int random = Random.Range(0, randomSpots.Length);
                    if(random != lastPath)
                        destSetter.target = randomSpots[Random.Range(0, randomSpots.Length)];
                    else
                    {
                        random = Random.Range(0, randomSpots.Length);
                        destSetter.target = randomSpots[random];
                    }
                }
            }
        }
    }

    IEnumerator ClaraoDelay()
    {
        yield return new WaitForSecondsRealtime(1f);
        timeSeeing = 0;
        gameManager.LoseGame(1);
    }

    private Sprite GetStatusSprite()
    {
        if ((curState == EnemyStates.Patroling || curState == EnemyStates.Hunting) && !fov.canSeePlayer)
        {
            print("sprite olho fechado");
            aipath.maxSpeed = seekVelocity * 0.8f;
            return statusFeedback[0];
        }
        if (curState == EnemyStates.SeekingPlayer)
        {
            print("sprite olho aberto");
            aipath.maxSpeed = seekVelocity;
            return statusFeedback[1];
        }

        return statusFeedback[0];
    }

    public void StartPatrolling()
    {
        AudioManager.instance.RemoveEnemyFromList(gameObject);

        if (curState != EnemyStates.Patroling)
        {
            int random = Random.Range(0, randomSpots.Length);
            if (random != lastPath)
                destSetter.target = randomSpots[random];
            else
            {
                random = Random.Range(0, randomSpots.Length);
                destSetter.target = randomSpots[random];
            }
        }
        curState = EnemyStates.Patroling;
        aipath.maxSpeed = huntVelocity;
    }

    private void SeekAnimal()
    {
        AudioManager.instance.RemoveEnemyFromList(gameObject);
        curState = EnemyStates.Hunting;
        destSetter.target = animalTransform;
        aipath.maxSpeed = huntVelocity;
    }

    public void SeekPlayer()
    {
        AudioManager.instance.AddEnemyToList(gameObject);
        curState = EnemyStates.SeekingPlayer;
        destSetter.target = playerTransform;
        aipath.maxSpeed = seekVelocity;
    }

    public void Damage()
    {
        life--;
    }

    private void Death()
    {
        curState = EnemyStates.Dead;
        aipath.enabled = false;
        destSetter.enabled = false;
        seeker.enabled = false;
        enemyGFX.Death();

        StartCoroutine(DestroyDelay());
    }

    IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(2f);
        gameManager.enemies.Remove(this);
        Destroy(gameObject);
    }

}
    public enum EnemyStates { Patroling, Hunting, SeekingPlayer, Dead }
  
