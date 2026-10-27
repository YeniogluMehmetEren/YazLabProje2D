using System.Collections;
using System.Threading;
using Unity.MLAgents.Integrations.Match3;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    
    public Rigidbody2D action;
    public Transform Knight;
    public float speed = 9f;
    public float hp = 40f;
    public float maxhp = 40f;
    public float menzil=7f;
    public float damage=3f;
    public Animator animator;
    public float takipmenzili=20f;
    public float mesafex;
    public int karar;
    public SpriteRenderer spriteRenderer;
    public GameObject AOE;
    public float patlamalarArasiMesafe = 1f;
    public Transform baslangicyerisol;
    public Transform baslangicyeri;
    public enum  durumlar  { Attack1, Attack2, Attack3,Attack4,Teleport, Move, Karar,tpAttack,Retreat,Stunned,SecondPhase};
    public durumlar currentState = durumlar.Move;
    public bool canAttack = true;
    public  GameObject yeniMermi;
    public GameObject yeniMermi2;
    public bool tpattack = false;
    public AnaKarakter anaKarakter;
    public Transform camera;
    public Color color;
    public bool isShielded = true;
    public bool isShieldBreakable = false;
    public int counter = 0;
    public AudioSource TeleportSesleri;
    public AudioClip Tp1;
    public AudioClip Tp2;
    public LayerMask Layer;
    bool isStunned = false;
    public float tpmesafesi = 10f;
    public Slider slider;
    bool enterSecondPhase = false;
    public Transform giris;
    public Animator crowdAnimator;
    public AudioSource crowdAudio;
    public AudioClip EntranceAudio;
    public AudioClip ApplauseAudio;

    [Header("Ai Egitimi")]
    public bool AiEgitimiOlsunMu = true;
    public void Run()
    {
        if (!AiEgitimiOlsunMu && anaKarakter.bossdur==false)
        {
            currentState = durumlar.Move;
            if (canAttack == true)
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
    }


    public IEnumerator SahneyeGiris()
    {
        Vector2 giriskonumu = giris.position;
        Vector2 ilkkonum = transform.position;

        animator.SetTrigger("Teleport");
        yield return new WaitForSeconds(0.45f);
        transform.position = giriskonumu;

        if(hp<=0)
        {
            yield return new WaitForSeconds(2f);
            Death();
            transform.position = ilkkonum;
        }
    }


    public void ActivateSecondPhase()
    {
       
        StartCoroutine(SecondPhase());
        speed = speed * 2f;
        animator.SetFloat("hýz", 1.7f);
 
    }


    public IEnumerator Retreat()
    {
        currentState= durumlar.Retreat; 

        
        
            animator.SetBool("Run", true);
            if (mesafex > 0)
            {
                action.linearVelocity = new Vector2(25, action.linearVelocity.y);
            }
            else if (mesafex < 0)
            {
                action.linearVelocity = new Vector2(25f, action.linearVelocity.y);
            }

        yield return new WaitForSeconds(0.2f);

        action.linearVelocity = Vector2.zero;

        currentState = durumlar.Move;

       

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
        if (enterSecondPhase == true)
        {

            StartCoroutine(Teleport());
            yield return new WaitForSeconds(1f);
        }
        canAttack = true;
        currentState= durumlar.Move;    
    }

    public IEnumerator TPattack()
    {
        canAttack = false;
        tpattack = true;
        currentState = durumlar.tpAttack;
        spriteRenderer.color = Color.red;   
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
        if (enterSecondPhase == true)
        {

            StartCoroutine(Teleport());
            yield return new WaitForSeconds(1f);
        }
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
        if (enterSecondPhase == true)
        {

            StartCoroutine(Teleport());
            yield return new WaitForSeconds(1f);
        }
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
        if (enterSecondPhase == true)
        {

            StartCoroutine(Teleport());
            yield return new WaitForSeconds(1f);
        }
        canAttack = true;

        currentState = durumlar.Move;



    }



    public IEnumerator ShakeCamera(float siddet, float zaman)
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

   
   

   

    public void DamageVur()
    {
       if(spriteRenderer.flipX==false)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(baslangicyerisol.position, 2);

            if(hitEnemies!= null)
            {
                foreach(Collider2D enemy in hitEnemies)
                {
                    AnaKarakter anakarakter= enemy.GetComponent<AnaKarakter>();


                    if(anakarakter!=null)
                    {
                        anakarakter.GetDamage(damage);
                    }
                }
            }
        }


         else if(spriteRenderer.flipX==true)
          {
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(baslangicyeri.position, 2);
    
                if(hitEnemies!= null)
                {
                 foreach(Collider2D enemy in hitEnemies)
                 {
                    AnaKarakter anakarakter = enemy.GetComponent<AnaKarakter>();


                    if (anakarakter != null)
                    {
                        anakarakter.GetDamage(damage);
                    }

                 }
                }
        }
    }


    public void ShieldKirAiIcin()
    {
        isShielded = false;
        isShieldBreakable = false;
        Debug.Log("Kalkan kýrýldý");
        StartCoroutine(ShieldReset());
    }


    public void TeleportSFX()
    {
        int ses = Random.Range(0, 2);


        if(ses==0)
        {
            TeleportSesleri.PlayOneShot(Tp1);
        }
        else if (ses==1)
        {
            TeleportSesleri.PlayOneShot(Tp2);
        }

    }

    public void Shield5050()
    {
        if (Random.Range(0f, 1f)<0.5f)
        {
            isShielded = false;
            isShieldBreakable = false;
            StartCoroutine(ShieldReset());
        }
        else
        {
            isShielded = true;
            isShieldBreakable = true;
        }
    }

    public IEnumerator Stunned()
    {
        currentState = durumlar.Stunned;
        isStunned = true;
        crowdAudio.clip = EntranceAudio;
        crowdAudio.Play();
        anaKarakter.crowdAnimator.SetTrigger("Entrance");
        anaKarakter.crowdAnimator2.SetTrigger("Entrance");
        anaKarakter.crowdAnimator.SetTrigger("Entrance");
        isShieldBreakable = true;
        animator.SetBool("stun", true);
        yield return new WaitForSeconds(5f);
        animator.SetBool("stun", false);
        isStunned = false;

        currentState= durumlar.Move;

    }

    public void DamageAl(float damage)
    {
        if(isShielded==true)
        {
            counter++;
            anaKarakter.KilicSesleri.clip = anaKarakter.KalkanaVurma;
            anaKarakter.KilicSesleri.Play();
        }
    
        if(counter>=10)
        {
            counter = 0;
            StartCoroutine(Stunned());
        }

        /*if(isShieldBreakable==true)
        {
            isShieldBreakable=false;
            isShielded = false;
            StartCoroutine(ShieldReset());
        }*/

        if(isShielded==false)
        {
            hp = hp - damage;
            StartCoroutine(DamageEffect());
            StartCoroutine(ShakeCamera(0.01f, 0.1f));
            anaKarakter.KilicSesleri.clip = anaKarakter.Kiliclavurma;
            anaKarakter.KilicSesleri.Play();
            StartCoroutine(anaKarakter.FreezeTime(0.1f));

        }
       
          

    }
    public IEnumerator DamageEffect()
    {
        spriteRenderer.color= Color.gray;
        yield return new WaitForSeconds(0.3f);
        spriteRenderer.color = Color.white;
    }

    public IEnumerator ShieldReset()
    {
       if(isShielded == false)
        {
            yield return new WaitForSeconds(15f);
            isShielded = true;
            if (AiEgitimiOlsunMu)
            {
                isShieldBreakable = true;
            }
            Debug.Log("Kalkan açýldý");
        }
    }

    public void Death()
    {
        if (hp <= 0 && anaKarakter.bossdur==true)
        {
            hp = 0;
            animator.SetTrigger("teleport");
            float animlength = animator.GetCurrentAnimatorStateInfo(0).length;
            slider.gameObject.SetActive(false);
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

    public IEnumerator AOEAttack()
    {
        Vector3 yon = transform.position - Knight.position;
        Vector3 arttir = yon.normalized * 2f;
        Vector3 patlamaNoktasi;

      
        if (spriteRenderer.flipX ==true)
        {
            patlamaNoktasi = baslangicyeri.position;
        }
        else 
        {
            patlamaNoktasi = baslangicyerisol.position;
        }

      
        for (int i = 0; i <6; i++)
        {
            yeniMermi = Instantiate(AOE, patlamaNoktasi, Quaternion.identity);
            patlamaNoktasi-= arttir;
            yield return new WaitForSeconds(0.3f);
            Destroy(yeniMermi, 0f);

        }

      
    }


    public IEnumerator Teleport()
    {

        currentState = durumlar.Teleport;
        canAttack = false;
        Vector2 pozisyon = transform.position;
        int karar = Random.Range(0, 2);
        animator.SetTrigger("teleport");

        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.57f);
        StartCoroutine(ShakeCamera(0.03f, 0.3f));


        if (karar == 0)
        {
            pozisyon.x = Knight.position.x + tpmesafesi;
        }
        else if (karar == 1)
        {
            pozisyon.x = Knight.position.x - tpmesafesi;
        }
        transform.position = pozisyon;


        canAttack = true;
    }




    public IEnumerator SecondPhase()
    {
        currentState = durumlar.SecondPhase;
        tpmesafesi = tpmesafesi*7 / 10;
        crowdAnimator.SetTrigger("Entrance");
        yield return new WaitForEndOfFrame();
        animator.SetBool("run", false);


        
       


        for (int i = 0; i < 5; i++)
        {

          
            StartCoroutine(Teleport());
            yield return new WaitForSeconds(0.6f);
            currentState = durumlar.SecondPhase;
            YonAyarla();
            
            

            
            Vector3 yon = transform.position - Knight.position;
            Vector3 arttir = yon.normalized * 1.5f;
            Vector3 patlamaNoktasi;
            Vector3 patlamaNoktasi2;
            patlamaNoktasi = baslangicyeri.position;
            patlamaNoktasi2 = baslangicyerisol.position;


            for (int j = 0; j < 4; j++)
            {
                YonAyarla();
               

                yeniMermi = Instantiate(AOE, patlamaNoktasi, Quaternion.identity);
                yeniMermi2 = Instantiate(AOE, patlamaNoktasi2, Quaternion.identity);
               

                if(spriteRenderer.flipX==true)
                {
                    patlamaNoktasi -= arttir;
                    patlamaNoktasi2 += arttir;
                }

                else if (spriteRenderer.flipX==false)
                {
                    patlamaNoktasi += arttir;
                    patlamaNoktasi2 -= arttir;
                }




                yield return new WaitForSeconds(0.3f);
                Destroy(yeniMermi, 0f);
                Destroy(yeniMermi2, 0f);
                
            }
            yield return new WaitForSeconds(0.3f);

        }


       
        enterSecondPhase = true;
        crowdAnimator.SetTrigger("Applause");
        crowdAudio.PlayOneShot(ApplauseAudio);
        currentState = durumlar.Move;
    }



    public IEnumerator KararVer()
    {
        if (!AiEgitimiOlsunMu)
        {
            currentState = durumlar.Karar;
            karar = Random.Range(0, 5);
            Debug.Log(karar);
            canAttack = false;
            if (karar == 0)
            {

                StartCoroutine(Attack1());
              
                

            }

            else if (karar == 1)
            {
                StartCoroutine(Teleport());
                yield return new WaitForSeconds(1f);
                StartCoroutine(Attack2());
            }
            else if (karar == 2)
            {
                StartCoroutine(TPattack());

            }

            else if (karar == 3)
            {
                StartCoroutine(Attack3());
               

            }
            else if (karar == 4)
            {
                StartCoroutine(Attack4());
               

            }
        }
    }



    public void SetColor()
    {
       if(isShielded==true)
       {
            spriteRenderer.color = Color.blue;

       }

        else if (isShielded==false && tpattack==false) 
        {
            spriteRenderer.color = Color.white;
        }

       else if (tpattack==true)
       {
            spriteRenderer.color = Color.red;
       }

    }

    public bool IsShieldBreakable()
    {
        return isShieldBreakable;
    }
    public bool IsShielded()
    {
        return isShielded;
    }

    void Start()
    {
      
        tpattack = false;
        slider.maxValue = hp;
        slider.minValue = 0;
    }

    // Update is called once per frame
  void Update()
    {
        slider.value = hp;

        
        mesafex = Knight.position.x - transform.position.x;
        switch (currentState)
        {
            case durumlar.Move:
                if(Mathf.Abs(mesafex) <= takipmenzili && Mathf.Abs(mesafex)>menzil && isStunned==false)
                {
                    YonAyarla();
                    Run();
                }
                else if(Mathf.Abs(mesafex) <= menzil&& canAttack==true && isStunned==false && Mathf.Abs(mesafex)>4)
                {
                    animator.SetBool(name: "Run", value: false);
                    action.linearVelocity=Vector2.zero;
                    StartCoroutine(KararVer());
         
                }


                else if (Mathf.Abs(mesafex)<=4 && isStunned==false)
                {
                   StartCoroutine(Retreat());
                }

               if(hp<= maxhp/2 && isStunned==false &&enterSecondPhase==false)
                {
                    ActivateSecondPhase();
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
            case durumlar.Stunned:
                action.linearVelocity = Vector2.zero;
            break;
            case durumlar.tpAttack:
            action.linearVelocity = Vector2.zero;
            animator.SetBool("Run", false);
            break;

            case durumlar.SecondPhase:

            break;
        }

        Death();
        SetColor();
        
    }
}
