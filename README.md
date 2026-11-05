[Yazlab.md](https://github.com/user-attachments/files/34439080/Yazlab.md)
[Uploading ReadME.md…]()
# 1. Proje Adı ve Amacı
Oyunumuz esir düşmüş yetenkli bir savaşçı ve arkadaşının  seyircileri eğlendirmek için şov dövüşlerine çıkmasını konu alıyor. Oyuncuya zorlanacağı ve alıştıkça heyecanını arttıran **zorlu** ve **keyifli** bir boss savaşı kurgulamaya çalıştık. Boss'un hasar almasını engelleyen bir kalkanı mevcut,oyuncu kalkanı kırmak için Boss'u sersemletir ve otonom karakter sersemletmeden faydalanarak bossun kalkanını kırar ve hasara açık hale getirir.Bu belli sürelerle tekrarlanır ta ki savaş bitene kadar.
* **Oyun Türü:** 2D / Souls-like
* **Geliştirme Ortamı:** Unity, C#

---

## 2. Otonom AI Kahramanın Karar Mekanizması
<img width="2634" height="4872" alt="işte buuu" src="https://github.com/user-attachments/assets/abaf84ad-8d18-4e6a-b379-8116d0685366" />

## 3. LLM Entegrasyonunun Mimarisi
Yerel LLM entegrasyonunun mimarisi, NPC'nin karakterini, oyunun bağlamını ve kesin JSON çıktısı verme kuralını tanımlayan bir sistem promptu tasarımı ile başlar. Oyuncu bir metin girdiğinde, bu girdiyi prompt ile paketleyerek yerel LLM sunucusuna iletir ve model, cümleyi analiz ederek yalnızca NPC'nin cevabını ve eylemin puanını barındıran bir JSON şeması döndürür. Bu veriler ayrıştırılarak NPC'nin diyalog metnini oyuncuya gösterir ve ikna barı güncellenir. Toplam puan belirlenen eşiğe ulaştığında ise arena kapısı açılır ve senaryo sonlanır.

<img width="2238" height="1506" alt="LLM yeee" src="https://github.com/user-attachments/assets/6f045c88-a4f7-47c8-89d5-e4a8a8d1fa93" />

## 4. Hata Yönetimi Stratejisi
Ollama sunucusunun kapalı olması veya ağın yanıt vermemesi gibi erişilemezlik durumları UnityWebRequest denetleyicisi ile tespit edilir. İkinci bir güvenlik önlemi olarak, LLM'in JSON şemasının dışına çıkıp bozuk veya geçersiz bir metin üretmesi ihtimaline karşı veri ayrıştırma işlemi try-catch bloğu içinde yapılır. Bu strateji sayesinde sistem, sunucu kapansa dahi hata verip donmak yerine süreci iptal ederek oyuncuya standart bir uyarı mesajı gösterir.

## 5. Uygulamaya Ait Görüntüler

---

## 6. Sonuç
**Başarılar** 
Boss yapay zekası (state machine) sayesinde ana karakterimiz ve boss arasındaki mesafeye göre şekillenen bir karar mekanizması başarıyla kodlandı.
Oyuncuyu gerçek bir şov maçına çıkmış gibi hissetirmek için yapılan çevre ve ses tasarımı yapıldı.
ML Agents kullanılarak otonom bir karakter tasarlandı.
OLLAMA - Gemma3:4b kullanılarak tutarlı bir NPC diyalog örüntüsü tasarlandı. 

**Sınırlar**
- Mekaniklerin daha oturaklı ve daha tok hissettirmesi için ekstra ince ayarlara vakit yetmedi.
- Oyunun bölüm tasarımı (level design) ve düşman çeşitliliği, projenin teslim süresine yetişmesi için temel düzeyde tutuldu.

**Olası Geliştirmeler**
İlerde projeye devam edersek ekstra boss savaşları ve her bir savaşını kazandıktan sonra karaktere tercihi oyuncuya bırakılacak şekilde güçlendirmeler sunulabilirdi.
Ekstra bir para sistemi ve satın alınabilecek geliştirmeler veya ürünler eklenebilirdi.

**Geliştiriciler:**
- [Senin Adın/GitHub Linkin](https://github.com/KaanKeriman) - Temel mekanikler,Seviye Tasarımı,Asset araştırması,Boss AI'ı (LLM değil),Boss Tasarımı, Bölge Tasarımı
- [Kerem'in Adı/GitHub Linki](https://github.com/keremin-profili) - Otonom AI Eğitimi,LLM NPC Tasarımı ve kodlanması,Level tasarımı,Asset araştırması
