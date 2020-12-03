using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeansOffice
{
    class Assessment
    {
        public int IdNumber { get; set; }
        public Double Value { get; set; }
        public Teacher TeacherName { get; set; }
        public Subject SubjectName { get; set; }
        public Student StudentName { get; set; }
    }
}
