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
    public float damage=4f;
    public Animator animator;
    public float takipmenzili=20f;
    public float mesafex;
    public int karar;
    public SpriteRenderer spriteRenderer;
    public GameObject AOE;
    public float patlamalarArasiMesafe = 1f;
    public Transform baslangicyerisol;
    public Transform baslangicyeri;
    public enum  durumlar  { Attack1, Attack2, Attack3,Attack4,Teleport, Move, Karar,tpAttack,Retreat};
    public durumlar currentState = durumlar.Move;
    public bool canAttack = true;
    public  GameObject yeniMermi;
    public bool tpattack = false;
    public AnaKarakter anaKarakter;
    public Transform camera;
    public Color color;
    public bool isShielded = true;
    public void Run()
    {
    
         currentState= durumlar.Move;   
        if (canAttack==true)
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


    public void Retreat()
    {
        currentState= durumlar.Retreat; 

        
        
            animator.SetBool("Run", true);
            if (mesafex > 0)
            {
                action.linearVelocity = new Vector2(-speed*3, action.linearVelocity.y);
            }
            else if (mesafex < 0)
            {
                action.linearVelocity = new Vector2(speed*3, action.linearVelocity.y);
            }
        

        currentState= durumlar.Move;
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
        currentState= durumlar.Move;    
    }

    public IEnumerator TPattack()
    {
        canAttack = false;
        currentState = durumlar.tpAttack;
        spriteRenderer.color = Color.blueViolet;   
        for (int i=0; i<3; i++)
        {
            Vector2 playerposition = Knight.position;
            animator.SetTrigger("teleport");
            YonAyarla();
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(ShakeCamera(0.02f, 0.3f));
            transform.position = playerposition;
            yield return new WaitForSeconds(1f);
        }
        tpattack = false;
        canAttack= true;   
 
        spriteRenderer.color = Color.white;
        currentState = durumlar.Move;
    

    }
    public IEnumerator Attack2()
    {
        canAttack = false;
        currentState = durumlar.Attack2;
       
      
        animator.SetTrigger("Attack2");
        YonAyarla();
        yield return new WaitForEndOfFrame();
        float animlength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(0f);
        canAttack = true;

        currentState = durumlar.Move;



    }


    public IEnumerator Attack3()
    {
        canAttack = false;
        currentState = durumlar.Attack3;


        animator.SetTrigger("Attack3");
        YonAyarla();
        yield return new WaitForEndOfFrame();
        float animlength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(0f);
        canAttack = true;

        currentState = durumlar.Move;



    }


    public IEnumerator Attack4()
    {
        canAttack = false;
        currentState = durumlar.Attack4;


        animator.SetTrigger("Attack4");
        YonAyarla();
        yield return new WaitForEndOfFrame();
        float animlength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(0.5f);
        canAttack = true;

        currentState = durumlar.Move;



    }



    private IEnumerator ShakeCamera(float siddet, float zaman)
    {
        Vector3 orginalpozisyon = camera.localPosition;



        float gecensure = 0f;
        while (gecensure <= zaman)
        {

            float x = Random.Range(-1f, 1f) * siddet;
            float y = Random.Range(-1f, 1f) * siddet;


            camera.localPosition = new Vector3(orginalpozisyon.x + x, orginalpozisyon.y + y, orginalpozisyon.z);
            gecensure += Time.unscaledDeltaTime;
            yield return null;
        }



        camera.localPosition = orginalpozisyon;



    }

    public IEnumerator Teleport()
    {
      
        
           
       

        Vector2 pozisyon = transform.position;
        currentState = durumlar.Teleport;
        canAttack = false;
        int karar= Random.Range(0, 2);
        animator.SetTrigger("teleport");
       
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.57f);
        StartCoroutine(ShakeCamera(0.03f, 0.3f));

        if (karar==0)
        {
            pozisyon.x = Knight.position.x + 10f;
        }
        else if(karar==1)
        {
            pozisyon.x = Knight.position.x - 10f;
        }   
        transform.position = pozisyon;
        yield return new  WaitForSeconds(1f);
        canAttack = true;








    }


    public void GetDamage(float damage)
    {
        if (isShielded == true)
        {
            spriteRenderer.color = Color.blue;
        }

        else if (isShielded == false)
        {
            spriteRenderer.color = Color.white;
            hp -= damage;
        }
    }

    public void DamageVur()
    {
       if(spriteRenderer.flipX==false)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(baslangicyerisol.position, 4);

            if(hitEnemies!= null)
            {
                foreach(Collider2D enemy in hitEnemies)
                {
                    AnaKarakter anakarakter = enemy.GetComponent<AnaKarakter>();
                    anaKarakter.GetDamage(damage);
                }
            }
        }


         else if(spriteRenderer.flipX==true)
          {
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(baslangicyeri.position, 4);
    
                if(hitEnemies!= null)
                {
                 foreach(Collider2D enemy in hitEnemies)
                 {

                    AnaKarakter anakarakter = enemy.GetComponent<AnaKarakter>();
                    anakarakter.GetDamage(damage);
                 }
                }
        }
    }




    public void DamageAl(float damage)
    {


        hp = hp - damage;

        if (hp <= 0)
        {
            hp = 0;
            animator.SetTrigger("teleport");
            float animlength = animator.GetCurrentAnimatorStateInfo(0).length;
            Destroy(gameObject, animlength);
        }

    }
    public void TPkarar()
    {
        if(tpattack==true)
        {
            StartCoroutine(TPattack());
        }

        else if (tpattack==false)
        {
            StartCoroutine(Teleport());
        }
    }

    public void AOEAttack()
    {
        Vector3 yon = transform.position-Knight.position;
        Vector3 arttýr = yon.normalized * 0.25f;
        for (int i=0; i<5; i++)
        {


            Vector3 patlamaNoktasi = baslangicyeri.position +arttýr;
            yeniMermi=Instantiate(AOE,patlamaNoktasi,Quaternion.identity);
            Destroy(yeniMermi, 1.5f);

        }
       

    }



    public IEnumerator KararVer()
    {
        currentState = durumlar.Karar;
        karar = Random.Range(0, 5);
        Debug.Log(karar);
        canAttack = false;
        if (karar==0)
        {
   
            StartCoroutine(Attack1());
         
        }

        else if(karar==1)
        {
            StartCoroutine(Teleport()); 
            yield return new WaitForSeconds(1f);
            StartCoroutine(Attack2());
        }
        else if (karar==2)
        {
            StartCoroutine(TPattack());
      
        }

        else if (karar==3)
        {
            StartCoroutine(Attack3());
           
        }
        else if (karar==4)
        {
            StartCoroutine(Attack4());
           
        }

    }



    public void ShieldUp()
    {
       
        if(isShielded==true)
        {
            spriteRenderer.color = Color.blue;

        }

        else if(isShielded==false)
        {
            spriteRenderer.color = Color.white;
        }
    }

    void Start()
    {
      
        color=spriteRenderer.color;
        color = Color.white;
       

    }

    // Update is called once per frame
  void Update()
    {
        if (isShielded == true)
        {
            spriteRenderer.color = Color.blue;
        }
        mesafex = Knight.position.x - transform.position.x;
        switch (currentState)
        {
            case durumlar.Move:
                if(Mathf.Abs(mesafex) <= takipmenzili && Mathf.Abs(mesafex)>menzil)
                {
                    YonAyarla();
                    Run();
                }
                else if(Mathf.Abs(mesafex) <= menzil&& canAttack==true)
                {
                    animator.SetBool(name: "Run", value: false);
                    action.linearVelocity=Vector2.zero;

                    StartCoroutine(KararVer());
         
                }


                else if (Mathf.Abs(mesafex) <menzil)
                {
                    Retreat();
                }
                    break;
            case durumlar.Attack1:
               action.linearVelocity=Vector2.zero;
                break;
            case durumlar.Attack2:
                   action.linearVelocity=Vector2.zero;
                break;
            case durumlar.Teleport:
                action.linearVelocity = Vector2.zero;
                animator.SetBool("Run", false);
            break;
             case durumlar.Attack3:
                action.linearVelocity = Vector2.zero;
                break;
            case durumlar.Attack4:
                action.linearVelocity = Vector2.zero;
                break;

            case durumlar.Karar:
            break;
            case durumlar.Retreat:
            break;

            case durumlar.tpAttack:
                action.linearVelocity = Vector2.zero;
                animator.SetBool("Run", false);
                break;
        }


    }
}
