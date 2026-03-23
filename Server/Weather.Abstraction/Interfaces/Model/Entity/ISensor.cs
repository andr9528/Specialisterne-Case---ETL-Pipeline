namespace Weather.Abstraction.Interfaces.Model.Entity
{
    public interface ISensor
    {
        DateTime ObservedAt { get; set; }
        DateTime PulledAt { get; set; }
    }
}