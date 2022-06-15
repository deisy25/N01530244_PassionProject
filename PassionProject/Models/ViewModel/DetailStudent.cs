using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject.Models.ViewModel
{
    public class DetailStudent
    {
        public StudentDto SelectedStudent { get; set; }
        public IEnumerable<CourseDto> CoursesTaken { get; set; }
    }
}