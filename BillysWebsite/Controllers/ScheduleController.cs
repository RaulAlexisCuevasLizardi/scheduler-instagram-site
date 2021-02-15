using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using BillysWebsite.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace BillysWebsite.Controllers
{
    public enum TIME_OF_DAY
    {
        MORNING = 0,
        AFTERNOON = 1
    }
    public class ScheduleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult MakeAppointment(DateTime date)
        {
            ViewData["date"] = date;
            return View();
        }

        [HttpPost]
        public IActionResult MakeAppointment(string name, string description, TIME_OF_DAY timeOfDay, DateTime startDate)
        {
            if(timeOfDay == TIME_OF_DAY.MORNING)
            {
                TimeSpan time = new TimeSpan(10, 0, 0);
                startDate = startDate.Date + time;
            }
            else if(timeOfDay == TIME_OF_DAY.AFTERNOON)
            {
                TimeSpan time = new TimeSpan(15, 0, 0);
                startDate = startDate.Date + time;
            }
            DateTime endDate = startDate.AddHours(4);
            if(Functions.AddAppointent(name, description, startDate, endDate) == 1)
            {
                //return success
            }
            else
            {
                //return failure
            }
            return RedirectToAction("Index", "Home");
        }
    }
}