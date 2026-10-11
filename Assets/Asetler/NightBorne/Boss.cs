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
    public SpriteRenderer spriteRenderer;
    public GameObject AOE;
    public float patlamalarArasiMesafe = 1f;
    public Transform baslangicyeri;
    public enum  durumlar  { Attack1, Attack2, Teleport, Move, Karar};
    public durumlar currentState = durumlar.Move;
    public bool canAttack = true;


    public void Run()
    {
    
        
        if(canAttack==true)
        {
            animator.SetBool("Run", true);
            if (mesafex > 0)
            {
                action.linearVelocity = new Vector2(speed, action.linearVelocity.y);
            }
            else if (mesafex < 0)
            {
                action.linearVelocity = new Vector2(-speed, action.linearVelocity.y);
            }
        }
       
        
    }


    public void YonAyarla()
    {
        if (mesafex > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (mesafex < 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    public IEnumerator Attack1()
    {
        canAttack = false;
        currentState = durumlar.Attack1;
        animator.SetTrigger("Attack1");
        yield return new WaitForEndOfFrame();
        float animlength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        canAttack = true;
        yield return new WaitForSeconds(1.5f);
       
        currentState = durumlar.Move;
    }


    public IEnumerator Attack2()
    {
        
        currentState = durumlar.Attack2;
        canAttack = false;
        YonAyarla();
        animator.SetTrigger("Attack2");
        yield return new WaitForEndOfFrame();
        float animlength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        canAttack = true;
        yield return new WaitForSeconds(1.5f);
        
        currentState = durumlar.Move;
    }

    public IEnumerator Teleport()
    {
        
        currentState = durumlar.Teleport;
 
        animator.SetTrigger("teleport");

        Vector2 pozisyon = transform.position;
        yield return new WaitForEndOfFrame();
        float animlength= animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animlength);
        pozisyon.x = Knight.position.x + 9.3f;
       




        transform.position = pozisyon;


        StartCoroutine(Attack2());

    }



    public IEnumerator AOEAttack()
    {
        Vector3 yon = transform.position-Knight.position;
      
        for(int i=0; i<mesafex/4; i++)
        {
            Vector3 patlamaNoktasi = baslangicyeri.position + yon/ 4;
            GameObject yeniMermi=Instantiate(AOE,patlamaNoktasi,Quaternion.identity);
            
            yield return new WaitForSeconds(0.5f);
            Destroy(yeniMermi, 0.5f);
        }


    }



    public IEnumerator KararVer()
    {
        currentState = durumlar.Karar;
        karar = Random.Range(0, 2);
        yield return new WaitForSeconds(0.5f);  
        if(karar==0)
        {
   
            StartCoroutine(Attack1());
        }

        else if(karar==1)
        {
             
            StartCoroutine(Teleport());
        }

        yield return new WaitForSeconds(5f);
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
                    action.linearVelocity=Vector2 .zero;

                    StartCoroutine(KararVer());

                 

                }
                break;
            case durumlar.Attack1:
               
                break;
            case durumlar.Attack2:
                
                break;
            case durumlar.Teleport:
                break;

            case durumlar.Karar:
            break;
        }


    }
}
