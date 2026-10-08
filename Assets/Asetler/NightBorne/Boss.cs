using System.Collections;
using Unity.MLAgents.Integrations.Match3;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class Boss : MonoBehaviour
{
    
    public Rigidbody2D action;
    public Transform Knight;
    public float speed = 9f;
    public float hp = 40f;
    public float menzil=7f;
    public Animator animator;
    public float takipmenzili=20f;
    public float mesafex;
    public int karar;
    public enum  durumlar  { Attack1, Attack2, Teleport, Move };
    public durumlar currentState = durumlar.Move;
    bool canAttack = true;


    public void Run()
    {
    
        animator.SetBool("Run", true);
        if(mesafex>0)
        {
            action.linearVelocity = new Vector2(speed, action.linearVelocity.y);
        }
        else if (mesafex<0)
        {
            action.linearVelocity = new Vector2(-speed, action.linearVelocity.y);
        }
        
    }


    public void YonAyarla()
    {
        if (mesafex > 0)
        {
            action.linearVelocity = new Vector2(speed, action.linearVelocity.y);
        }
        else if (mesafex < 0)
        {
            action.linearVelocity = new Vector2(-speed, action.linearVelocity.y);
        }
    }

    public IEnumerator Attack1()
    {
        canAttack = false;
        currentState = durumlar.Attack1;
        animator.SetTrigger("Attack1");
        float animlength= animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        canAttack = true;
        currentState = durumlar.Move;
    }


    public IEnumerator Attack2()
    {
        canAttack = false;
        currentState = durumlar.Attack2;
        animator.SetTrigger("Attack2");
        float animlength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        canAttack = true;
        currentState = durumlar.Move;
    }

    public IEnumerator Teleport()
    {
        
        currentState = durumlar.Teleport;
        canAttack = false;
        animator.SetTrigger("teleport");
        Vector2 pozisyon = transform.position;
        float animlength= animator.GetCurrentAnimatorStateInfo(0).length;
      
        yield return new WaitForSeconds(animlength);
        if(mesafex>0)
        {
            pozisyon.x = Knight.position.x + 8f;
        }

        else if (mesafex<0)
        {
            pozisyon.x = Knight.position.x - 8f;
        }

      
        transform.position = pozisyon;

        canAttack = true;
        currentState = durumlar.Move;

    }



    void Start()
    {
        
    }

    // Update is called once per frame
  void Update()
    {
        mesafex = Knight.position.x - transform.position.x;
        switch (currentState)
        {
            case durumlar.Move:
                if(Mathf.Abs(mesafex) <= takipmenzili && Mathf.Abs(mesafex)>menzil)
                {
                    Run();
                }
                else if(Mathf.Abs(mesafex) <= menzil&&canAttack==true)
                {
                    animator.SetBool(name: "Run", value: false);
           
                    action.linearVelocity = Vector2.zero;
                    StartCoroutine(Teleport());
                    YonAyarla();
                    StartCoroutine(Attack2());

                }
                break;
            case durumlar.Attack1:
               
                break;
            case durumlar.Attack2:
               
                break;
            case durumlar.Teleport:
              
                break;
        }


    }
}
