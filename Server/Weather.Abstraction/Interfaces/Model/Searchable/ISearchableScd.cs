using Weather.Abstraction.Interfaces.Persistence;

namespace Weather.Abstraction.Interfaces.Model.Searchable
{
    public interface ISearchableScd : ISearchable
    {
        Guid ReaderId { get; set; }
        int CarbonDioxide { get; set; }
    }
}