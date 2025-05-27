# OpenSchema

**Esnek. Hızlı. Evrensel.**  
Veri tabanı erişimini ve yönetimini bir üst seviyeye taşıyan açık, generic framework.

---

## 🚀 Nedir?

**OpenSchema**, farklı veri tabanlarını (SQL, NoSQL, dosya tabanlı vb.) ve tabloları/collection'ları dinamik olarak yönetmek isteyen geliştiriciler için geliştirilmiş bir data-access framework’üdür.  
Tek bir generic arayüz üzerinden tüm CRUD işlemlerini kolayca ve güvenle gerçekleştirin!

---

## 🎯 Kimler İçin?

- Hızlı prototipleme ve MVP geliştirenler
- Çoklu veri tabanı veya tablo/collection ile çalışan ekipler
- Esnek, ölçeklenebilir, tekrar kullanılabilir bir erişim katmanı arayan yazılımcılar
- İç araç, demo, script ve veri yönetim uygulaması geliştirenler

---

## 💡 Slogan

> **OpenSchema: Veri Tabanda Sınırları Kaldır.**

---

## ⚡️ Temel Özellikler

- Tablo/collection adını parametreyle kullanabilme
- LiteDB, MongoDB, SQL ve diğer veritabanı türlerine hızlı entegrasyon
- Plug-and-play veri tabanı servisleri
- Tek arayüz, çoklu kaynak
- Açık kaynak, topluluğa açık geliştirme

---

## 🛠️ Örnek Kullanım

```csharp
// OpenSchema ile LiteDB'de tüm ürünleri çek
var db = DbFramework.GetService("litedb", "mydata.db");
var products = db.GetAll("products");

// MongoDB'de yeni müşteri ekle
var mdb = DbFramework.GetService("mongodb", "mongodb://localhost:27017", "mydb");
mdb.Insert("customers", new { Name = "Alice", Age = 30 });
```

---

## 🌐 Katkıda Bulunun

Açık kaynak felsefesiyle geliştirildi!  
Yeni veri tabanı entegrasyonları, hata düzeltmeleri ve geliştirmeler için katkılarınızı bekliyoruz.

---

## 📣 Lisans

MIT

---

**OpenSchema ile verinin sınırlarını siz belirleyin!**