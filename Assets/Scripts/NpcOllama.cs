using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;


[Serializable]public class OllamaRequest
{
    public string model;
    public string prompt;
    public bool stream = false;
    public string format = "json";
}

[Serializable]public class OllamaResponse
{
    public string response;
}

[Serializable]public class NPCData
{
    public string dialogResponse;
    public int iknaEtkisi;
}

public class NpcOllama : MonoBehaviour
{
    
    private string ollamaUrl = "http://localhost:11434/api/generate";

    [SerializeField] private string modelName = "gemma3:4b"; //phi3 veya //llama3.2 //gemma3:4b

    private int toplamIknaPuani = 0;
    private int IknaBasariSýnýrý = 100;

    [SerializeField] private TextUI TextUIKodu;
    private TextMeshProUGUI textDusunuyor;

    [SerializeField] private GameObject SonKapiAcik;
    [SerializeField] private GameObject SonKapiKapali;

    private string npcSistemPrompt =
    "Sen oyuncunun bir canavarý kestiðine inanmýyorsun. Oyuncu senden geçmek istiyor. " +
    "Oyuncunun söylediði cümleye göre ikna olma durumunu deðerlendir. " +
    "Eðer mantýklý bir gerekçe(canavarýn ýþýnlandýðýný,kýlýç kullandýðýný,kalkaný olduðunu,mor ve siyah renklerde olduðunu) söylerse veya rüþvet sunarsa 'iknaEtkisi'ni yüksek pozitif (+10 ile +25 arasý), " +
    "Eðer hakaret ederse veya tehdit ederse negatif (-10 ile -25 arasý) bir deðer ver. " +
    "Cevabýný YALNIZCA þu JSON formatýnda ver: " +
    "\"dialogResponse\": Oyuncuya söyleyeceðin Türkçe cümle \"iknaEtkisi\": Oyuncunun söylediðine ne kadar ikna olduðun";

    public void SendPlayerMessage(string playerInput)
    {
        textDusunuyor = TextUIKodu.DusunuyorTextiEkle();
        StartCoroutine(PostLLMRequest(playerInput));
    }

    private IEnumerator PostLLMRequest(string playerInput)
    {
        string fullPrompt = $"{npcSistemPrompt}\n\nOyuncu dedi ki: \"{playerInput}\"\nCevap:";

        OllamaRequest reqData = new OllamaRequest
        {
            model = modelName,
            prompt = fullPrompt,
            stream = false,
            format = "json"
        };

        string jsonPayload = JsonUtility.ToJson(reqData);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);

        using (UnityWebRequest request = new UnityWebRequest(ollamaUrl, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            Debug.Log(modelName + " Modeline istek gönderiliyor...");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                ProcessLLMResponse(request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("LLM Baðlantý Hatasý: " + request.error);
                TextUIKodu.TextEkle("LLM Baðlantý Hatasý: " + request.error);
            }
        }
    }

    private void ProcessLLMResponse(string rawJsonResponse)
    {
        try
        {
            OllamaResponse ollamaRes = JsonUtility.FromJson<OllamaResponse>(rawJsonResponse);
            NPCData npcData = JsonUtility.FromJson<NPCData>(ollamaRes.response);

            Debug.Log("NPC Yanýtý: " + npcData.dialogResponse);
            toplamIknaPuani += npcData.iknaEtkisi;
            Debug.Log("Ýkna Oraný: " + toplamIknaPuani);

            if (textDusunuyor != null)
            {
                Destroy(textDusunuyor);
            }

            TextUIKodu.GuvenTextGuncelle(toplamIknaPuani);
            TextUIKodu.NpcTextiEkle(npcData.dialogResponse);

            SeneryoTamamlandiMi();
        }
        catch (Exception e)
        {
            Debug.LogError("LLM istenen formatta yanýt veremedi " + e.Message);
            TextUIKodu.TextEkle("LLM istenen formatta yanýt veremedi " + e.Message);
        }
    }

    private void SeneryoTamamlandiMi()
    {
        if (toplamIknaPuani >= IknaBasariSýnýrý)
        {
            Debug.Log("SENARYO BÝTTÝ: NPC ikna oldu.");
            TextUIKodu.TextEkle("Ýkna etmeyi baþardýn.");

            SonKapiAcik.SetActive(true);
            SonKapiKapali.SetActive(false);
        }
    }

    public bool GetIknaOlduMu()
    {
        if (toplamIknaPuani >= 100)
        {
            return true;
        }
        return false;
    }
}
