using Weather.Abstraction.Enum;

namespace Weather.Abstraction.Interfaces.Persistence
{
    // Todo: Consider at better name...

    /// <summary>
    /// Add Implementation for things that need a Complex search.
    /// </summary>
    /// <typeparam name="TSearchable"></typeparam>
    public interface IComplexSearchable<TSearchable> where TSearchable : class, ISearchable, new()
    {
        TSearchable Searchable { get; set; }

        OrderDirection? OrderByObservedAt { get; set; }
        OrderDirection? OrderByPulledAt { get; set; }
        int? LastXDaysObservedAt { get; set; }
        int? LastXDaysPulledAt { get; set; }
        DateTime? ObservedAtAfterThisDateTime { get; set; }
        DateTime? ObservedAtBeforeThisDateTime { get; set; }
        DateTime? PulledAtAfterThisDateTime { get; set; }
        DateTime? PulledAtBeforeThisDateTime { get; set; }
    }
}