using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    EnemyAIHandler targetEnemy;

    [SerializeField] private float moveSpeed = 5f;
    SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Animator animator;
    Horizontal lastHor;
    Vertical lastVer;

    Vector2 movement;

    GameStatsHandler gameManager;
    Vector2 oldMovement;

    #region Singleton
    public static PlayerController instance;

    void Awake()
    {
        instance = this;
    }
    #endregion
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        SetMovement();

        //attack
        if (Input.GetButton("Fire1"))
        {
            animator.SetTrigger("Attack");
            if (targetEnemy != null)
            {
                targetEnemy.Damage();
            }
        }

    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    void SetMovement()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        if (movement.x != 0)
        {
            oldMovement.x = movement.x;
        }
        if (movement.y != 0)
        {
            oldMovement.y = movement.y;
        }


        animator.SetFloat("Horizontal", oldMovement.x);
        animator.SetFloat("Vertical", oldMovement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }


    private void getLastPosition(Vector2 movement)
    {

        if (movement.x > 0)
        {
            lastHor = Horizontal.Right;
        }
        if (movement.x < 0)
        {
            lastHor = Horizontal.Left;
        }
        if (movement.y > 0)
        {
            lastVer = Vertical.Top;
        }
        if (movement.y < 0)
        {
            lastVer = Vertical.Bottom;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            targetEnemy = collision.gameObject.GetComponent<EnemyAIHandler>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            targetEnemy = null;
        }
    }

    public void Death()
    {
        animator.SetTrigger("Dead");
    }
}
public enum Horizontal
{
    Right,
    Left
}

public enum Vertical
{
    Bottom,
    Top
}
