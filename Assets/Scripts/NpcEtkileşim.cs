using System.Collections;
using UnityEngine;

public class NpcEtkileşim : MonoBehaviour
{
    [SerializeField] private GameObject yaziUI;
    [SerializeField] private GameObject eTiklaYazisi;
    [SerializeField] private AnaKarakter anaKarakterKodu;
    [SerializeField] Boss boss;
    [SerializeField] Transform tpKonumu;
    [SerializeField] GameObject anaKarakter;
    [SerializeField] SpriteRenderer spriteRenderer;
    float mesafex;
    private bool oyuncuAlandaMi;

    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && oyuncuAlandaMi)
        {
            EBasti();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            TabBasti();
        }
        mesafex = anaKarakter.transform.position.x - transform.position.x;

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

    public void EBasti()
    {
        anaKarakterKodu.canMove = false;
        yaziUI.SetActive(true);
    }

    public void TabBasti()
    {
        anaKarakterKodu.canMove = true;
        yaziUI.SetActive(false);
    }

    public IEnumerator GetToPlayer()
    {
        Vector2 MonkPozisyonu = transform.position;
        if(boss.hp<=0)
        {
            yield return new WaitForSeconds(4f);
            transform.position = tpKonumu.position;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger Enter");
        if (other.CompareTag("Player"))
        {
            eTiklaYazisi.SetActive(true);
            oyuncuAlandaMi = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Trigger Exit");
        if (other.CompareTag("Player"))
        {
            eTiklaYazisi.SetActive(false);
            oyuncuAlandaMi = false;
        }
    }
}
