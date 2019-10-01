using System;
using System.Collections.Generic;

namespace Megyek.Models
{
    public partial class Person
    {
        public Person()
        {
            Membership = new HashSet<Membership>();
            Participation = new HashSet<Participation>();
            Post = new HashSet<Post>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }

        public virtual ICollection<Membership> Membership { get; set; }
        public virtual ICollection<Participation> Participation { get; set; }
        public virtual ICollection<Post> Post { get; set; }
    }
}
