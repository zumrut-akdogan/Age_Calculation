using AgeCalculation.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace AgeCalculation.Web.Context
{
    public class AgeCalculationDbContext : DbContext
    {
        // Burada AgeCalculationDbContext sınıfı oluşturduk. Bu sınıf DbContext sınıfından kalıtım almaktadır.
        // Bu sınıfın içerisinde veritabanı bağlantısı için gerekli olan ayarlar yapılmaktadır.
        // Bu sınıfın içerisindeki Person sınıfı DbSet olarak tanımlanmıştır. Bu sayede Person sınıfı veritabanında bir tablo olarak oluşturulacaktır.
        // Bu sınıfın içerisindeki Person sınıfının özellikleri veritabanında birer sütun olarak oluşturulacaktır.
        // Bu sınıfta sql server veritabanı kullanılmaktadır. Bu yüzden OnConfiguring metodunda sql server veritabanı bağlantı adresi belirtilmiştir.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=ABDULLAH;Initial Catalog=AgeCalculationDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }

        
        // Bu sınıfın içerisindeki Person sınıfı DbSet olarak tanımlanmıştır. Bu sayede Person sınıfı veritabanında bir tablo olarak oluşturulacaktır.
        public DbSet<Person> Persons { get; set; }


    }
}
