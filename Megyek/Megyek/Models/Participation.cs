using System;
using System.ComponentModel.DataAnnotations;

namespace Megyek.Models
{
    public partial class Participation
    {
        public int PersonId { get; set; }
        public int EventId { get; set; }
        public bool? Participate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy.MM.dd H:mm:ss}")]
        public DateTime Date { get; set; }
        public int ByPersonId { get; set; }

        public virtual Event Event { get; set; }
        public virtual Person Person { get; set; }

    }
}
