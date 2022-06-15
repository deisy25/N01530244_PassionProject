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

    }
}
