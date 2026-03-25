namespace Weather.Abstraction.Interfaces.Dto
{
    public interface IReadDto
    {
        string LocalObservedAtHumanReadable { get; set; }
        string LocalPulledAtHumanReadable { get; set; }
    }
}