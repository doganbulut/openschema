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
using OpenSchema;
using System;
using System.Collections.Generic;

// --- LiteDB Example ---
// Initialize LiteDB service
var liteDb = OpenDb.GetService("litedb", "mydata.db");

// Define a simple Product class
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}

// Insert a new product
var newProduct = new Product { Name = "Laptop", Price = 1200.00m };
liteDb.Insert("products", newProduct);
Console.WriteLine($"Inserted product: {newProduct.Name}");

// Get all products
var allProducts = liteDb.GetAll<Product>("products");
Console.WriteLine("All products in LiteDB:");
foreach (var p in allProducts)
{
    Console.WriteLine($"- Id: {p.Id}, Name: {p.Name}, Price: {p.Price}");
}

// Update a product (assuming Id is set after insert or retrieved)
if (allProducts.Count > 0)
{
    var productToUpdate = allProducts[0];
    productToUpdate.Price = 1250.00m;
    liteDb.Update("products", productToUpdate);
    Console.WriteLine($"Updated product: {productToUpdate.Name} to new price {productToUpdate.Price}");
}

// Delete a product
if (allProducts.Count > 0)
{
    var productToDelete = allProducts[0];
    liteDb.Delete("products", productToDelete.Id);
    Console.WriteLine($"Deleted product with Id: {productToDelete.Id}");
}


// --- MongoDB Example ---
// Initialize MongoDB service
// Ensure MongoDB is running at mongodb://localhost:27017
var mongoDb = OpenDb.GetService("mongodb", "mongodb://localhost:27017", "mydb");

// Define a simple Customer class
public class Customer
{
    public string Id { get; set; } // MongoDB uses string Id
    public string Name { get; set; }
    public int Age { get; set; }
}

// Insert a new customer
var newCustomer = new Customer { Name = "Alice", Age = 30 };
mongoDb.Insert("customers", newCustomer);
Console.WriteLine($"Inserted customer: {newCustomer.Name}");

// Get all customers
var allCustomers = mongoDb.GetAll<Customer>("customers");
Console.WriteLine("All customers in MongoDB:");
foreach (var c in allCustomers)
{
    Console.WriteLine($"- Id: {c.Id}, Name: {c.Name}, Age: {c.Age}");
}

// Update a customer (assuming Id is set after insert or retrieved)
if (allCustomers.Count > 0)
{
    var customerToUpdate = allCustomers[0];
    customerToUpdate.Age = 31;
    mongoDb.Update("customers", customerToUpdate);
    Console.WriteLine($"Updated customer: {customerToUpdate.Name} to new age {customerToUpdate.Age}");
}

// Delete a customer
if (allCustomers.Count > 0)
{
    var customerToDelete = allCustomers[0];
    mongoDb.Delete("customers", customerToDelete.Id);
    Console.WriteLine($"Deleted customer with Id: {customerToDelete.Id}");
}


// --- RedisDB Example ---
// Initialize RedisDB service
// Ensure Redis is running at localhost:6379
var redisDb = OpenDb.GetService("redisdb", "localhost:6379");

// Redis typically stores key-value pairs.
// For complex objects, you might serialize them to JSON.
public class User
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
}

var newUser = new User { Id = "user:1", Username = "john_doe", Email = "john@example.com" };
redisDb.Insert("users", newUser.Id, newUser); // Redis Insert might take key and value
Console.WriteLine($"Inserted user to Redis: {newUser.Username}");

var retrievedUser = redisDb.GetById<User>("users", newUser.Id);
Console.WriteLine($"Retrieved user from Redis: {retrievedUser?.Username}");

// Update user
retrievedUser.Email = "john.doe@newdomain.com";
redisDb.Update("users", retrievedUser.Id, retrievedUser);
Console.WriteLine($"Updated user email in Redis: {retrievedUser.Email}");

// Delete user
redisDb.Delete("users", newUser.Id);
Console.WriteLine($"Deleted user from Redis: {newUser.Id}");


