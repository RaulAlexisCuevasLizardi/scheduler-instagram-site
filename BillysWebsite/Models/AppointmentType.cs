using System;

namespace BillysWebsite.Models
{
    public class AppointmentType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeSpan StartTime { get; set; }
        public byte DaysOfWeek { get; set; }
        public Decimal Duration { get; set; }
    }
}