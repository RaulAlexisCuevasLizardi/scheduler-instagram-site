using System;

namespace BillysWebsite.Models
{
    public class Appointment
    {
        public int AppointmentPK { get; set; }
        public string Title { get; set; }
        public DateTime StartDate {get; set;}
        public DateTime EndDate { get; set; }
    }
}
