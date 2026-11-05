using UnityEngine;

public class KapiyaGirme : MonoBehaviour
{
    [SerializeField] private GameObject anaKarakter;
    [SerializeField] private GameObject gnome;

    [SerializeField] private GameObject cadirKapisi;
    [SerializeField] private GameObject arenayaGirdigimizKapi;
    [SerializeField] private GameObject arenadanCikisKapi;
    
    [SerializeField] private NpcOllama monkKodu;

    private bool cadirKapisiOnundeMi;
    private bool arenayaGirdigimizKapiOnundeMi;
    private bool arenadanCikisKapiOnundeMi;

    private void Update()
    {
        if (cadirKapisiOnundeMi && Input.GetKeyDown(KeyCode.E))
        {
            anaKarakter.transform.position = arenayaGirdigimizKapi.transform.position;
            gnome.transform.position = arenayaGirdigimizKapi.transform.position;
        }
        else if (arenayaGirdigimizKapiOnundeMi && Input.GetKeyDown(KeyCode.E))
        {
            anaKarakter.transform.position = cadirKapisi.transform.position;
            gnome.transform.position = cadirKapisi.transform.position;
        }
        else if (arenadanCikisKapiOnundeMi && Input.GetKeyDown(KeyCode.E) && monkKodu.GetIknaOlduMu())
        {
            Debug.Log("OYUN BÝTTÝ");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("cadirKapisi"))
        {
            cadirKapisiOnundeMi = true;
        }
        else if (other.CompareTag("arenayaGirdigimizKapi"))
        {
            arenayaGirdigimizKapiOnundeMi = true;
        }
        else if (other.CompareTag("arenadanCikisKapi"))
        {
            arenadanCikisKapiOnundeMi = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("cadirKapisi"))
        {
            cadirKapisiOnundeMi = false;
        }
        else if (other.CompareTag("arenayaGirdigimizKapi"))
        {
            arenayaGirdigimizKapiOnundeMi = false;
        }
        else if (other.CompareTag("arenadanCikisKapi"))
        {
            arenadanCikisKapiOnundeMi = false;
        }
    }

    
}
