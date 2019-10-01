using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Megyek.Models
{
    public partial class Event
    {
        public Event()
        {
            Participation = new HashSet<Participation>();
        }

        public int Id { get; set; }
        public int TeamId { get; set; }
        public DateTime Date { get; set; }
        public DateTime? EndDate { get; set; }
        public string Description { get; set; }
        public string Place { get; set; }
        public bool Special { get; set; }
        public DateTime? LastAlertSent { get; set; }

        public virtual Team Team { get; set; }
        public virtual ICollection<Participation> Participation { get; set; }

        public string DisplayDate
        {
            get
            {
                DateTime endDate = EndDate ?? Date;
                if (Date == endDate)
                {
                    return Date.ToString("yyyy.MM.dd H:mm:ss");
                }
                if (Date.Year == endDate.Year && Date.Month == endDate.Month && Date.Day == endDate.Day)
                {
                    return Date.ToString("yyyy.MM.dd H:mm:ss") + " - " + endDate.ToString("H:mm:ss");
                }
                return Date.ToString("yyyy.MM.dd H:mm:ss") + " - " + endDate.ToString("yyyy.MM.dd H:mm:ss");
            }
        }

        public bool IsAlertAllowed
        {
            get
            {
                return (LastAlertSent ?? new DateTime(1900, 1, 1)) < DateTime.Now.AddHours(-24);
            }
        }

        public string LastAlertSentAsText
        {
            get
            {
                return "Last alert: "  + LastAlertSent?.ToString("H:mm:ss") ?? "";
            }
        }

    }
}
