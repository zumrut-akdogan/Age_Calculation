using System.ComponentModel.DataAnnotations;

namespace AgeCalculation.Web.ViewModels
{
    // Burada PersonViewModel sınıfı oluşturduk. Bu sınıfın içerisinde kişinin bilgileri bulunmaktadır.
    // PersonViewModel sınıfı Person sınıfından farklı olarak DataAnnotations kullanılarak oluşturulmuştur.
    // DataAnnotations kullanılarak oluşturulan sınıfların içerisindeki özelliklerin başına [Required] yazılarak bu özelliklerin zorunlu olması sağlanmıştır.
    // [Display(Name = "Doğum Tarihi")] yazılarak bu özelliğin ekranda "Doğum Tarihi" olarak gözükmesi sağlanmıştır.
    public class PersonViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Ad alanı zorunludur.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Soyad alanı zorunludur.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Doğum Tarihi alanı zorunludur.")]
        [Display(Name = "Doğum Tarihi")]
        public DateTime BirthDate { get; set; }
        [Required(ErrorMessage = "Cinsiyet alanı zorunludur.")]
        public string Gender { get; set; }
        public int Age { get; set; }
        [Display(Name = "Fotoğraf")]
        public IFormFile? Photo { get; set; }
    }
}
