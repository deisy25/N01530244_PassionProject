using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using PassionProject.Models;
using PassionProject.Models.ViewModel;
using System.Web.Script.Serialization;

namespace PassionProject.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static TeacherController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44379/api/");
        }

        //GET: Teacher/List
        public ActionResult List()
        {
            //objective: communicate with our Teacher data api to retrieve a list of Teacher
            //curl https://localhost:44379/api/teacherdata/listteachers

            string url = "teacherdata/listteachers";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<TeacherDto> Teachers = response.Content.ReadAsAsync<IEnumerable<TeacherDto>>().Result;
            return View(Teachers);
        }

        public ActionResult Error()
        {
            return View();
        }

        //GET: Student/New
        public ActionResult New()
        {
            string url = "teacherdata/listTeachers";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<TeacherDto> Teachers = response.Content.ReadAsAsync<IEnumerable<TeacherDto>>().Result;
            return View(Teachers);
        }

        //POST: Teacher/Create
        [HttpPost]
        public ActionResult Create(Teacher Teacher)
        {
            Debug.WriteLine("the json payload is :");
            //curl -H "Content-Type:application/json" -d @Teacher.json  https://localhost:44379/api/Teacherdata/addTeacher 
            string url = "Teacherdata/addTeacher";

            string jsonpayload = jss.Serialize(Teacher);
            Debug.WriteLine(jsonpayload);

            Debug.WriteLine("DOB: " + Teacher.DateOfBirth);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        //GET: Teacher/Detail/2
        public ActionResult Detail(int id)
        {
            DetailTeacher ViewModel = new DetailTeacher();

            //objective: communicate with our student data api to retrieve a list of students
            //curl https://localhost:44379/api/Teacherdata/FindTeacher/{id}

            string url = "Teacherdata/FindTeacher/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            TeacherDto SelectedTeacher = response.Content.ReadAsAsync<TeacherDto>().Result;
            ViewModel.SelectedTeacher = SelectedTeacher;

            url = "coursedata/listcoursesforteacher/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<CourseDto> CoursesTaught = response.Content.ReadAsAsync<IEnumerable<CourseDto>>().Result;
            ViewModel.CoursesTaught = CoursesTaught;
            return View(ViewModel);
        }

        //GET: Teacher/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateTeacher ViewModel = new UpdateTeacher();
            string url = "Teacherdata/FindTeacher/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            TeacherDto SelectedTeacher = response.Content.ReadAsAsync<TeacherDto>().Result;
            ViewModel.SelectedTeacher = SelectedTeacher;
            Debug.WriteLine(SelectedTeacher.DateOfBirth);
            return View(ViewModel);
        }

        //POST: Teacher/Update/5
        [HttpPost]
        public ActionResult Update(int id, Teacher Teacher)
        {
            string url = "Teacherdata/updateTeacher/" + id;
            string jsonpayload = jss.Serialize(Teacher);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Teacher/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "Teacherdata/findTeacher/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TeacherDto selectedTeacher = response.Content.ReadAsAsync<TeacherDto>().Result;
            return View(selectedTeacher);
        }

        // POST: Teacher/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "Teacherdata/deleteTeacher/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

    }
}