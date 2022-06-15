using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using PassionProject.Models;
using System.Web.Script.Serialization;
using PassionProject.Models.ViewModel;

namespace PassionProject.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static StudentController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44379/api/");
        }

        //GET: Student/List
        public ActionResult List()
        {
            //objective: communicate with our student data api to retrieve a list of students
            //curl https://localhost:44379/api/studentdata/liststudents

            string url = "studentdata/liststudents";
            HttpResponseMessage response = client.GetAsync(url).Result;
            Debug.WriteLine(response);
            IEnumerable<StudentDto> students = response.Content.ReadAsAsync<IEnumerable<StudentDto>>().Result;
            return View(students);
        }

        //GET: Student/Detail/2
        public ActionResult Detail(int id)
        {
            DetailStudent ViewModel = new DetailStudent();

            //objective: communicate with our student data api to retrieve a list of students
            //curl https://localhost:44379/api/studentdata/FindStudent/{id}

            string url = "studentdata/FindStudent/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            StudentDto SelectedStudent = response.Content.ReadAsAsync<StudentDto>().Result;

            ViewModel.SelectedStudent = SelectedStudent;
            return View(ViewModel);
        }

        public ActionResult Error()
        {
            return View();
        }

        //GET: Student/New
        public ActionResult New()
        {
            string url = "studentdata/liststudents";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<StudentDto> students = response.Content.ReadAsAsync<IEnumerable<StudentDto>>().Result;
            return View(students);
        }

        //POST: Student/Create
        [HttpPost]
        public ActionResult Create(Student Student)
        {
            Debug.WriteLine("the json payload is :");
            //curl -H "Content-Type:application/json" -d @student.json  https://localhost:44379/api/studentdata/addstudent 
            string url = "Studentdata/addstudent";

            string jsonpayload = jss.Serialize(Student);
            Debug.WriteLine(jsonpayload);

            Debug.WriteLine("DOB: " + Student.DateOfBirth);

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

        //GET: Student/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateStudent ViewModel = new UpdateStudent();
            string url = "studentdata/FindStudent/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            StudentDto SelectedStudent = response.Content.ReadAsAsync<StudentDto>().Result;
            ViewModel.SelectedStudent = SelectedStudent;
            Debug.WriteLine(SelectedStudent.DateOfBirth);
            return View(ViewModel);
        }

        //POST: Student/Update/5
        [HttpPost]
        public ActionResult Update(int id, Student Student)
        {
            string url = "studentdata/UpdateStudent/" + id;
            string jsonpayload = jss.Serialize(Student);
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

        // GET: Student/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "Studentdata/findStudent/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            StudentDto selectedStudent = response.Content.ReadAsAsync<StudentDto>().Result;
            return View(selectedStudent);
        }

        // POST: Student/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "Studentdata/deleteStudent/" + id;
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