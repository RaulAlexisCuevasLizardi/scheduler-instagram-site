using System;

namespace BillysWebsite.Models
{
    public class AppointmentType
    {

        public enum DURATION_TYPE
        {
            HOURS = 0,
            MINUTES = 1
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeSpan StartTime { get; set; }
        public byte DaysOfWeek { get; set; }
        public int Duration { get; set; }
        public char[] Color { get; set; }
        public Decimal Price { get; set; }
        public DURATION_TYPE DurationType { get; set; }
        public AppointmentType()
        {
            Color = new char[6];
        }
    }
}