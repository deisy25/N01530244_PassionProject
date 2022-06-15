using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject.Models.ViewModel
{
    public class DetailCourse
    {
        public CourseDto SelectedCourse { get; set; }

        public IEnumerable<StudentDto> StudentTaken { get; set; }
        public IEnumerable<StudentDto> AvailableStudent { get; set; }
        public IEnumerable<TeacherDto> TeacherTaught { get; set; }
    }
}