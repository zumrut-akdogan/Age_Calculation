using AgeCalculation.Web.Context;
using AgeCalculation.Web.Models;
using AgeCalculation.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using X.PagedList;

namespace AgeCalculation.Web.Controllers
{
    public class PersonsController : Controller
    {
        // Burada AgeCalculationDbContext ve IFileProvider nesnelerini oluşturduk.
        private readonly AgeCalculationDbContext _context; // Veritabanı işlemleri için gerekli nesne
        private readonly IFileProvider _fileProvider; // Fotoğraf işlemleri için gerekli nesne

        // Burada AgeCalculationDbContext ve IFileProvider nesnelerini constructor methoduna ekledik.
        public PersonsController(AgeCalculationDbContext context, IFileProvider fileProvider)
        {
            _context = context;
            _fileProvider = fileProvider;
        }

        public async Task<IActionResult> Index(string sortOrder, string searchString, int? page)
        {
            // Filtreleme parametrelerini ViewData ile view sayfasına gönder
            ViewData["SearchString"] = searchString;


            // Burada sıralama işlemleri için gerekli parametreleri oluşturduk.
            // ViewData ile bu parametreleri view sayfasına gönderdik.
            // View sayfasında bu parametrelere göre sıralama işlemlerini yaptık.
            // sortOder parametresi boş ise FirstName_desc olarak sıralama işlemi yapıyoruz.
            // 
            ViewData["FirstNameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "FirstName_desc" : "";
            ViewData["LastNameSortParm"] = sortOrder == "LastName_asc" ? "LastName_desc" : "LastName_asc";
            ViewData["GenderSortParm"] = sortOrder == "Gender_asc" ? "Gender_desc" : "Gender_asc";
            ViewData["BirthDateSortParm"] = sortOrder == "BirthDate_asc" ? "BirthDate_desc" : "BirthDate_asc";
            ViewData["AgeSortParm"] = sortOrder == "Age_asc" ? "Age_desc" : "Age_asc";
            
            // Burada sql de kaydedilen verileri çekiyoruz.
            var person = _context.Persons.AsQueryable();

            // Burada arama işlemleri için gerekli parametreleri oluşturduk.
            if (!string.IsNullOrEmpty(searchString))
            {
                // firstName veya lastName içinde arama işlemi yapıyoruz.
                person = person.Where(p => p.FirstName.Contains(searchString) || p.LastName.Contains(searchString) || p.Gender.Contains(searchString));
            }


            // Burada sıralama işlemlerini yaptık.
            person = sortOrder switch
            {
                // Burada person sortOrder FirstName_desc gelirse gereken method cağırılıyor.
                "FirstName_desc" => person.OrderByDescending(p => p.FirstName),
                "LastName_asc" => person.OrderBy(p => p.LastName),
                "LastName_desc" => person.OrderByDescending(p => p.LastName),
                "Gender_asc" => person.OrderBy(p => p.Gender),
                "Gender_desc" => person.OrderByDescending(p => p.Gender),
                "BirthDate_asc" => person.OrderBy(p => p.BirthDate),
                "BirthDate_desc" => person.OrderByDescending(p => p.BirthDate),
                "Age_asc" => person.OrderBy(p => p.Age),
                "Age_desc" => person.OrderByDescending(p => p.Age),
                _ => person.OrderBy(p => p.FirstName)
            };

            var pageSize = 10; // Her sayfada gösterilecek kişi sayısı
            var pageNumber = page ?? 1; // Sayfa numarasını URL parametresinden al, eğer boşsa 1 olarak ayarla


            // Burada PageList kullanarak sayfalama işlemlerini yaptık.
            var pagedPersons = await person.ToPagedListAsync(pageNumber, pageSize);

            
            return View(pagedPersons);


        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PersonViewModel personViewModel)
        {
            // Model doğrulama: hataları varsa, Create sayfasını yeniden göster
            if (!ModelState.IsValid) return View(personViewModel);





            // Kişi bilgilerini personViewModel'den al ve yeni bir Person nesnesi oluştur
            var person = new Person
            {
                FirstName = personViewModel.FirstName,
                LastName = personViewModel.LastName,
                Gender = personViewModel.Gender
            };

            var birthDate = personViewModel.BirthDate;
            if (DateTime.Now.Date >= birthDate.Date)
            {
                person.BirthDate = birthDate;
            }
            else
            {
                ViewData["Message"] = "Doğum tarihi bugünden büyük olamaz";
                return View();
            }


            // Burada yaş hesapla methodunu cağırıp person daki yaş propertisini hesapladık.
            var age = CalculateAge(person.BirthDate);
            if (age >= 0 )
            {
                person.Age = age;
            }
            else
            {
                ViewData["Message"] = "Yaş hesaplanamadı";
                return View();
            }


            // Burada eğer fotoğraf değeri null veya karekter uzunluğu 0 dan büyük ise fotoğraf ekleme işlemi yapıyoruz.
            if (personViewModel.Photo != null && personViewModel.Photo.Length > 0)
            {
                // Burada wwwroot klasörün dizisini aldık
                var wwwRootFolder = _fileProvider.GetDirectoryContents("wwwroot");

                // Burada benzersiz bir isim oluşturduk.
                var randomFileName = $"{Guid.NewGuid()}{Path.GetExtension(personViewModel.Photo.FileName)}";

                //Burada verilen "wwwRootFolder" adlı bir klasörün içindeki "images" adlı bir alt klasörün fiziksel yolunu alır. Daha sonra, bu yola, rastgele bir dosya adı olan "randomFileName" eklenir ve sonuç olarak yeni bir dosya yolu oluşturulur. 
                var newPicturePath = Path.Combine(wwwRootFolder!.First(x => x.Name == "images").PhysicalPath!,
                    randomFileName);

                // Burada yeni bir dosya oluşturup, "newPicturePath" adlı dosyaya kopyalıyoruz.
                await using var stream = new FileStream(newPicturePath, FileMode.Create);

                // Burada "personViewModel" adlı dosyayı "stream" adlı dosyaya kopyalıyoruz.
                await personViewModel.Photo.CopyToAsync(stream);

                // Burada "person" nesnesinin "PhotoPath" özelliğine "randomFileName" değerini atıyoruz.
                person.PhotoPath = randomFileName;
            }

            // Yeni kişiyi veritabanına ekle
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");

        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            // Burada veritabanından ki erkek ve kadın kişi sayısını çekiyoruz.
            var maleCount = _context.Persons.Count(p => p.Gender == "Erkek");
            var femaleCount = _context.Persons.Count(p => p.Gender == "Kadın");

            // Burada veritabanından ki yaş ortalamalarını çekiyoruz. Örnek: yaşı 0 ile 15 arası olan kisilerin yaş ortalamasını al
            var ageGroup1Average = _context.Persons.Where(p => p.Age >= 0 && p.Age <= 15).Average(p => p.Age);
            var ageGroup2Average = _context.Persons.Where(p => p.Age > 15 && p.Age <= 30).Average(p => p.Age);
            var ageGroup3Average = _context.Persons.Where(p => p.Age > 30 && p.Age <= 45).Average(p => p.Age);
            var ageGroup4Average = _context.Persons.Where(p => p.Age > 45).Average(p => p.Age);



             
            var genderData = new
            {
                labels = new[] { "Erkek", "Kadın" }, // Cinsiyet verilerinin etiketleri

                datasets = new[]
                {
                    new
                    {
                        data = new[] { maleCount, femaleCount }, // Erkek ve kadın sayıları
                        backgroundColor = new[] { "#36a2eb", "#ff6384" } // Veri görselleştirmesinin arka plan renkleri
                    }
                }
            };

            var ageData = new
            {
                labels = new[] { "0-15", "15-30", "30-45", "45+" }, // Yaş verilerinin etiketleri
                datasets = new[]
                {
                    new
                    {
                        label = "Yaş Ortalamaları", // Veri görselleştirmesinin başlığı
                        data = new[] { ageGroup1Average, ageGroup2Average, ageGroup3Average, ageGroup4Average }, // Yaş gruplarının ortalamaları
                        backgroundColor = "#4bc0c0" // Veri görselleştirmesinin arka plan rengi
                    }
                }
            };

            // Burada oluşturduğumuz verileri viewbag e atıyoruz.
            ViewBag.genderData = JsonConvert.SerializeObject(genderData);
            ViewBag.ageData = JsonConvert.SerializeObject(ageData);

            return View();
        }


        // Burada yaş hesaplama methodunu yazdık.
        [NonAction]
        public static int CalculateAge(DateTime birthDate)
        {
            DateTime today = DateTime.Today; // Günün tarihini aldık
            int age = today.Year - birthDate.Year; //Bugünün yılını gelen doğum tarihinin yolında cıkarığ age depişkenine atıyoruz
            if (birthDate > today.AddYears(-age)) // Eğer doğum tarihi bugünden büyükse yaşa 1 ekliyoruz
            {
                age--;
            }
            return age;
        }

    }
}
