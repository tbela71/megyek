namespace Megyek.Models
{
    public partial class Membership
    {
        public int PersonId { get; set; }
        public int TeamId { get; set; }
        public string DisplayName { get; set; }
        public bool Manager { get; set; }
        public bool Mail { get; set; }

        public virtual Person Person { get; set; }
        public virtual Team Team { get; set; }

        public string DisplayNameX
        {
            get
            {
                return DisplayName.Replace(" ", "");
            }
        }
    }
}
