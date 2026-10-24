using Google.Protobuf.WellKnownTypes;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AnaKarakter : MonoBehaviour
{


    public Rigidbody2D move;
    public Animator animator;
    public int hiz = 5;
    public float damage=1f;
    public float health=10f;
    public float jumpforce = 12f;
    public SpriteRenderer sprite;
    public float x;
    public bool isGrounded = true;
    public Boss boss;
    public SpriteRenderer spriteRenderer;
    public Transform vurusyeri;
    public Transform vurusyerireverse;
    public LayerMask EnemyLayer;
    public LayerMask PlayerLayer;
    public Collider2D kilic;
    public Collider2D reversekilic;
    public GameObject King;
    public Slider slider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("AOE"))
        {
            GetDamage(boss.damage * 2);
        }

       
    }



    public IEnumerator HasarAlma()
    {
        King.layer = LayerMask.NameToLayer("Default");

        for(int i=0; i<5; i++)
        {
            sprite.enabled = false;
            yield return new WaitForSeconds(0.1f);
            sprite.enabled = true;
        }

        King.layer = LayerMask.NameToLayer("Player");




    }


    public IEnumerator FreezeTime(float time)
    {
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(time);

        Time.timeScale = 1f;
    }


    public void Walk()

    {
        
         x = Input.GetAxisRaw("Horizontal");

         move.linearVelocity = new Vector2(x * hiz, move.linearVelocity.y);   

        if (x==0)
        {
            animator.SetBool("Run", false);
        
        }
        else if (x==-1)
        {
            animator.SetBool("Run", true);
            sprite.flipX=true;
        }

        else if (x==1)
        {
            animator.SetBool("Run", true);
            sprite.flipX=false;
        }



    }

    public void DealDamage()
    {
        if(spriteRenderer.flipX==false)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(vurusyeri.position, 3,EnemyLayer);

            if(hitEnemies!= null)
            {
                foreach(Collider2D enemy in hitEnemies)
                {
                    Boss boss = enemy.GetComponent<Boss>();

                    boss.DamageAl(damage);
            
                }
            }
        }


         else if(spriteRenderer.flipX==true)
          {
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(vurusyerireverse.position, 3,EnemyLayer);
    
                if(hitEnemies!= null)
                {
                 foreach(Collider2D enemy in hitEnemies)
                 {

                    Boss boss = enemy.GetComponent<Boss>();

                    boss.DamageAl(damage);
                
                 }
                }
        }
    }

    

    public void GetDamage(float damage)
    {
        health -= damage;
        animator.SetTrigger("GetHit");
        StartCoroutine(HasarAlma());
        if (health<=0)
        {
            health = 0;
            animator.CrossFade("Death", 0.6f);
            SceneManager.LoadScene("test");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isGrounded = false;
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isGrounded = true;
        }
    }   

    public void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            move.AddForce(new Vector2(0f, jumpforce), ForceMode2D.Impulse);
            animator.SetTrigger("Jump");
        }



        if(move.linearVelocityY<-0.1f && isGrounded==false)
        {
            animator.SetBool("Fall",true);
        }


        if(isGrounded==true)
        {
            animator.SetBool("Fall",false); 
        }

        else if (isGrounded==false)
        {
            animator.SetBool("Run",false);
        }
    }

    public void Hit()
    {
        if(Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
        }
        
    }



    public void Hareketler()
    {
        Walk();
        Hit();
        Jump();
    }

    public void Start()
    {
        Time.timeScale = 1f;
        slider.maxValue = health;
        slider.minValue = 0;
    }


    void Update()
    {
        Hareketler();
        slider.value = health;
    }
}
