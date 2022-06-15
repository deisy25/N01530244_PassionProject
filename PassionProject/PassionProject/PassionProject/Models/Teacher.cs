using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject.Models
{
    public class Teacher
    {
        public int TeacherID { get; set; }
        public string TeacherName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }

        public decimal Salary { get; set; }

        //A teacher can teach many courses
        //public ICollection<Course> Courses { get; set; }
    }

    public class TeacherDto
    {
        public int TeacherID { get; set; }
        public string TeacherName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public decimal Salary { get; set; }
    }
}