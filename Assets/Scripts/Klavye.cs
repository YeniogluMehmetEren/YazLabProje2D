using TMPro;
using UnityEngine;

public class Klavye : MonoBehaviour
{
    public bool isTypeing;

    public TMP_InputField mesaj;

    public NPCKontrolu npcKontrolcusu;

    void Start()
    {
        mesaj.onSubmit.AddListener(delegate { TextAiGonder(); });
        InputAl();
    }

    public void TextAiGonder()
    {
        if (mesaj.text.Trim().Length > 0 && npcKontrolcusu != null)
        {
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
}
