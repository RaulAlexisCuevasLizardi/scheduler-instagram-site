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
                    //tempEvent.display = "background";
                    tempEvent.start = appointments[i].StartDate;
                    tempEvent.end = appointments[i].EndDate;
                    tempEvent.id = appointments[i].AppointmentPK;
                    tempEvent.title = appointments[i].Title;
                    tempEvent.url = Url.Action("ViewAppointment", "Schedule", new { id = appointments[i].AppointmentPK});
                    if(i > 0)
                    {
                        if (appointments[i-1].StartDate.Month == appointments[i].StartDate.Month ||
                               appointments[i-1].StartDate.Day == appointments[i].StartDate.Day ||
                               appointments[i-1].StartDate.Year == appointments[i].StartDate.Year)
                        {
                            if (fullScheduleDays == null)
                                fullScheduleDays = new List<DateTime>();
                            fullScheduleDays.Add(new DateTime(appointments[i].StartDate.Year, appointments[i].StartDate.Month, appointments[i].StartDate.Day));

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
            //Get appointments and check if appointment already exists
            List<Appointment> appointments = Functions.GetAppointments(0, startDate, endDate);
            if(appointments != null && appointments.Count > 0)
            {
                //We already have an appointment at that time 
                //Return a failure message
                ViewData["message"] = "An appointment at that time already exists.";
            }
            if(Functions.AddAppointent(name, description, startDate, endDate) == 1)
            {
                ViewData["message"] = "Appointment has been successfully created.";
            }
            else
            {
                ViewData["message"] = "Error: Could not create appointment.";
            }
            return RedirectToAction("Index", "ViewAppointment");
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