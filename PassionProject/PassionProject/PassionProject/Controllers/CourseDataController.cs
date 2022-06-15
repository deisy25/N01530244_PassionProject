using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PassionProject.Models;
using System.Diagnostics;

namespace PassionProject.Controllers
{
    public class CourseDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Return all Course in the system
        /// </summary>
        /// <returns>
        /// Content: all Course in the database
        /// </returns>
        /// <example>GET: api/CourseData/ListCourses</example>
        [HttpGet]
        [ResponseType(typeof(CourseDto))]
        public IHttpActionResult ListCourses()
        {
            List<Course> Courses = db.Courses.ToList();
            List<CourseDto> CourseDtos = new List<CourseDto>();

            Courses.ForEach(a => CourseDtos.Add(new CourseDto()
            {
                CourseId = a.CourseId,
                CourseName = a.CourseName,
                Level = a.Level,
                Hours = a.Hours,
                Price = a.Price,
                Day = a.Day,
                Time = a.Time,
                Description = a.Description
            }));

            return Ok(CourseDtos);
        }

        ///<summary>
        /// get the Course's data accroding to StudentID
        /// </summary>
        /// <returns>
        /// An student in the system matching up to the Course ID 
        /// </returns>
        /// <param name="id">it is a primary key of Course</param>
        ///<example>
        ///GET: api/Coursedata/findCourse/2
        ///</example>
        [HttpGet]
        [ResponseType(typeof(CourseDto))]
        public IHttpActionResult FindCourse(int id)
        {
            Course Course = db.Courses.Find(id);
            CourseDto CourseDto = new CourseDto()
            {
                CourseId = Course.CourseId,
                CourseName = Course.CourseName,
                Level = Course.Level,
                Hours = Course.Hours,
                Price = Course.Price,
                Day = Course.Day,
                Time = Course.Time,
                Description = Course.Description
            };

            return Ok(CourseDto);
        }

        /// <summary>
        /// Adding a Course to the system
        /// </summary>
        /// <param name="Course">JSON form data of a Course</param>
        /// <returns></returns>
        /// <example>
        /// POST: api/Coursetdata/addCourse
        /// </example>
        [HttpPost]
        [ResponseType(typeof(Course))]
        public IHttpActionResult AddCourse(Course Course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Courses.Add(Course);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Course.CourseId }, Course);
        }
    }
}