// --- PostgreSQL Example ---
// Initialize PostgreSQL service
// Ensure PostgreSQL is running and database 'mydatabase' exists
// Connection string example: "Host=localhost;Port=5432;Username=myuser;Password=mypassword;Database=mydatabase"
var pgDb = OpenDb.GetService("postgresqldb", "Host=localhost;Port=5432;Username=postgres;Password=mysecretpassword;Database=mydatabase");

// Define a simple Order class
public class Order
{
    public int Id { get; set; }
    public string Item { get; set; }
    public int Quantity { get; set; }
}

// Insert a new order
var newOrder = new Order { Item = "Book", Quantity = 2 };
pgDb.Insert("orders", newOrder);
Console.WriteLine($"Inserted order: {newOrder.Item}");

// Get all orders
var allOrders = pgDb.GetAll<Order>("orders");
Console.WriteLine("All orders in PostgreSQL:");
foreach (var o in allOrders)
{
    Console.WriteLine($"- Id: {o.Id}, Item: {o.Item}, Quantity: {o.Quantity}");
}

// Update an order
if (allOrders.Count > 0)
{
    var orderToUpdate = allOrders[0];
    orderToUpdate.Quantity = 3;
    pgDb.Update("orders", orderToUpdate);
    Console.WriteLine($"Updated order: {orderToUpdate.Item} to new quantity {orderToUpdate.Quantity}");
}

// Delete an order
if (allOrders.Count > 0)
{
    var orderToDelete = allOrders[0];
    pgDb.Delete("orders", orderToDelete.Id);
    Console.WriteLine($"Deleted order with Id: {orderToDelete.Id}");
}


// --- SQLiteDB Example ---
// Initialize SQLiteDB service
var sqliteDb = OpenDb.GetService("sqlitedb", "mydatabase.sqlite");

// Define a simple LogEntry class
public class LogEntry
{
    public int Id { get; set; }
    public string Message { get; set; }
    public DateTime Timestamp { get; set; }
}

// Insert a new log entry
var newLog = new LogEntry { Message = "Application started.", Timestamp = DateTime.Now };
sqliteDb.Insert("logs", newLog);
Console.WriteLine($"Inserted log entry: {newLog.Message}");

// Get all log entries
var allLogs = sqliteDb.GetAll<LogEntry>("logs");
Console.WriteLine("All log entries in SQLite:");
foreach (var log in allLogs)
{
    Console.WriteLine($"- Id: {log.Id}, Message: {log.Message}, Timestamp: {log.Timestamp}");
}

// Update a log entry
if (allLogs.Count > 0)
{
    var logToUpdate = allLogs[0];
    logToUpdate.Message = "Application running smoothly.";
    sqliteDb.Update("logs", logToUpdate);
    Console.WriteLine($"Updated log entry: {logToUpdate.Message}");
}

// Delete a log entry
if (allLogs.Count > 0)
{
    var logToDelete = allLogs[0];
    sqliteDb.Delete("logs", logToDelete.Id);
    Console.WriteLine($"Deleted log entry with Id: {logToDelete.Id}");
}
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
using OpenSchema;
using System;
using System.Collections.Generic;

// --- LiteDB Örneği ---
// LiteDB servisini başlat
var liteDb = OpenDb.GetService("litedb", "mydata.db");

// Basit bir Ürün sınıfı tanımla
public class Urun
{
    public int Id { get; set; }
    public string Ad { get; set; }
    public decimal Fiyat { get; set; }
}

// Yeni bir ürün ekle
var yeniUrun = new Urun { Ad = "Dizüstü Bilgisayar", Fiyat = 1200.00m };
liteDb.Insert("urunler", yeniUrun);
Console.WriteLine($"Eklenen ürün: {yeniUrun.Ad}");

// Tüm ürünleri çek
var tumUrunler = liteDb.GetAll<Urun>("urunler");
Console.WriteLine("LiteDB'deki tüm ürünler:");
foreach (var u in tumUrunler)
{
    Console.WriteLine($"- Id: {u.Id}, Ad: {u.Ad}, Fiyat: {u.Fiyat}");
}

