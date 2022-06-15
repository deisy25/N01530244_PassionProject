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
    public class TeacherDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Return all Teacher in the system
        /// </summary>
        /// <returns>
        /// Content: all Teacher in the database
        /// </returns>
        /// <example>GET: api/TeacherData/ListTeachers</example>
        [HttpGet]
        [ResponseType(typeof(TeacherDto))]
        public IHttpActionResult ListTeachers()
        {
            List<Teacher> Teachers = db.Teachers.ToList();
            List<TeacherDto> TeacherDtos = new List<TeacherDto>();

            Teachers.ForEach(a => TeacherDtos.Add(new TeacherDto()
            {
                TeacherID = a.TeacherID,
                TeacherName = a.TeacherName,
                DateOfBirth = a.DateOfBirth,
                Address = a.Address,
                PostalCode = a.PostalCode,
                Salary = a.Salary
            }));

            return Ok(TeacherDtos);
        }

        /// <summary>
        /// Adding a Teacher to the system
        /// </summary>
        /// <param name="Teacher">JSON form data of a Teacher</param>
        /// <returns></returns>
        /// <example>
        /// POST: api/Teacherdata/addTeacher
        /// </example>
        [HttpPost]
        [ResponseType(typeof(Teacher))]
        public IHttpActionResult AddTeacher(Teacher Teacher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Teachers.Add(Teacher);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Teacher.TeacherID }, Teacher);
        }

        ///<summary>
        /// get the Teacher's data accroding to StudentID
        /// </summary>
        /// <returns>
        /// An student in the system matching up to the Teacher ID 
        /// </returns>
        /// <param name="id">it is a primary key of Teacher</param>
        ///<example>
        ///GET: api/Teacherdata/findTeacher/2
        ///</example>
        [HttpGet]
        [ResponseType(typeof(TeacherDto))]
        public IHttpActionResult FindTeacher(int id)
        {
            Teacher Teacher = db.Teachers.Find(id);
            TeacherDto TeacherDto = new TeacherDto()
            {
                TeacherID = Teacher.TeacherID,
                TeacherName = Teacher.TeacherName,
                DateOfBirth = Teacher.DateOfBirth,
                Address = Teacher.Address,
                PostalCode = Teacher.PostalCode,
                Salary = Teacher.Salary
            };

            return Ok(TeacherDto);
        }

        /// <summary>
        /// Update a Teacher data in the system with POST data input
        /// </summary>
        /// <param name="ID">represent the Teacher ID as primary key</param>
        /// <param name="Student">JSON form data of a Teacher</param>
        /// <returns></returns>
        /// <example>
        /// POST: api/Teacherdata/UpdateTeacher/5
        /// </example>
        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateTeacher(int id, Teacher Teacher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Teacher.TeacherID)
            {
                return BadRequest();
            }

            db.Entry(Teacher).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherExists(id))
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
        /// Deletes an Teacher from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Teacher</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/StudentTeacher/DeleteTeacher/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Teacher))]
        [HttpPost]
        public IHttpActionResult DeleteTeacher(int id)
        {
            Teacher Teacher = db.Teachers.Find(id);
            if (Teacher == null)
            {
                return NotFound();
            }

            db.Teachers.Remove(Teacher);
            db.SaveChanges();

            return Ok();
        }

        private bool TeacherExists(int id)
        {
            return db.Teachers.Count(e => e.TeacherID == id) > 0;
        }


        /// <summary>
        /// Returns all Course in the system associated with a particular teacher.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Course in the database taught by Teacher
        /// </returns>
        /// <param name="id">      /// <summary>
        /// Returns all Course in the system associated with a particular Student.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Course in the database taken by student
        /// </returns>
        /// <param name="id">Student Primary Key</param>
        /// <example>
        /// GET: api/CourseData/listCourseTaught/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(CourseDto))]
        public IHttpActionResult listCourseTaught(int id)
        {
            List<Teacher> Teachers = db.Teachers.Where(
                k => k.Courses.Any(
                    a => a.CourseId == id)
                ).ToList();
            List<TeacherDto> TeacherDtos = new List<TeacherDto>();

            Teachers.ForEach(k => TeacherDtos.Add(new TeacherDto()
            {
                TeacherID = k.TeacherID,
                TeacherName = k.TeacherName

            }));

            return Ok(TeacherDtos);
        }
        /// <summary>
        /// Returns all Course in the system associated with a particular Teacher.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Course in the database Taught by TEacher
        /// </returns>
        /// <param name="id">Student Primary Key</param>
        /// <example>
        /// GET: api/CourseData/listCourseTaken/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(CourseDto))]
        public IHttpActionResult listCourseTaken(int id)
        {
            List<Student> Students = db.Students.Where(
                k => k.Courses.Any(
                    a => a.CourseId == id)
                ).ToList();
            List<StudentDto> StudentDtos = new List<StudentDto>();

            Students.ForEach(k => StudentDtos.Add(new StudentDto()
            {
                StudentID = k.StudentID,
                StudentName = k.StudentName

            }));

            return Ok(StudentDtos);
        }

    }
}
