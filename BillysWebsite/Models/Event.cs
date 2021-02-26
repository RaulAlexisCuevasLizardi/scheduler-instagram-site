using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillysWebsite.Models
{
    public class Event
    {
        public int id { get; set; }
        public bool allDay { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string startStr { get; set; }
        public string endStr { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public bool editable { get; set; }
        public string[] classNames{ get; set; }
        public bool startEditable { get; set; }
        public bool durationEditable { get; set; }
        public bool resourceEditable { get; set; }
        public string backgroundColor { get; set; }
        public string display { get; set; }
    }
}
