using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
using Random = UnityEngine.Random;

public class EnemyAIHandler : MonoBehaviour
{
    EnemyGFXScript enemyGFX;
    public bool playerIsInRange;

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
    private float life = 1;
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
        enemyGFX = GetComponentInChildren<EnemyGFXScript>();

        playerController = PlayerController.instance;
        animal = Animal.instance;
        playerTransform = playerController.gameObject.transform;
        if(animal != null) animalTransform = animal.transform;

        switch (startState) 
        {
            case EnemyStates.Patroling:
                lastPath = GetRandomPath();
                destSetter.target = randomSpots[lastPath];
                break;
            case EnemyStates.SeekingPlayer:
                destSetter.target = playerTransform;
                break;
        }
        

    }

    private int GetRandomPath()
    {
        return Random.Range(0, randomSpots.Length);
    }

    private void Update()
    {
        aipath.enabled = (gameManager.mode == GameMode.Normal);
        playerIsInRange = (Vector2.Distance(gameObject.transform.position, playerTransform.position) <= fov.radius);

        if (curState == EnemyStates.SeekingPlayer)
        {
            if (fov.canSeePlayer)
            {
                //start counting how many seconds the enemy is seeing the player
                timeLostPlayer = 0;
                timeSeeing += Time.deltaTime;
            }
            else
            {
                //start counting how many seconds the enemy has lost the player
                timeSeeing = 0;
                timeLostPlayer += Time.deltaTime;
            }
        } 
        if(timeSeeing > maxTimeToSee)
        {
            KillPlayer();
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
                    int random = GetRandomPath();
                    if(random != lastPath)
                    {
                        destSetter.target = randomSpots[random];
                    }
                    else
                    {
                        random = GetRandomPath();
                        destSetter.target = randomSpots[random];
                    }
                    lastPath = random;
                }
            }
        }
    }

    private void KillPlayer()
    {
        clarao.SetActive(true);
        playerController.Death();
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
        AudioManager.instance.RemoveEnemyFromList(gameObject);
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
  
