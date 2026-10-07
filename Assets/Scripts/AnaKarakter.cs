using UnityEngine;

public class AnaKarakter : MonoBehaviour
{


    public Rigidbody2D move;
    public Animator animator;
    public float hiz = 5f;
    public float damage=1f;
    public float health=10f;
    public SpriteRenderer sprite;




    public void Walk()

    {
        
        float x = Input.GetAxis("Horizontal");

         move.linearVelocity=new Vector2(x * hiz,0);

        if(x==0)
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
