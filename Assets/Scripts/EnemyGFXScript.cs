using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Curupira
{

    public class EnemyGFXScript : MonoBehaviour
    {   
        SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite[] sprites;
        [SerializeField] Animator animator;

        private void Start()
        {
            //animator = GetComponent<Animator>();
            //spriteRenderer = GetComponent<SpriteRenderer>();   
        }
        public void ResetRotation()
        {   

            transform.rotation = Quaternion.identity;
        }

        public void SetAnimDir(float curAngle)
        {
            //if(curAngle > 0 && curAngle < 90)
            //{
            //    //print("cima esquerda");
            //    spriteRenderer.sprite = sprites[0];
            //}else if(curAngle >= 90 && curAngle < 180)
            //{
            //    //print("baixo esquerda");
            //    spriteRenderer.sprite = sprites[2];
            //}
            //else if(curAngle >= 180 && curAngle < 270)
            //{
            //    //print("baixo direita");
            //    spriteRenderer.sprite = sprites[3];
            //}
            //else if (curAngle >= 270 && curAngle < 360)
            //{
            //    //print("cima direita");
            //    spriteRenderer.sprite = sprites[1];
            //}
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


}
