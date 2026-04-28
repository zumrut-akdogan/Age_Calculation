using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace AgeCalculation.Web.Models
{


    // Burada Person sınıfı oluşturduk. Bu sınıfın içerisinde kişinin bilgileri bulunmaktadır.
    public class Person
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public string Gender { get; set; }

        public int Age { get; set; }

        public string? PhotoPath { get; set; }
    }

}
