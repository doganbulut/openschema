# OpenSchema

**Flexible. Fast. Universal.**
An open, generic framework that takes database access and management to the next level.

---

## 🚀 What is it?

**OpenSchema** is a data-access framework developed for developers who want to dynamically manage different databases (SQL, NoSQL, file-based, etc.) and tables/collections.
Easily and securely perform all CRUD operations through a single generic interface!

---

## 🎯 Who is it for?

- Those developing rapid prototypes and MVPs
- Teams working with multiple databases or tables/collections
- Developers looking for a flexible, scalable, reusable access layer
- Those developing internal tools, demos, scripts, and data management applications

---

## 💡 Slogan

> **OpenSchema: Remove Data Boundaries.**

---

## ⚡️ Key Features

- Ability to use table/collection name as a parameter
- Fast integration with LiteDB, MongoDB, SQL, and other database types
- Plug-and-play database services
- Single interface, multiple sources
- Open source, community-driven development

---

## 🛠️ Example Usage

```csharp
// Fetch all products from LiteDB with OpenSchema
var db = DbFramework.GetService("litedb", "mydata.db");
var products = db.GetAll("products");

// Add a new customer in MongoDB
var mdb = DbFramework.GetService("mongodb", "mongodb://localhost:27017", "mydb");
mdb.Insert("customers", new { Name = "Alice", Age = 30 });
```

---

## 🌐 Contribute

Developed with an open-source philosophy!
We welcome your contributions for new database integrations, bug fixes, and enhancements.

---

## 📣 License

MIT

---

**With OpenSchema, you define the boundaries of your data!**

---

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
