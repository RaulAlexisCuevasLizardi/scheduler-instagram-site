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
                    tempEvent.id = appointments[i].AppointmentPK;
                    tempEvent.title = appointments[i].FirstName + " " + appointments[i].LastName; 
                    tempEvent.url = Url.Action("ViewAppointment", "Admin", new { id = appointments[i].AppointmentPK });
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
    }
}
