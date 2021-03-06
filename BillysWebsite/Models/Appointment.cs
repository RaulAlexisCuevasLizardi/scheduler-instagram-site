using System;

namespace BillysWebsite.Models
{
    public class Appointment
    {
        public int AppointmentPK { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string FileName { get; set; }
        public string FileDescription { get; set; }
        public string Description { get; set; }
        public DateTime StartDate {get; set;}
        public DateTime EndDate { get; set; }
    }
}
