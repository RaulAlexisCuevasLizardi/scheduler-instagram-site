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
                    tempEvent.id = appointments[i].Id;
                    //title should be the category type
                    //we dont want to display the name to everyone 
                    //that visits the web page
                    tempEvent.title = appointments[i].FirstName;
                    tempEvent.url = Url.Action("ViewAppointment", "Schedule", new { id = appointments[i].Id});
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
            List<AppointmentType> appointmentTypes = Functions.GetAppointmentTypes();
            ViewData["appointmentTypes"] = appointmentTypes;
            ViewData["date"] = date;
            return View();
        }

        [HttpPost]
        public IActionResult MakeAppointment(IFormCollection collection)
        {
            TIME_OF_DAY timeOfDay;
            string firstName = collection["firstName"];
            string lastName = collection["lastName"];
            string dateOfBirthString = collection["dateOfBirth"];
            string phoneNumber = collection["phoneNumber"];
            string email = collection["email"];
            string description = collection["description"];
            string licenseNumber = collection["licenseNumber"];
            string timeOfDayString = collection["timeOfDay"];
            string startDateString = collection["startDate"];
            string appointmentTypeString = collection["AppointmentType"];
            IFormFile file = collection.Files.GetFile("imageReferenceUpload");
            if (!DateTime.TryParse(dateOfBirthString, out DateTime dateOfBirth))
            {
                //return error message
            }
            if (!int.TryParse(timeOfDayString, out int timeOfDayInt))
            {
                //return error message
            }
            timeOfDay = (TIME_OF_DAY)timeOfDayInt;
            if(!DateTime.TryParse(startDateString, out DateTime startDate))
            {
                //return error message
            }
            //getting filename extension
            string fileExtension = "";
            if(file.ContentType.Split('/').Length > 1)
            {
                fileExtension = file.ContentType.Split('/')[1];
            }
            //Saving image file to Uploads folder
            string fileName = Guid.NewGuid().ToString() + "." + fileExtension;
            string fileDescription = file.FileName;
            using (var fileStream = new FileStream(Path.Combine(_hostingEnvironment.WebRootPath + "/Uploads", fileName), FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            if (!int.TryParse(appointmentTypeString, out int appointmentTypeId))
            {
                //return error message
            }

            AppointmentType appointmentType = null;
            {
                List<AppointmentType> appointmentTypes = Functions.GetAppointmentTypes(appointmentTypeId);
                if (appointmentTypes != null && appointmentTypes.Count > 0)
                    appointmentType = appointmentTypes[0];
            }

            startDate += appointmentType.StartTime;
            DateTime endDate;
            if (appointmentType.DurationType == AppointmentType.DURATION_TYPE.HOURS)
            {
                endDate = startDate.AddHours(appointmentType.Duration);
            }
            else
            {
                endDate = startDate.AddMinutes(appointmentType.Duration);
            }

            //if (timeOfDay == TIME_OF_DAY.MORNING)
            //{
            //    TimeSpan time = new TimeSpan(10, 0, 0);
            //    startDate = startDate.Date + time;
            //}
            //else if (timeOfDay == TIME_OF_DAY.AFTERNOON)
            //{
            //    TimeSpan time = new TimeSpan(15, 0, 0);
            //    startDate = startDate.Date + time;
            //}
            //DateTime tempendDate = startDate.AddHours(4);
            //Get appointments and check if appointment already exists
            List<Appointment> appointments = Functions.GetAppointments(0, startDate, endDate);
            if (appointments != null && appointments.Count > 0)
            {
                //We already have an appointment at that time 
                //Return a failure message
                ViewData["message"] = "An appointment at that time already exists.";
            }
            if (Functions.AddAppointent(description, startDate, endDate, firstName, lastName,
                                        dateOfBirth, phoneNumber, email, fileName, fileDescription,
                                        appointmentType.Id) == 1)
            {
                ViewData["message"] = "Appointment has been successfully created.";
            }
            else
            {
                ViewData["message"] = "Error: Could not create appointment.";
            }
            return RedirectToAction("Index", "ViewAppointment");
        }
    }
}