using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using BillysWebsite.Helpers;
using BillysWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using NUnit.Framework;
using Microsoft.AspNetCore.Http;
using System.Web;
using Contentful.Core.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace BillysWebsite.Controllers
{
    public enum TIME_OF_DAY
    {
        MORNING = 0,
        AFTERNOON = 1
    }
    public class ScheduleController : Controller
    {
        private IWebHostEnvironment _hostingEnvironment;
        public ScheduleController(IWebHostEnvironment environment)
        {
            _hostingEnvironment = environment;
        }
        public IActionResult Index()
        {
            List<DateTime> fullScheduleDays = null;
            List<Appointment> appointments = Functions.GetAppointments(0);
            List<Event> events = null;
            if(appointments != null && appointments.Count > 0)
            {
                for(int i = 0; i < appointments.Count; i++)
                {
                    if (events == null)
                        events = new List<Event>();
                    Event tempEvent = new Event();
                    tempEvent.allDay = false;
                    tempEvent.start = appointments[i].StartDate;
                    tempEvent.end = appointments[i].EndDate;
                    tempEvent.id = appointments[i].AppointmentPK;
                    //title should be the category type
                    //we dont want to display the name to everyone 
                    //that visits the web page
                    tempEvent.title = appointments[i].FirstName;
                    tempEvent.url = Url.Action("ViewAppointment", "Schedule", new { id = appointments[i].AppointmentPK});
                    if(i > 0)
                    {
                        DateTime tomorrowDate = DateTime.Today.AddDays(1);
                        if(appointments[i].StartDate >= tomorrowDate)
                        if (appointments[i-1].StartDate.Month == appointments[i].StartDate.Month &&
                               appointments[i-1].StartDate.Day == appointments[i].StartDate.Day &&
                               appointments[i-1].StartDate.Year == appointments[i].StartDate.Year)
                        {
                            if (fullScheduleDays == null)
                                fullScheduleDays = new List<DateTime>();
                            fullScheduleDays.Add(new DateTime(appointments[i].StartDate.Year,
                                                              appointments[i].StartDate.Month,
                                                              appointments[i].StartDate.Day));
                        }
                    }

                    events.Add(tempEvent);
                }
            }
            string eventsJson = JsonSerializer.Serialize(events, typeof(List<Event>));
            string fullScheduleDaysJson = JsonSerializer.Serialize(fullScheduleDays, typeof(List<DateTime>));
            ViewData["fullScheduleDaysJson"] = fullScheduleDaysJson;
            ViewData["appointments"] = appointments;
            ViewData["eventsJson"] = eventsJson;
            return View();
        }

        [HttpGet]
        public IActionResult MakeAppointment(DateTime date)
        {
            ViewData["date"] = date;
            return View();
        }

        [HttpPost]
        public IActionResult MakeAppointment(IFormCollection collection)
        {
            string firstName = collection["firstName"];
            string lastName = collection["lastName"];
            string dateOfBirth = collection["dateOfBirth"];
            string phoneNumber = collection["phoneNumber"];
            string email = collection["email"];
            string description = collection["description"];
            string licenseNumber = collection["licenseNumber"];
            string imageReferenceUpload = collection["imageReferenceUpload"];
            string timeOfDay = collection["timeOfDay"];
            string startDate = collection["startDate"];
            IFormFile file = collection.Files.GetFile("imageReferenceUpload");
            using (var fileStream = new FileStream(Path.Combine(_hostingEnvironment.WebRootPath + "/Uploads", file.FileName), FileMode.Create))
            {
                file.CopyToAsync(fileStream);
            }

            //if (timeOfDay == TIME_OF_DAY.MORNING)
            //{
            //    TimeSpan time = new TimeSpan(10, 0, 0);
            //    startDate = startDate.Date + time;
            //}
            //else if(timeOfDay == TIME_OF_DAY.AFTERNOON)
            //{
            //    TimeSpan time = new TimeSpan(15, 0, 0);
            //    startDate = startDate.Date + time;
            //}
            //DateTime endDate = startDate.AddHours(4);
            ////Get appointments and check if appointment already exists
            //List<Appointment> appointments = Functions.GetAppointments(0, startDate, endDate);
            //if(appointments != null && appointments.Count > 0)
            //{
            //    //We already have an appointment at that time 
            //    //Return a failure message
            //    ViewData["message"] = "An appointment at that time already exists.";
            //}
            //if(Functions.AddAppointent(firstName, description, startDate, endDate) == 1)
            //{
            //    ViewData["message"] = "Appointment has been successfully created.";
            //}
            //else
            //{
            //    ViewData["message"] = "Error: Could not create appointment.";
            //}
            return RedirectToAction("Index", "ViewAppointment");
        }
    }
}