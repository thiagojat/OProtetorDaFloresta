using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0, 360)]
    public float angle;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;

    EnemyAIHandler aIHandler;

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        aIHandler = GetComponent<EnemyAIHandler>();
        StartCoroutine(FOVRoutine());
        
    }

    private void Update()
    {
        if (canSeePlayer)
        {
            aIHandler.SeekPlayer();
        }
    }

    private IEnumerator FOVRoutine()
    {
           WaitForSeconds wait = new WaitForSeconds(0.2f);

            while (true)
            {
                yield return wait;
                FieldOfViewCheck();
            }
    }

    private void FieldOfViewCheck()
    {
        Collider2D rangeChecks = Physics2D.OverlapCircle(transform.position, radius, targetMask);

        if (rangeChecks != null)
        {
           
            Transform target = rangeChecks.transform;
            Vector2 directionToTarget = (target.position - transform.position).normalized;

            if (Vector2.Angle(transform.up, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);

                if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;
                    Debug.Log("ta vendo o player");
                }
                else
                {
                    canSeePlayer = false;
                    print("nao ta vendo o player");
                }
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }

    //private void OnDrawGizmos()
    //{   
    //    if(Vector2.Distance(playerRef.transform.position, transform.position) <= radius)
    //        Gizmos.DrawRay(playerRef.transform.position, transform.position);
    //}
}