// Bir ürünü güncelle (Id eklemeden sonra ayarlanmış veya alınmış varsayılarak)
if (tumUrunler.Count > 0)
{
    var guncellenecekUrun = tumUrunler[0];
    guncellenecekUrun.Fiyat = 1250.00m;
    liteDb.Update("urunler", guncellenecekUrun);
    Console.WriteLine($"Güncellenen ürün: {guncellenecekUrun.Ad} yeni fiyatı {guncellenecekUrun.Fiyat}");
}

// Bir ürünü sil
if (tumUrunler.Count > 0)
{
    var silinecekUrun = tumUrunler[0];
    liteDb.Delete("urunler", silinecekUrun.Id);
    Console.WriteLine($"Id'si silinen ürün: {silinecekUrun.Id}");
}


// --- MongoDB Örneği ---
// MongoDB servisini başlat
// MongoDB'nin mongodb://localhost:27017 adresinde çalıştığından emin olun
var mongoDb = OpenDb.GetService("mongodb", "mongodb://localhost:27017", "mydb");

// Basit bir Müşteri sınıfı tanımla
public class Musteri
{
    public string Id { get; set; } // MongoDB string Id kullanır
    public string Ad { get; set; }
    public int Yas { get; set; }
}

// Yeni bir müşteri ekle
var yeniMusteri = new Musteri { Ad = "Ayşe", Yas = 30 };
mongoDb.Insert("musteriler", yeniMusteri);
Console.WriteLine($"Eklenen müşteri: {yeniMusteri.Ad}");

// Tüm müşterileri çek
var tumMusteriler = mongoDb.GetAll<Musteri>("musteriler");
Console.WriteLine("MongoDB'deki tüm müşteriler:");
foreach (var m in tumMusteriler)
{
    Console.WriteLine($"- Id: {m.Id}, Ad: {m.Ad}, Yas: {m.Yas}");
}

// Bir müşteriyi güncelle (Id eklemeden sonra ayarlanmış veya alınmış varsayılarak)
if (tumMusteriler.Count > 0)
{
    var guncellenecekMusteri = tumMusteriler[0];
    guncellenecekMusteri.Yas = 31;
    mongoDb.Update("musteriler", guncellenecekMusteri);
    Console.WriteLine($"Güncellenen müşteri: {guncellenecekMusteri.Ad} yeni yaşı {guncellenecekMusteri.Yas}");
}

// Bir müşteriyi sil
if (tumMusteriler.Count > 0)
{
    var silinecekMusteri = tumMusteriler[0];
    mongoDb.Delete("musteriler", silinecekMusteri.Id);
    Console.WriteLine($"Id'si silinen müşteri: {silinecekMusteri.Id}");
}


// --- RedisDB Örneği ---
// RedisDB servisini başlat
// Redis'in localhost:6379 adresinde çalıştığından emin olun
var redisDb = OpenDb.GetService("redisdb", "localhost:6379");

// Redis genellikle anahtar-değer çiftleri saklar.
// Karmaşık nesneler için JSON'a seri hale getirebilirsiniz.
public class Kullanici
{
    public string Id { get; set; }
    public string KullaniciAdi { get; set; }
    public string Eposta { get; set; }
}

var yeniKullanici = new Kullanici { Id = "kullanici:1", KullaniciAdi = "can_yılmaz", Eposta = "can@example.com" };
redisDb.Insert("kullanicilar", yeniKullanici.Id, yeniKullanici); // Redis Insert anahtar ve değer alabilir
Console.WriteLine($"Redis'e eklenen kullanıcı: {yeniKullanici.KullaniciAdi}");

var alinanKullanici = redisDb.GetById<Kullanici>("kullanicilar", yeniKullanici.Id);
Console.WriteLine($"Redis'ten alınan kullanıcı: {alinanKullanici?.KullaniciAdi}");

// Kullanıcıyı güncelle
alinanKullanici.Eposta = "can.yilmaz@yenidomain.com";
redisDb.Update("kullanicilar", alinanKullanici.Id, alinanKullanici);
Console.WriteLine($"Redis'te güncellenen kullanıcı e-postası: {alinanKullanici.Eposta}");

