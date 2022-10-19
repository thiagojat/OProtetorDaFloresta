using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Animator animator;
    Horizontal lastHor;
    Vertical lastVer;

    Vector2 movement;

    GameStatsHandler gameManager;

    [SerializeField] private Sprite[] sprites;
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
        if (Input.GetButton("Fire1"))
        {
            animator.SetTrigger("Attack");
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
        print(movement.sqrMagnitude);
    }

    private Sprite GetSprite()
    {
        if (lastHor == Horizontal.Left && lastVer == Vertical.Top)
        {
            return sprites[0];
        }
        else if (lastHor == Horizontal.Right && lastVer == Vertical.Top)
        {
            return sprites[1];
        }
        else if (lastHor == Horizontal.Left && lastVer == Vertical.Bottom)
        {
            return sprites[2];
        }
        else if (lastHor == Horizontal.Right && lastVer == Vertical.Bottom)
        {
            return sprites[3];
        }
        else return sprites[0];
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
            print("colidiu com o inimigo");
        }
        if (Input.GetButton("Fire1") && collision.gameObject.tag == "Enemy")
        {
            print("mataaaa");

            collision.gameObject.GetComponent<EnemyAIHandler>().Damage();
        }
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
