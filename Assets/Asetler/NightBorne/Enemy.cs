using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public AnaKarakter anaKarakter;
    public float health =10f;
    public float damage = 1f;
    float distance;
    public Animator animator;
    public Transform right;
    public Transform left;
    public SpriteRenderer spriteRenderer;



    public void Move()
    {
        if(distance<5f)
        {
            if(anaKarakter.transform.position.x>transform.position.x)
            {
                spriteRenderer.flipX = false;
                transform.position=Vector2.MoveTowards(transform.position, right.position, 1f*Time.deltaTime);
                animator.SetBool("Run", true);
            }
            else if(anaKarakter.transform.position.x<transform.position.x)
            {
                spriteRenderer.flipX = true;
                transform.position=Vector2.MoveTowards(transform.position, left.position, 1f*Time.deltaTime);
                animator.SetBool("Run", true);
            }
        }
        else
        {
            animator.SetBool("Run", false);
        }
    }

    public void DamageVur()
    {
        if (spriteRenderer.flipX == false)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(left.position, 2);

            if (hitEnemies != null)
            {
                foreach (Collider2D enemy in hitEnemies)
                {
                    AnaKarakter anakarakter = enemy.GetComponent<AnaKarakter>();


                    if (anakarakter != null)
                    {
                        StartCoroutine(anaKarakter.GetDamage(damage));
                    }
                }
            }
        }


        else if (spriteRenderer.flipX == true)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(right.position, 2);

            if (hitEnemies != null)
            {
                foreach (Collider2D enemy in hitEnemies)
                {
                    AnaKarakter anakarakter = enemy.GetComponent<AnaKarakter>();


                    if (anakarakter != null)
                    {
                        StartCoroutine(anaKarakter.GetDamage(damage));
                    }

                }
            }
        }
    }

    public void DamageAl(float damage)
    {
        health -= damage;
        animator.SetTrigger("GetHit");
        if (health <= 0)
        {
            animator.SetTrigger("Death");
            Destroy(gameObject, 1f);
        }
    }

    public IEnumerator Attack()
    {
        animator.SetBool("Run", false);
        if (distance <3f)
        {
            animator.SetTrigger("Attack");
        }
        yield return new WaitForEndOfFrame();
        float animlength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animlength);
    }

    void Update()
    {
        distance=Vector2.Distance(transform.position, anaKarakter.transform.position);
        Move();
        StartCoroutine(Attack());
    }
}
