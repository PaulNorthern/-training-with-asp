using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcCore.Models;
using MvcCore.Util;

namespace MvcCore.Controllers
{
    public class HomeController : Controller
    {
        MobileContext db;

        // блок по загрузки файлов 
        private readonly IWebHostEnvironment _appEnvironment;
        //private readonly IDrink _drink;
        // инхекция через ктор
        public HomeController(MobileContext context, IWebHostEnvironment appEnvironment/*, IDrink drink*/)
        {
            db = context;
            _appEnvironment = appEnvironment;
            //_drink = drink;
        }
        public IActionResult GetFile()
        {
            //_drink.Drink();
            // Путь к файлу
            string file_path = Path.Combine(_appEnvironment.ContentRootPath, "Files/sample.pdf");
            // Тип файла - content-type
            string file_type = "application/pdf";
            // Имя файла - необязательно
            string file_name = "sample.pdf";


            //IDrink f = new Party();
            //var gay = new Vanya(new Party());

            //gay.bobo = new Party();
            //gay.bobo.Drink();
            
            return PhysicalFile(file_path, file_type, file_name);
        }
        //class Vanya
        //{
        //    public IDrink bobo { get; set; }
        //    public IDrink dodo { get; set; }
        //    public Vanya(IDrink bobo)
        //    {
        //        this.bobo = bobo;
        //    }

        //}

        //public interface IDrink
        //{
        //    void Drink();
        //}

        //class Party : IDrink
        //{
        //    public void Drink()
        //    {
        //        throw new NotImplementedException();
        //    }
        //}


        public IActionResult Index()
        {
            return View(db.Phones.ToList());
        }
        public HtmlResult GetHtml()
        {
            return new HtmlResult("<h2>Привет ASP.NET 5</h2>");
        }

        [ActionName("Welcome")]
        public string Hello()
        {
            return "Hello ASP.NET";
        }

        public string Sum(Geometry[] geoms)
        {
            return $"Сумма площадей равна {geoms.Sum(g => g.GetArea())}";
        }

        
        public IActionResult Area(int altitude, int height)
        {
            double area = altitude * height / 2;
            return Content($"Площадь треугольника с основанием {altitude} и высотой {height} равна {area}");
        }

        public class Geometry
        {
            public int Altitude { get; set; } // основание
            public int Height { get; set; } // высота

            public double GetArea() // вычисление площади треугольника
            {
                return Altitude * Height / 2;
            }
        }


        // JSON-part
        public class User
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }
        public JsonResult GetUser()
        {
            User user = new User { Name = "Tom", Age = 28 };
            return Json(user);
        }


        [HttpGet]
        public IActionResult Buy(int? id)
        {
            if (id == null) return RedirectToAction("Index");
            // Чтобы передать id смартфона в представление применяется объект ViewBag.
            ViewBag.PhoneId = id;
            return View();
        }

        [HttpPost]
        public string Buy(Order order)
        {
            db.Orders.Add(order);
            // сохраняем в бд все изменения
            db.SaveChanges();
            return "Спасибо, " + order.User + ", за покупку!";
        }
    }
}
