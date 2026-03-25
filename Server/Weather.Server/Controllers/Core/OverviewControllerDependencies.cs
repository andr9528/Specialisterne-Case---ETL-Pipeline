using Weather.Abstraction.Interfaces.Persistence;
using Weather.Abstraction.Interfaces.Startup;
using Weather.Model.Dto.Read;
using Weather.Model.Entity;
using Weather.Model.Searchable;

namespace Weather.Server.Controllers.Core
{
    public sealed record OverviewControllerDependencies(
        // Query services
        IEntityQueryService<Bme, SearchableBme> BmeQueryService,
        IEntityQueryService<Dmi, SearchableDmi> DmiQueryService,
        IEntityQueryService<Ds, SearchableDs> DsQueryService,
        IEntityQueryService<Scd, SearchableScd> ScdQueryService,

        // DTO factories
        IReadDtoFactory<Bme, ReadDtoBme> BmeReadDtoFactory,
        IReadDtoFactory<Dmi, ReadDtoDmi> DmiReadDtoFactory,
        IReadDtoFactory<Ds, ReadDtoDs> DsReadDtoFactory,
        IReadDtoFactory<Scd, ReadDtoScd> ScdReadDtoFactory);
}