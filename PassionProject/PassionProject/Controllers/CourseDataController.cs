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

        /// <summary>
        /// Update a Course data in the system with POST data input
        /// </summary>
        /// <param name="ID">represent the Course ID as primary key</param>
        /// <param name="Student">JSON form data of a Course</param>
        /// <returns></returns>
        /// <example>
        /// POST: api/Coursetdata/UpdateCourse/5
        /// </example>
        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateCourse(int id, Course Course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Course.CourseId)
            {
                return BadRequest();
            }

            db.Entry(Course).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Deletes an Course from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Course</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/CoursetData/DeleteCourse/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Course))]
        [HttpPost]
        public IHttpActionResult DeleteCourse(int id)
        {
            Course Course = db.Courses.Find(id);
            if (Course == null)
            {
                return NotFound();
            }

            db.Courses.Remove(Course);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Returns all Course in the system associated with a particular Student.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Course in the database taken by student
        /// </returns>
        /// <param name="id">Student Primary Key</param>
        /// <example>
        /// GET: api/CourseData/listcoursesforstudent/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(CourseDto))]
        public IHttpActionResult listcoursesforstudent(int id)
        {
            List<Course> Courses = db.Courses.Where(
                k => k.Students.Any(
                    a => a.StudentID == id)
                ).ToList();
            List<CourseDto> CourseDtos = new List<CourseDto>();

            Courses.ForEach(k => CourseDtos.Add(new CourseDto()
            {
                CourseId= k.CourseId,
                CourseName= k.CourseName

            }));

            return Ok(CourseDtos);
        }

        /// <summary>
        /// Returns all Course in the system associated with a particular Teacher.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Course in the database taught by Teacher
        /// </returns>
        /// <param name="id">Teacher Primary Key</param>
        /// <example>
        /// GET: api/CourseData/listcoursesforTeacher/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(CourseDto))]
        public IHttpActionResult listcoursesforTeacher(int id)
        {
            List<Course> Courses = db.Courses.Where(
                k => k.Teachers.Any(
                    a => a.TeacherID == id)
                ).ToList();
            List<CourseDto> CourseDtos = new List<CourseDto>();

            Courses.ForEach(k => CourseDtos.Add(new CourseDto()
            {
                CourseId = k.CourseId,
                CourseName = k.CourseName

            }));

            return Ok(CourseDtos);
        }

        private bool CourseExists(int id)
        {
            return db.Courses.Count(e => e.CourseId == id) > 0;
        }
    }
}
