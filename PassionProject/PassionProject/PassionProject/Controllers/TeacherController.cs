using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using PassionProject.Models;
//using PassionProject.Models.ViewModels;
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

        //POST: Student/Create
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

    }
}