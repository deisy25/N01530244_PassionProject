using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject.Models.ViewModel
{
    public class DetailTeacher
    {
        public TeacherDto SelectedTeacher { get; set; }
        public IEnumerable<CourseDto> CoursesTaught { get; set; }
    }
}