using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public Animator animator;
    public AudioSource loopsource;
    public AudioClip mainmenuthemeclip;
 
    void Start()
    {
        loopsource.loop = true;
        loopsource.clip = mainmenuthemeclip;
        loopsource.Play();
        animator.SetTrigger("Entrance");
    }


    void Update()
    {
        
       


    }
}
