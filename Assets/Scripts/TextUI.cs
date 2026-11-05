using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextUI : MonoBehaviour
{
    public TMP_InputField mesaj;
    public NpcOllama npcKontrolcusu;

    public Transform svContent;
    public TextMeshProUGUI prefebText;

    public Slider guvenSlider;
    public TextMeshProUGUI guvenText;

    void Start()
    {
        GuvenTextGuncelle(0);
        mesaj.onSubmit.AddListener(delegate { TextAiGonder(); });
        InputAl();
    }

    public void TextAiGonder()
    {
        if (mesaj.text.Trim().Length > 0 && npcKontrolcusu != null)
        {
            OyuncuTextiEkle();
            npcKontrolcusu.SendPlayerMessage(mesaj.text);
            mesaj.text = "";
        }
        InputAl();
    }

    private void InputAl()
    {
        if (mesaj != null)
        {
            mesaj.Select();
            mesaj.ActivateInputField();
        }
    }

    public void OyuncuTextiEkle()
    {
        TextMeshProUGUI yeniSatir = Instantiate(prefebText, svContent);
        string tamMesaj = " Sen: " + mesaj.text;
        yeniSatir.text = tamMesaj;
    }

    public void NpcTextiEkle(string yanit)
    {
        TextMeshProUGUI yeniSatir = Instantiate(prefebText, svContent);
        string tamMesaj = " Monk: " + yanit;
        yeniSatir.text = tamMesaj;
    }

    public TextMeshProUGUI DusunuyorTextiEkle()
    {
        TextMeshProUGUI yeniSatir = Instantiate(prefebText, svContent);
        yeniSatir.text = "    Düþünüyor...";
        return yeniSatir;
    }

    public void TextEkle(string metin)
    {
        TextMeshProUGUI yeniSatir = Instantiate(prefebText, svContent);
        yeniSatir.text = metin;
    }

    public void GuvenTextGuncelle(int guven)
    {
        guvenText.text = "Güven: " + guven+"/100";
        guvenSlider.value = guven;
        SliderRenginiAyarla(guven);
    }

    public void SliderRenginiAyarla(int guven)
    {
        if (guven < 0)
        {
            guvenSlider.targetGraphic.color = Color.red;
        }
        else if (guven >= 100)
        {
            guvenSlider.targetGraphic.color = Color.green;
        }
        else
        {
            guvenSlider.targetGraphic.color = Color.yellow;
        }
    }
}
