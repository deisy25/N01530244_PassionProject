using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PassionProject.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Level { get; set; }
        public int Hours { get; set; }
        public decimal Price { get; set; }
        public string Day { get; set; }
        public string Time { get; set; }
        public string Description { get; set; }

        //a Courses can be taken by many students
        public ICollection<Student> Students { get;set;}

        //a courses can be taught by many teachers
        //public ICollection<Teacher> Teachers { get;set;}
    }

    public class CourseDto
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Level { get; set; }
        public int Hours { get; set; }
        public decimal Price { get; set; }
        public string Day { get; set; }
        public string Time { get; set; }
        public string Description { get; set; }
    }
}