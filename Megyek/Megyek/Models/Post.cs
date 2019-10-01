using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Megyek.Models
{
    public partial class Post
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public int TeamId { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy.MM.dd H:mm:ss}")]
        public DateTime Date { get; set; }
        public string Text { get; set; }

        public virtual Person Person { get; set; }
        public virtual Team Team { get; set; }

        public string DisplayName
        {
            get
            {
                return Team?.Membership?.FirstOrDefault(x => x.PersonId == PersonId)?.DisplayName ?? Person?.UserName ?? "?";
            }
        }

    }
}
