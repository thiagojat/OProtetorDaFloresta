using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGFXScript : MonoBehaviour
{
    
    Animator animator;
    EnemyAIHandler AIHandler;
    SpriteRenderer eyeRenderer;
    [SerializeField] Sprite[] eyes;
    private void Start()
    {
        AIHandler = transform.GetComponentInParent<EnemyAIHandler>();
        animator = GetComponent<Animator>();
        eyeRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        eyeRenderer.enabled = AIHandler.playerIsInRange;

        if (AIHandler.curState == EnemyStates.SeekingPlayer) eyeRenderer.sprite = eyes[1];
        else eyeRenderer.sprite = eyes[0];
    }

    public void ResetRotation()
    {
        transform.rotation = Quaternion.identity;
    }

    public void SetAnimDir(float curAngle)
    {
        if (animator != null) animator.SetFloat("Angle", curAngle);
        else print("animador é nulo");
    }

    internal void SetSpeed(Vector3 velocity)
    {
        animator.SetFloat("Speed", velocity.sqrMagnitude);
    }

    public void Death()
    {
        animator.SetTrigger("Dead");
    }
}


