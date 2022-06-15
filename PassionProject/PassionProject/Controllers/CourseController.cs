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
    public class CourseController : Controller
    {
        // GET: Course
        public ActionResult Index()
        {
            return View();
        }

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static CourseController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44379/api/");
        }

        //GET: Course/List
        public ActionResult List()
        {
            //objective: communicate with our Course data api to retrieve a list of Course
            //curl https://localhost:44379/api/Coursedata/listCourses

            string url = "Coursedata/listCourses";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<CourseDto> Courses = response.Content.ReadAsAsync<IEnumerable<CourseDto>>().Result;
            return View(Courses);
        }

        public ActionResult Error()
        {
            return View();
        }

        //GET: Student/Detail/2
        public ActionResult Detail(int id)
        {
            DetailCourse ViewModel = new DetailCourse();

            //objective: communicate with our student data api to retrieve a list of students
            //curl https://localhost:44379/api/Coursedata/FindCourse/{id}

            string url = "Coursedata/FindCourse/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            CourseDto SelectedCourse = response.Content.ReadAsAsync<CourseDto>().Result;

            ViewModel.SelectedCourse = SelectedCourse;
            
            url = "studentdata/listCourseTaken/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<StudentDto> StudentTaken = response.Content.ReadAsAsync<IEnumerable<StudentDto>>().Result;
            ViewModel.StudentTaken = StudentTaken;


            url = "teacherdata/listCourseTaught/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<TeacherDto> TeacherTaught = response.Content.ReadAsAsync<IEnumerable<TeacherDto>>().Result;
            ViewModel.TeacherTaught = TeacherTaught;

            return View(ViewModel);
        }

        //GET: Course/New
        public ActionResult New()
        {
            string url = "Coursedata/listCourses";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<CourseDto> Courses = response.Content.ReadAsAsync<IEnumerable<CourseDto>>().Result;
            return View(Courses);
        }


        //POST: Course/Create
        [HttpPost]
        public ActionResult Create(Course Course)
        {
            Debug.WriteLine("the json payload is :");
            //curl -H "Content-Type:application/json" -d @Course.json  https://localhost:44379/api/Coursedata/addCourse 
            string url = "Coursedata/addCourse";

            string jsonpayload = jss.Serialize(Course);
            Debug.WriteLine(jsonpayload);

            Debug.WriteLine("Name: " + Course.CourseName);

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

        //GET: Course/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateCourse ViewModel = new UpdateCourse();
            string url = "Coursedata/FindCourse/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            CourseDto SelectedCourse = response.Content.ReadAsAsync<CourseDto>().Result;
            ViewModel.SelectedCourse = SelectedCourse;
            return View(ViewModel);
        }

        //POST: Course/Update/5
        [HttpPost]
        public ActionResult Update(int id, Course Course)
        {
            string url = "Coursedata/updateCourse/" + id;
            string jsonpayload = jss.Serialize(Course);
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

        // GET: Course/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "Coursedata/findCourse/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CourseDto selectedCourse = response.Content.ReadAsAsync<CourseDto>().Result;
            return View(selectedCourse);
        }

        // POST: Course/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "Coursedata/deleteCourse/" + id;
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