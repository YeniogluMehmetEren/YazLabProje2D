using UnityEngine;

public class AnaKarakter : MonoBehaviour
{


    public Rigidbody2D move;
    public Animator animator;
    public int hiz = 5;
    public float damage=1f;
    public float health=10f;
    public SpriteRenderer sprite;
    public float x;




    public void Walk()

    {
        
         x = Input.GetAxisRaw("Horizontal");

         move.linearVelocity = new Vector2(x * hiz, move.linearVelocity.y);   

        if (x==0)
        {
            animator.SetBool("RunRight", false);
            animator.SetBool("RunLeft", false);
        }
        else if (x==-1)
        {
            animator.SetBool("RunLeft", true);
            sprite.flipX=true;
        }

        else if (x==1)
        {
            animator.SetBool("RunRight", true);
            sprite.flipX=false;
        }



    }


    public void Hit()
    {
        if(Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
        }
        
    }



    void Start()
    {
        
    }

    void Update()
    {
        Walk();
        Hit();
    }
}
