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
    public Enemy enemy;
    public SpriteRenderer spriteRenderer;
    public Transform vurusyeri;
    public Transform vurusyerireverse;
    public LayerMask EnemyLayer;
    public LayerMask PlayerLayer;
    public Collider2D kilic;
    public Collider2D reversekilic;
    public GameObject King;
    public Slider slider;
    public AudioSource KilicSesleri;
    public AudioClip Kilicsallama;
    public AudioClip Kilicsallama2;
    public AudioClip Kilicsallama3;
    public AudioClip Kiliclavurma;
    public AudioClip KalkanaVurma;
    public Animator crowdAnimator;
    public Animator crowdAnimator2;
    public Animator crowdAnimator3;
    public AudioSource crowdAudio;
    public AudioSource loopAudio;
    public AudioClip EntranceAudio;
    public AudioClip ApplauseAudio;
    public AudioClip LoopAudio;
    public AudioClip Announcer;
    public AudioClip FirstPhase;
    public Slider shieldSlider;
    public Canvas escmenu;
    public bool entrance=false;
    public bool bossdur = false;
    public bool canMove = true;
    bool isPaused=false;


    public void SeyircileriCostur()
    {
        crowdAudio.clip=EntranceAudio;
        crowdAudio.Play();
        crowdAnimator.SetTrigger("Entrance");
        crowdAnimator2.SetTrigger("Entrance");
        crowdAnimator3.SetTrigger("Entrance");
    }
    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("AOE"))
        {
            StartCoroutine(GetDamage(boss.damage));
        }

        else if (collision.CompareTag("giris")&&entrance==false)
        {
            entrance = true;
            bossdur = true;
            canMove = false;
           
            yield return new WaitForEndOfFrame();
            animator.SetBool("run", false);
            move.linearVelocity = Vector2.zero;
          
            
            crowdAudio.clip=EntranceAudio;
            crowdAudio.Play();
            crowdAudio.PlayOneShot(Announcer);
            loopAudio.loop = true;
            loopAudio.clip = FirstPhase;
            loopAudio.Play();
            StartCoroutine(boss.SahneyeGiris());
            yield return new WaitForSeconds(2f);
            
            crowdAnimator.SetTrigger("Entrance");
            crowdAnimator2.SetTrigger("Entrance");
            crowdAnimator3.SetTrigger("Entrance");
            StartCoroutine(boss.ShakeCamera(0.05f, 6f));
            yield return new WaitForSeconds(6f);
  
            canMove = true;
            bossdur = false;
          
            

        }

       
    }

    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; 

     
        
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; 
        escmenu.gameObject.SetActive(false);



    }
    public void ESCMENU()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            escmenu.gameObject.SetActive(true);
            PauseGame();

        }
    }


    public void Dash()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetTrigger("Dash");
            if(spriteRenderer.flipX==false)
            {
                move.linearVelocity = new Vector2(hiz * 3, move.linearVelocityY);         
            }

            else if (spriteRenderer.flipX==true)
            {
                move.linearVelocity = new Vector2(hiz * -3, move.linearVelocityY);
            }
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

    public void Menu()
    {
        
    }
    

    public IEnumerator GetDamage(float damage)
    {
        health -= damage;
        animator.SetTrigger("GetHit");
        StartCoroutine(HasarAlma());
        yield return null;
      
    }

    public IEnumerator Death()
    {
        if (health <= 0)
        {
            health = 0;
            animator.CrossFade("Death", 0.6f);
            yield return new WaitForSeconds(1.5f);
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
        if(Input.GetKeyDown(KeyCode.Space) &&isGrounded==true)
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
        int sýra = 0;
        if (Input.GetMouseButtonDown(0))
        {
            sýra++;
            animator.SetTrigger("Attack");
            

            if(sýra==0)
            {
                KilicSesleri.PlayOneShot(Kilicsallama);
            }

            else if(sýra==1)
            {
                KilicSesleri.PlayOneShot(Kilicsallama2);
            }

            else if (sýra==2)
            {
                KilicSesleri.PlayOneShot(Kilicsallama3);
                sýra = 0;
            }
        }
        
    }



    public void Hareketler()
    {
        if(canMove==true)
        {
            Walk();
            if(isPaused==false)
            {
                Hit();
            }
           
            Jump();
            Dash();
            ESCMENU();
            
        }
       StartCoroutine(Death()); 
    }

    public void Start()
    {
        Time.timeScale = 1f;
        slider.maxValue = health;
        slider.minValue = 0;
        shieldSlider.maxValue = 10;
        shieldSlider.minValue = 0;
    }


    void Update()
    {
        Hareketler();
        slider.value = health;
        shieldSlider.value = boss.counter;
    }
}
