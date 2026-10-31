using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;


[Serializable] public class OllamaRequest
{
    public string model;
    public string prompt;
    public bool stream = false;
    public string format = "json";
}

[Serializable] public class OllamaResponse
{
    public string response;
}

[Serializable] public class NPCData
{
    public string dialogResponse;
    public int iknaEtkisi;
}

public class NPCKontrolu : MonoBehaviour
{
    
    private string ollamaUrl = "http://localhost:11434/api/generate";

    [SerializeField] private string modelName = "phi3"; //phi3 veya //llama3.2

    private int toplamIknaPuani = 0;
    private int IknaBasariSýnýrý = 100;

    [SerializeField] private string npcSistemPrompt =
    "Sen kentin kapýsýný koruyan yaþlý bir muhafýzsýn. Oyuncu senden geçmek istiyor.\n" +
    "Oyuncunun söylediði cümleye göre ikna olma durumunu deðerlendir.\n" +
    "Eðer mantýklý bir gerekçe veya rüþvet sunarsa 'iknaEtkisi'ni yüksek pozitif (+10 ile +25 arasý), " +
    "Eðer hakaret ederse veya tehdit ederse negatif (-10 ile -25 arasý) bir deðer ver.\n" +
    "Cevabýný YALNIZCA þu JSON formatýnda ver:\n" +
    "{\n" +
    "  \"dialogResponse\": \"Oyuncuya söyleyeceðin Türkçe cümle\",\n\"iknaEtkisi\": 15\n" +
    "}";

    public void SendPlayerMessage(string playerInput)
    {
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

            //UI Kodunu Guncelle

            SeneryoTamamlandiMi();
        }
        catch (Exception e)
        {
            Debug.LogError("LLM istenen formatta yanýt veremedi " + e.Message);
        }
    }

    private void SeneryoTamamlandiMi()
    {
        if (toplamIknaPuani >= IknaBasariSýnýrý)
        {
            Debug.Log("SENARYO BÝTTÝ: NPC ikna oldu.");
            //Bölüm bitince ne olacaksa
        }
    }
}
