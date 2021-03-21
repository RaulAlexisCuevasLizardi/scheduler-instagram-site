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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace BillysWebsite.Controllers
{
    public class AdminController : Controller
    {
        private IWebHostEnvironment _hostingEnvironment;
        public AdminController(IWebHostEnvironment environment)
        {
            _hostingEnvironment = environment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Preferences()
        {
            List<AppointmentType> appointmentTypes = null;
            appointmentTypes = Functions.GetAppointmentTypes();
            ViewData["appointmentTypes"] = appointmentTypes;
            return View();
        }

        public IActionResult ViewSchedule()
        {
            List<Appointment> appointments = Functions.GetAppointments(0);
            List<Event> events = null;
            if (appointments != null && appointments.Count > 0)
            {
                for (int i = 0; i < appointments.Count; i++)
                {
                    if (events == null)
                        events = new List<Event>();
                    Event tempEvent = new Event();
                    tempEvent.allDay = false;
                    tempEvent.start = appointments[i].StartDate;
                    tempEvent.end = appointments[i].EndDate;
                    tempEvent.id = appointments[i].Id;
                    tempEvent.title = appointments[i].FirstName + " " + appointments[i].LastName; 
                    tempEvent.url = Url.Action("ViewAppointment", "Admin", new { id = appointments[i].Id });
                    events.Add(tempEvent);
                }
            }
            string eventsJson = JsonSerializer.Serialize(events, typeof(List<Event>));
            ViewData["appointments"] = appointments;
            ViewData["eventsJson"] = eventsJson;
            return View();
        }

        [HttpGet]
        public IActionResult ViewAppointment(int id)
        {
            Appointment appointment = null;
            {
                List<Appointment> appointments = Functions.GetAppointments(id);
                if (appointments != null && appointments.Count > 0)
                    appointment = appointments[0];
            }
            if (appointment == null)
                return View("Error");
            ViewData["imagePath"] = @"\Uploads\" + appointment.FileName;
            ViewData["appointment"] = appointment;
            return View();
        }

        [HttpGet]
        public IActionResult AddAppointmentType()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddAppointmentType(IFormCollection collection)
        {
            string name = collection["name"];
            string startTimeString = collection["StartTime"]; //16:20
            string durationString = collection["Duration"]; //1
            string durationType = collection["DurationType"]; //0
            string eventColor = collection["EventColor"]; //#1b6ec2
            string priceString = collection["Price"]; //1.00
            string daysOfWeek = collection["daysOfWeek"]; //2,4,8,16,32
            string[] startTimeComponents = null;
            if (!string.IsNullOrEmpty(startTimeString))
            {
                startTimeComponents = startTimeString.Split(':');
            }
            else
            {
                //return error
            }
            string startTimeHourString = null;
            string startTimeMinuteString = null;
            if(startTimeComponents.Length > 0)
            {
                startTimeHourString = startTimeComponents[0];
            }
            if (startTimeComponents.Length > 1)
            {
                startTimeMinuteString = startTimeComponents[1];
            }
            if(!int.TryParse(startTimeHourString, out int startTimeHourInt))
            {
                //return error
            }
            if (!int.TryParse(startTimeMinuteString, out int startTimeMinuteInt))
            {
                //return error
            }
            TimeSpan startTime = new TimeSpan(startTimeHourInt, startTimeMinuteInt, 0);
            if (!int.TryParse(durationString, out int durationInt))
            {
                //return error
            }
            if (!int.TryParse(durationType, out int durationTypeInt))
            {
                //return error
            }
            if(!string.IsNullOrEmpty(eventColor) && eventColor[0] == '#')
            {
                eventColor = eventColor.Trim('#');
            }
            else
            {
                //return error
            }
            if (!Decimal.TryParse(priceString, out Decimal priceDecimal))
            {
                //return error
            }
            int daysOfWeekMask = 0;
            if(!string.IsNullOrEmpty(daysOfWeek) && daysOfWeek.Contains(','))
            {
                //split daysOfWeek by its commas and add all of the values
                string[] daysOfWeekComponents = daysOfWeek.Split(',');
                foreach (var dayOfWeek in daysOfWeekComponents)
                {
                    if (!int.TryParse(dayOfWeek, out int dayOfWeekInt))
                    {
                        //return error
                    }
                    daysOfWeekMask += dayOfWeekInt;
                }
            }
            else if (true)
            {
                if (!int.TryParse(daysOfWeek, out int dayOfWeekInt))
                {
                    //return error
                }
                daysOfWeekMask += dayOfWeekInt;
            }
            else
            {
                //return error
            }
            int success = Functions.AddAppointmentType(name, startTime, daysOfWeekMask,
                                         eventColor, priceDecimal, durationTypeInt,
                                         durationInt);
            if(success <= 0)
            {
                //return error
            }
            return RedirectToAction("AddAppointmentType");
        }
    }
}
