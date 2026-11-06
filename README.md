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
<img width="1917" height="1078" alt="Ekran görüntüsü 2026-11-06 164757" src="https://github.com/user-attachments/assets/53d0fe24-9550-4a29-b384-f0081d633b78" />

<img width="1916" height="1078" alt="Ekran görüntüsü 2026-11-06 164815" src="https://github.com/user-attachments/assets/32f6fb47-6f2b-4e4a-bfb6-db0a23452aa0" />

<img width="1917" height="1078" alt="Ekran görüntüsü 2026-11-06 164840" src="https://github.com/user-attachments/assets/d3191b96-d053-4883-974a-f82aec148958" />

<img width="1030" height="392" alt="Ekran görüntüsü 2026-11-06 132935" src="https://github.com/user-attachments/assets/7ee3290d-2375-4805-b0b5-312f6fa663af" />

<img width="1031" height="523" alt="Ekran görüntüsü 2026-11-06 132927" src="https://github.com/user-attachments/assets/85b92e9d-3ada-4df4-ba1d-c3194b3b1cd1" />

<img width="1917" height="1078" alt="Ekran görüntüsü 2026-11-06 170341" src="https://github.com/user-attachments/assets/2634c9c7-bef9-4ce0-9883-0fc69ba398a0" />

<img width="1917" height="1078" alt="Ekran görüntüsü 2026-11-06 170437" src="https://github.com/user-attachments/assets/13347cde-927b-4d74-8ded-c74e42bd5fe8" />

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
- [KaanKeriman/(https://github.com/KaanKeriman) - Temel mekanikler,Seviye Tasarımı,Asset araştırması,Boss AI'ı (LLM değil),Boss Tasarımı, Bölge Tasarımı
- [Mehmet Eren Yenioğlu/(https://github.com/YeniogluMehmetEren) - Otonom AI Eğitimi,LLM NPC Tasarımı ve kodlanması,Level tasarımı,Asset araştırması
