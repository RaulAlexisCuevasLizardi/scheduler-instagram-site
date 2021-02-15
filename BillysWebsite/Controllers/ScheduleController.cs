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
            List<Appointment> appointments = Functions.GetAppointments();
            List<Event> events = null;
            if(appointments != null && appointments.Count > 0)
            {
                foreach (Appointment appointment in appointments)
                {
                    if (events == null)
                        events = new List<Event>();
                    Event tempEvent = new Event();
                    tempEvent.allDay = false;
                    tempEvent.start = appointment.StartDate;
                    tempEvent.end = appointment.EndDate;
                    tempEvent.id = appointment.AppointmentPK;
                    tempEvent.title = appointment.Title;
                    tempEvent.url = Url.Action("ViewAppointment", "Schedule", new { id = appointment.AppointmentPK});
                    events.Add(tempEvent);
                }
            }
            string eventsJson = JsonSerializer.Serialize(events, typeof(List<Event>));
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
            ViewData["appointment"] = appointment;
            return View();
        }
    }
}