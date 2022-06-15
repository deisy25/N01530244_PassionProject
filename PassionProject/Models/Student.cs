using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PassionProject.Models
{
    public class Student
    {
        [Key]
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }

        //A student can take many courses
        public ICollection<Course> Courses { get; set; }
    }

    public class StudentDto
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
    }
}