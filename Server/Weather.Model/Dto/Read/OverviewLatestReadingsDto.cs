namespace Weather.Model.Dto.Read
{
    public class OverviewLatestReadingsDto
    {
        public required ReadDtoBme Bme { get; init; }
        public required ReadDtoDmi Dmi { get; init; }
        public required ReadDtoDs Ds { get; init; }
        public required ReadDtoScd Scd { get; init; }
    }
}