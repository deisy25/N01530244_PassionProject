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
    public class StudentDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        /// <summary>
        /// Return all students in the system
        /// </summary>
        /// <returns>
        /// Content: all students in the database
        /// </returns>
        /// <example>GET: api/StudentData/ListStudents</example>
        [HttpGet]
        [ResponseType(typeof(StudentDto))]
        public IHttpActionResult ListStudents()
        {
            List<Student> Students = db.Students.ToList();
            List<StudentDto> StudentDtos = new List<StudentDto>();

            Students.ForEach(a => StudentDtos.Add(new StudentDto()
            {
                StudentID = a.StudentID,
                StudentName = a.StudentName,
                DateOfBirth = a.DateOfBirth,
                Address = a.Address,
                PostalCode = a.PostalCode
            }));

            return Ok(StudentDtos);
        }

         ///<summary>
         /// get the student's data accroding to StudentID
         /// </summary>
         /// <returns>
         /// An student in the system matching up to the student ID 
         /// </returns>
         /// <param name="id">it is a primary key of student</param>
         ///<example>
         ///GET: api/studentdata/findstudent/2
         ///</example>
         [HttpGet]
         [ResponseType(typeof(StudentDto))]
         public IHttpActionResult FindStudent(int id)
         {
             Student Student=db.Students.Find(id);
             StudentDto StudentDto = new StudentDto()
             {
                 StudentID = Student.StudentID,
                 StudentName = Student.StudentName,
                 DateOfBirth = Student.DateOfBirth,
                 Address = Student.Address,
                 PostalCode = Student.PostalCode
             };

             return Ok(StudentDto);
         }

        /// <summary>
        /// Adding a student to the system
        /// </summary>
        /// <param name="Student">JSON form data of a student</param>
        /// <returns></returns>
        /// <example>
        /// POST: api/studentdata/addStudent
        /// </example>
        [HttpPost]
        [ResponseType(typeof(Student))]
        public IHttpActionResult AddStudent(Student Student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Students.Add(Student);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Student.StudentID }, Student);
        }

        /// <summary>
        /// Update a student data in the system with POST data input
        /// </summary>
        /// <param name="ID">represent the Student ID as primary key</param>
        /// <param name="Student">JSON form data of a student</param>
        /// <returns></returns>
        /// <example>
        /// POST: api/studentdata/UpdateStudent/5
        /// </example>
        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateStudent(int id, Student Student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Student.StudentID)
            {
                return BadRequest();
            }

            db.Entry(Student).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
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
        /// Deletes an Student from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the student</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/StudentData/DeleteStudent/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Student))]
        [HttpPost]
        public IHttpActionResult DeleteStudent(int id)
        {
            Student Student = db.Students.Find(id);
            if (Student == null)
            {
                return NotFound();
            }

            db.Students.Remove(Student);
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

            })) ;

            return Ok(StudentDtos);
        }


        private bool StudentExists(int id)
        {
            return db.Students.Count(e => e.StudentID == id) > 0;
        }


    }
}
