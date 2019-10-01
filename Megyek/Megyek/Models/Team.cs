using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Megyek.Models
{
    public partial class Team
    {
        public Team()
        {
            Event = new HashSet<Event>();
            Membership = new HashSet<Membership>();
            Post = new HashSet<Post>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string DefaultPlace { get; set; }

        public virtual ICollection<Event> Event { get; set; }
        public virtual ICollection<Membership> Membership { get; set; }
        public virtual ICollection<Post> Post { get; set; }

        public List<string> GetEmailsButPersons(List<int> personIdList)
        {
            return Membership.Where(x => x.Mail && !personIdList.Contains(x.Person.Id)).Select(x => x.Person.UserName).ToList();
        }
    }
}
