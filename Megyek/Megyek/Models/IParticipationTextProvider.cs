namespace Megyek.Models
{
    public interface IParticipationTextProvider
    {
        string GetPersonDisplayName(Participation participation);
        string GetPersonDisplayNameX(Participation participation);
        string GetByPersonDisplayName(Participation participation);
        string GetByPersonDisplayNameX(Participation participation);
        string GetDisplayText(Participation participation);
    }
}
