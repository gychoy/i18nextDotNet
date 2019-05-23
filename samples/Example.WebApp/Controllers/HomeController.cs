using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Example.WebApp.Models;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using I18Next.Net;

namespace Example.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly II18Next _i18Next;

        public HomeController(IStringLocalizer<HomeController> localizer, II18Next i18Next)
        {
            _localizer = localizer;
            _i18Next = i18Next;       
        }

        public IActionResult About()
        {
            ViewData["Message"] = _localizer["about.description"];

            return View();
        }
        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = _localizer["about.renderedOn", new { date = DateTime.Now }];

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SelectLanguage(IFormCollection collection)
        {
            string language = collection["Language"];

            if (language == "German")
            {
                _i18Next.Language = "de";
            }
            else if (language == "cimode")
            {
                _i18Next.Language = "cimode";
            }
            else
            {
                _i18Next.Language = "en";
            }



            return View("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
