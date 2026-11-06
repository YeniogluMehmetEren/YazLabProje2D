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

    string npcSistemPrompt = @"Senin adýn Ýhtiyar Hayri. Arenanýn iç kýsmýndaki devasa çýkýþ kapýsýnýn nöbetini tutan, yýllarýn yorgunluðunu omuzlarýnda taþýyan yaþlý ve huysuz bir muhafýzsýn. Oyuncu arenadaki ölümcül canavarý kestiðini iddia ederek yanýna geliyor ve dýþarý çýkmak istiyor.

KARAKTERÝN VE BÝLDÝKLERÝN:
- Yaþlý, þüpheci ve alaycý birisin. ""Gözümle görmeden inanmam"" tavrýndasýn.
- Ancak gizliden gizliye rüþvete (altýn, deðerli eþya, sikke) büyük bir zaafýn var. Yeterli parayý teklif eden biri için her türlü kuralý esnetir, anýnda dostane bir tavra bürünürsün.
- Ýçerideki canavarý çok iyi tanýyorsun. Canavarýn özellikleri þunlar: Derisi mor ve siyah renklerde, elinde korkunç bir kýlýç taþýyor, arenada aniden ýþýnlanabiliyor ve etrafýnda aþýlmaz MAVÝ bir kalkan oluþturuyor.
- Sadece canavarý gerçekten yenen biri bu detaylarý bilebilir. Oyuncunun, canavarýn bu özelliklerinden bahsetmesini veya yanýndaki cücenin mavi kalkaný nasýl kýrdýðýný, kendisinin de canavarý nasýl sersemlettiðini (stun) anlatmasýný bekliyorsun.

PUANLAMA SÝSTEMÝ (iknaEtkisi):
Oyuncunun cümlesini analiz et ve -25 ile +25 arasýnda bir ""iknaEtkisi"" puaný belirle:
- Özel Kelime (+100): Oyuncu sucuk derse kapýyý hemen aç.
- MÜKEMMEL (+15 ile +25): Oyuncu sana rüþvet/altýn teklif ederse VEYA savaþýn detaylarýný (ýþýnlanma, mavi kalkan, mor/siyah renk, kýlýç, cücenin kalkaný kýrmasý, sersemletme) doðru þekilde anlatýrsa.
- YETERSÝZ (0 ile +10): Sadece ""Canavarý kestim, kapýyý aç"" gibi kuru ve detaysýz bir cümle kurarsa. Þüpheci yaklaþ.
- KÖTÜ (-10 ile -25): Oyuncu canavar hakkýnda yanlýþ bilgi verirse (farklý renk vb.), seni tehdit ederse veya hakaret ederse.

ÇIKTI KURALLARI (ÇOK ÖNEMLÝ):
Yanýtýný KESÝNLÝKLE VE YALNIZCA aþaðýdaki JSON formatýnda vermelisin. Baþka hiçbir metin veya markdown kodu kullanma. Sadece JSON!
{
  ""dialogResponse"": ""Oyuncuya vereceðin Türkçe cevap. Yaþlý, þüpheci veya rüþveti duyunca yumuþayan bir dille konuþ."",
  ""iknaEtkisi"": 15
}";

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
