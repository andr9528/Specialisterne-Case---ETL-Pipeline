using Weather.Abstraction.Enum;
using Weather.Abstraction.Interfaces.Persistence;

namespace Weather.Abstraction.Interfaces.Model.Searchable
{
    public interface ISearchableDmi : ISearchable
    {
        Guid DmiId { get; set; }
        DmiParameter ParameterId { get; set; }
        int StationId { get; set; }
    }
}