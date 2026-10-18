using UnityEngine;

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
    public Collider2D kilic;
    public Collider2D reversekilic;


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
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(vurusyeri.position, 4,EnemyLayer);

            if(hitEnemies!= null)
            {
                foreach(Collider2D enemy in hitEnemies)
                {
                    Boss boss = enemy.GetComponent<Boss>();

                   boss.DamageAl(damage);
                    boss.isShielded = false;
                }
            }
        }


         else if(spriteRenderer.flipX==true)
          {
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(vurusyerireverse.position, 4,EnemyLayer);
    
                if(hitEnemies!= null)
                {
                 foreach(Collider2D enemy in hitEnemies)
                 {

                    Boss boss = enemy.GetComponent<Boss>();

                    boss.DamageAl(damage);
                    boss.isShielded = false;
                 }
                }
        }
    }



    public void GetDamage(float damage)
    {
        
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


   

    void Update()
    {
        Hareketler();
    }
}
