using UnityEngine;
using UnityEditor;
using System.IO;

public class TorusCreator : MonoBehaviour
{
    [MenuItem("Tools/Donut Sprite Oluþtur")]
    public static void GenerateDonut()
    {
        int size = 256; // Görselin boyutu (256x256)
        float outerRadius = 120f; // Halkanýn dýþ yarýçapý
        float innerRadius = 50f;  // Halkanýn iç (boþluk) yarýçapý

        // Þeffaf arka planlý yeni bir texture oluþtur
        Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
        Color transparent = new Color(0, 0, 0, 0);
        Color circleColor = Color.white; // Halkanýn rengi (Beyaz yaparsan Unity'de istediðin renge boyayabilirsin)

        Vector2 center = new Vector2(size / 2f, size / 2f);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                // Merkeze olan uzaklýðý hesapla
                float distance = Vector2.Distance(new Vector2(x, y), center);

                // Eðer piksel iç ve dýþ yarýçap arasýndaysa boya, deðilse þeffaf yap
                if (distance <= outerRadius && distance >= innerRadius)
                {
                    texture.SetPixel(x, y, circleColor);
                }
                else
                {
                    texture.SetPixel(x, y, transparent);
                }
            }
        }

        texture.Apply();

        // PNG olarak Assets klasörüne kaydet
        byte[] bytes = texture.EncodeToPNG();
        string path = Application.dataPath + "/DonutSprite.png";
        File.WriteAllBytes(path, bytes);

        // Projeyi yenile ki görsel anýnda Unity'de görünsün
        AssetDatabase.Refresh();
        Debug.Log("Harika! DonutSprite.png baþarýyla Assets klasörüne kaydedildi.");
    }
}
