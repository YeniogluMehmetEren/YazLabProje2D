using UnityEngine;

public class NpcEtkileşim : MonoBehaviour
{
    [SerializeField] private GameObject yaziUI;
    [SerializeField] private GameObject eTiklaYazisi;
    [SerializeField] private AnaKarakter anaKarakterKodu;

    private bool oyuncuAlandaMi;
    private bool eBasildiMi;

    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && oyuncuAlandaMi)
        {
            EBasti();
        }
    }

    public void EBasti()
    {
        eBasildiMi = !eBasildiMi;

        if (eBasildiMi)
        {
            anaKarakterKodu.canMove = false;
            yaziUI.SetActive(true);
        }
        else if (!eBasildiMi)
        {
            anaKarakterKodu.canMove = true;
            yaziUI.SetActive(false);
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