// Kullanıcıyı sil
redisDb.Delete("kullanicilar", yeniKullanici.Id);
Console.WriteLine($"Redis'ten silinen kullanıcı Id: {yeniKullanici.Id}");


// --- PostgreSQL Örneği ---
// PostgreSQL servisini başlat
// PostgreSQL'in çalıştığından ve 'veritabanım' adlı veritabanının var olduğundan emin olun
// Bağlantı dizesi örneği: "Host=localhost;Port=5432;Username=kullaniciadim;Password=sifrem;Database=veritabanım"
var pgDb = OpenDb.GetService("postgresqldb", "Host=localhost;Port=5432;Username=postgres;Password=mysecretpassword;Database=mydatabase");

// Basit bir Sipariş sınıfı tanımla
public class Siparis
{
    public int Id { get; set; }
    public string Oge { get; set; }
    public int Miktar { get; set; }
}

// Yeni bir sipariş ekle
var yeniSiparis = new Siparis { Oge = "Kitap", Miktar = 2 };
pgDb.Insert("siparisler", yeniSiparis);
Console.WriteLine($"Eklenen sipariş: {yeniSiparis.Oge}");

// Tüm siparişleri çek
var tumSiparisler = pgDb.GetAll<Siparis>("siparisler");
Console.WriteLine("PostgreSQL'deki tüm siparişler:");
foreach (var s in tumSiparisler)
{
    Console.WriteLine($"- Id: {s.Id}, Oge: {s.Oge}, Miktar: {s.Miktar}");
}

// Bir siparişi güncelle
if (tumSiparisler.Count > 0)
{
    var guncellenecekSiparis = tumSiparisler[0];
    guncellenecekSiparis.Miktar = 3;
    pgDb.Update("siparisler", guncellenecekSiparis);
    Console.WriteLine($"Güncellenen sipariş: {guncellenecekSiparis.Oge} yeni miktarı {guncellenecekSiparis.Miktar}");
}

// Bir siparişi sil
if (tumSiparisler.Count > 0)
{
    var silinecekSiparis = tumSiparisler[0];
    pgDb.Delete("siparisler", silinecekSiparis.Id);
    Console.WriteLine($"Id'si silinen sipariş: {silinecekSiparis.Id}");
}


// --- SQLiteDB Örneği ---
// SQLiteDB servisini başlat
var sqliteDb = OpenDb.GetService("sqlitedb", "veritabanim.sqlite");

// Basit bir LogKaydi sınıfı tanımla
public class LogKaydi
{
    public int Id { get; set; }
    public string Mesaj { get; set; }
    public DateTime ZamanDamgasi { get; set; }
}

// Yeni bir log kaydı ekle
var yeniLog = new LogKaydi { Mesaj = "Uygulama başlatıldı.", ZamanDamgasi = DateTime.Now };
sqliteDb.Insert("kayitlar", yeniLog);
Console.WriteLine($"Eklenen log kaydı: {yeniLog.Mesaj}");

// Tüm log kayıtlarını çek
var tumKayitlar = sqliteDb.GetAll<LogKaydi>("kayitlar");
Console.WriteLine("SQLite'daki tüm log kayıtları:");
foreach (var log in tumKayitlar)
{
    Console.WriteLine($"- Id: {log.Id}, Mesaj: {log.Mesaj}, ZamanDamgasi: {log.ZamanDamgasi}");
}

// Bir log kaydını güncelle
if (tumKayitlar.Count > 0)
{
    var guncellenecekLog = tumKayitlar[0];
    guncellenecekLog.Mesaj = "Uygulama sorunsuz çalışıyor.";
    sqliteDb.Update("kayitlar", guncellenecekLog);
    Console.WriteLine($"Güncellenen log kaydı: {guncellenecekLog.Mesaj}");
}

// Bir log kaydını sil
if (tumKayitlar.Count > 0)
{
    var silinecekLog = tumKayitlar[0];
    sqliteDb.Delete("kayitlar", silinecekLog.Id);
    Console.WriteLine($"Id'si silinen log kaydı: {silinecekLog.Id}");
}
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
