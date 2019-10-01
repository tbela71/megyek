using System.Collections.Generic;
using System.Threading.Tasks;

namespace Megyek.Models
{
    public interface IEventRepo
    {
        IList<Event> GetEventList(int? teamId);
        Event GetManagedEvent(int? eventId);

        Event GetEvent(int? eventId);
        Task<Event> GetEventAsync(int? eventId);
        Task DeleteEventAsync(int? id);
        Task SaveEventAsync(Event evnt);
        Task CreateEventAsync(Event evnt);
        Task SetLastAlertSent(Event evnt);
    }
}
