using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace DeansOffice
{
    public class Student
    {
        public int IdNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public int Telephone { get; set; }
        public string Pesel { get; set; }
        public string PassportNumber { get; set; }
        public string Citizenship { get; set; }
        public string CityofBirth { get; set; }
        public DateTime DataofBirth { get; set; }
        public String Faculty { get; set; }
        public DateTime RegisteredYear { get; set; }
        public Image PhotoLink { get; set; }
    }
}
