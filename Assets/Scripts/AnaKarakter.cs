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


    void Start()
    {
        
    }

    void Update()
    {
        Hareketler();
    }
}
