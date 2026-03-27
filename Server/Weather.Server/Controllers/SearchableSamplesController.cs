using Microsoft.AspNetCore.Mvc;
using Weather.Abstraction.Enum;
using Weather.Abstraction.Interfaces.Persistence;
using Weather.Model.ComplexSearchable;
using Weather.Model.Dto.Read;
using Weather.Model.Searchable;

namespace Weather.Server.Controllers
{
    [ApiController]
    [Route(Constants.ROUTE_TEMPLATE)]
    public class SearchableSamplesController : ControllerBase
    {
        /// <summary>
        /// Provides sample searchables for BME280 queries.
        /// These samples can be used with the GetByQuery and GetAllByQuery endpoints on the Bme controller.
        /// </summary>
        [HttpGet]
        public virtual ActionResult<IEnumerable<SearchableSampleDto>> GetSearchableBmeSamples()
        {
            return Ok(BuildSearchableBmeSamples());
        }

        /// <summary>
        /// Provides sample complex searchables for BME280 queries.
        /// These samples can be used with the GetByComplexQuery and GetAllByComplexQuery endpoints on the Bme controller.
        /// </summary>
        [HttpGet]
        public virtual ActionResult<IEnumerable<SearchableSampleDto>> GetComplexSearchableBmeSamples()
        {
            return Ok(BuildComplexSearchableBmeSamples());
        }

        /// <summary>
        /// Provides sample searchables for DMI queries.
        /// These samples can be used with the GetByQuery and GetAllByQuery endpoints on the Dmi controller.
        /// </summary>
        [HttpGet]
        public virtual ActionResult<IEnumerable<SearchableSampleDto>> GetSearchableDmiSamples()
        {
            return Ok(BuildSearchableDmiSamples());
        }

        /// <summary>
        /// Provides sample complex searchables for DMI queries.
        /// These samples can be used with the GetByComplexQuery and GetAllByComplexQuery endpoints on the Dmi controller.
        /// </summary>
        [HttpGet]
        public virtual ActionResult<IEnumerable<SearchableSampleDto>> GetComplexSearchableDmiSamples()
        {
            return Ok(BuildComplexSearchableDmiSamples());
        }

        /// <summary>
        /// Provides sample searchables for DS18B20 queries.
        /// These samples can be used with the GetByQuery and GetAllByQuery endpoints on the Ds controller.
        /// </summary>
        [HttpGet]
        public virtual ActionResult<IEnumerable<SearchableSampleDto>> GetSearchableDsSamples()
        {
            return Ok(BuildSearchableDsSamples());
        }

        /// <summary>
        /// Provides sample complex searchables for DS18B20 queries.
        /// These samples can be used with the GetByComplexQuery and GetAllByComplexQuery endpoints on the Ds controller.
        /// </summary>
        [HttpGet]
        public virtual ActionResult<IEnumerable<SearchableSampleDto>> GetComplexSearchableDsSamples()
        {
            return Ok(BuildComplexSearchableDsSamples());
        }

        /// <summary>
        /// Provides sample searchables for SCD41 queries.
        /// These samples can be used with the GetByQuery and GetAllByQuery endpoints on the Scd controller.
        /// </summary>
        [HttpGet]
        public virtual ActionResult<IEnumerable<SearchableSampleDto>> GetSearchableScdSamples()
        {
            return Ok(BuildSearchableScdSamples());
        }

        /// <summary>
        /// Provides sample complex searchables for SCD41 queries.
        /// These samples can be used with the GetByComplexQuery and GetAllByComplexQuery endpoints on the Scd controller.
        /// </summary>
        [HttpGet]
        public virtual ActionResult<IEnumerable<SearchableSampleDto>> GetComplexSearchableScdSamples()
        {
            return Ok(BuildComplexSearchableScdSamples());
        }

        private List<SearchableSampleDto> BuildSearchableBmeSamples()
        {
            return
            [
                CreateSample("Returns BME280 entries with Id equal to 1.", new SearchableBme
                {
                    Id = 1,
                }),

                CreateSample("Returns BME280 entries from the OUTSIDE location.", new SearchableBme
                {
                    Location = Location.OUTSIDE,
                }),

                CreateSample("Returns BME280 entries produced by a specific reader.", new SearchableBme
                {
                    ReaderId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                }),

                CreateSample("Returns BME280 entries matching BOTH Id = 1 AND Location = OUTSIDE.", new SearchableBme
                {
                    Id = 1,
                    Location = Location.OUTSIDE,
                }),

                CreateSample("Returns BME280 entries matching Location = INSIDE AND a specific ReaderId.",
                    new SearchableBme
                    {
                        Location = Location.INSIDE,
                        ReaderId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    }),

                CreateSample("Returns BME280 entries matching ALL properties (Id, Location, and ReaderId).",
                    new SearchableBme
                    {
                        Id = 5,
                        Location = Location.OUTSIDE,
                        ReaderId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    }),
            ];
        }

        private List<SearchableSampleDto> BuildComplexSearchableBmeSamples()
        {
            return
            [
                CreateSample(
                    "Returns BME280 entries ordered by ObservedAt ascending. When retrieving a single entity, this can be used to get the first registered matching the query arguments.",
                    complexSearchableBme: new ComplexSearchableBme
                    {
                        OrderByObservedAt = OrderDirection.ASCENDING,
                    }),

                CreateSample(
                    "Returns BME280 entries ordered by PulledAt descending. When retrieving a single entity, this can be used to get the last registered matching the query arguments.",
                    complexSearchableBme: new ComplexSearchableBme
                    {
                        OrderByPulledAt = OrderDirection.DESCENDING,
                    }),

                CreateSample("Returns BME280 entries observed within the last 7 days.",
                    complexSearchableBme: new ComplexSearchableBme
                    {
                        LastXDaysObservedAt = 7,
                    }),

                CreateSample("Returns BME280 entries pulled within the last 3 days.",
                    complexSearchableBme: new ComplexSearchableBme
                    {
                        LastXDaysPulledAt = 3,
                    }),

                CreateSample("Returns BME280 entries observed after 1 March 2026 at 00:00 UTC.",
                    complexSearchableBme: new ComplexSearchableBme
                    {
                        ObservedAtAfterThisDateTime = new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                    }),

                CreateSample("Returns BME280 entries observed before 15 March 2026 at 12:00 UTC.",
                    complexSearchableBme: new ComplexSearchableBme
                    {
                        ObservedAtBeforeThisDateTime = new DateTime(2026, 3, 15, 12, 0, 0, DateTimeKind.Utc),
                    }),

                CreateSample("Returns BME280 entries pulled after 10 March 2026 at 08:00 UTC.",
                    complexSearchableBme: new ComplexSearchableBme
                    {
                        PulledAtAfterThisDateTime = new DateTime(2026, 3, 10, 8, 0, 0, DateTimeKind.Utc),
                    }),

                CreateSample("Returns BME280 entries pulled before 20 March 2026 at 18:00 UTC.",
                    complexSearchableBme: new ComplexSearchableBme
                    {
                        PulledAtBeforeThisDateTime = new DateTime(2026, 3, 20, 18, 0, 0, DateTimeKind.Utc),
                    }),

                CreateSample("Returns BME280 entries from the OUTSIDE location observed within the last 7 days.",
                    complexSearchableBme: new ComplexSearchableBme
                    {
                        Searchable = new SearchableBme
                        {
                            Location = Location.OUTSIDE,
                        },
                        LastXDaysObservedAt = 7,
                    }),

                CreateSample(
                    "Returns BME280 entries from the INSIDE location ordered by ObservedAt descending. When retrieving a single entity, this can be used to get the last registered matching the query arguments.",
                    complexSearchableBme: new ComplexSearchableBme
                    {
                        Searchable = new SearchableBme
                        {
                            Location = Location.INSIDE,
                        },
                        OrderByObservedAt = OrderDirection.DESCENDING,
                    }),

                CreateSample("Returns BME280 entries for a specific reader observed after 1 March 2026 at 00:00 UTC.",
                    complexSearchableBme: new ComplexSearchableBme
                    {
                        Searchable = new SearchableBme
                        {
                            ReaderId = Guid.Parse("aaaaaaaa-1111-2222-3333-bbbbbbbbbbbb"),
                        },
                        ObservedAtAfterThisDateTime = new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                    }),

                CreateSample(
                    "Returns BME280 entries matching BOTH Id = 5 and Location = OUTSIDE, ordered by PulledAt descending. When retrieving a single entity, this can be used to get the last registered matching the query arguments.",
                    complexSearchableBme: new ComplexSearchableBme
                    {
                        Searchable = new SearchableBme
                        {
                            Id = 5,
                            Location = Location.OUTSIDE,
                        },
                        OrderByPulledAt = OrderDirection.DESCENDING,
                    }),

                CreateSample(
                    "Returns BME280 entries from the OUTSIDE location observed between 1 March 2026 and 20 March 2026 UTC.",
                    complexSearchableBme: new ComplexSearchableBme
                    {
                        Searchable = new SearchableBme
                        {
                            Location = Location.OUTSIDE,
                        },
                        ObservedAtAfterThisDateTime = new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                        ObservedAtBeforeThisDateTime = new DateTime(2026, 3, 20, 0, 0, 0, DateTimeKind.Utc),
                    }),

                CreateSample(
                    "Returns BME280 entries for a specific reader from the INSIDE location, pulled within the last 2 days and ordered by PulledAt ascending. When retrieving a single entity, this can be used to get the first registered matching the query arguments.",
                    complexSearchableBme: new ComplexSearchableBme
                    {
                        Searchable = new SearchableBme
                        {
                            ReaderId = Guid.Parse("cccccccc-4444-5555-6666-dddddddddddd"),
                            Location = Location.INSIDE,
                        },
                        LastXDaysPulledAt = 2,
                        OrderByPulledAt = OrderDirection.ASCENDING,
                    }),
                CreateSample("Returns BME280 entries with Temperature above 20°C.",
                    complexSearchableBme: new ComplexSearchableBme
                    {
                        AboveTemperature = 20.0f,
                    }),

                CreateSample("Returns BME280 entries with Humidity below 50%.",
                    complexSearchableBme: new ComplexSearchableBme
                    {
                        BelowHumidity = 50.0f,
                    }),

                CreateSample("Returns BME280 entries with Pressure between 1000 and 1020 hPa.",
                    complexSearchableBme: new ComplexSearchableBme
                    {
                        AbovePressure = 1000.0f,
                        BelowPressure = 1020.0f,
                    }),

                CreateSample("Returns BME280 entries from the OUTSIDE location with Temperature above 15°C.",
                    complexSearchableBme: new ComplexSearchableBme
                    {
                        Searchable = new SearchableBme
                        {
                            Location = Location.OUTSIDE,
                        },
                        AboveTemperature = 15.0f,
                    }),

                CreateSample("Returns BME280 entries from a specific reader with Temperature between 18°C and 25°C.",
                    complexSearchableBme: new ComplexSearchableBme
                    {
                        Searchable = new SearchableBme
                        {
                            ReaderId = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"),
                        },
                        AboveTemperature = 18.0f,
                        BelowTemperature = 25.0f,
                    }),

                CreateSample(
                    "Returns BME280 entries with Humidity between 30% and 60%, ordered by ObservedAt descending.",
                    complexSearchableBme: new ComplexSearchableBme
                    {
                        AboveHumidity = 30.0f,
                        BelowHumidity = 60.0f,
                        OrderByObservedAt = OrderDirection.DESCENDING,
                    }),

                CreateSample(
                    "Returns BME280 entries from the INSIDE location with Temperature between 20°C and 24°C observed within the last 3 days.",
                    complexSearchableBme: new ComplexSearchableBme
                    {
                        Searchable = new SearchableBme
                        {
                            Location = Location.INSIDE,
                        },
                        AboveTemperature = 20.0f,
                        BelowTemperature = 24.0f,
                        LastXDaysObservedAt = 3,
                    }),
            ];
        }

        private List<SearchableSampleDto> BuildSearchableDmiSamples()
        {
            return
            [
                CreateSample("Returns DMI entries with Id equal to 1.", searchableDmi: new SearchableDmi
                {
                    Id = 1,
                }),

                CreateSample("Returns DMI entries with a specific DmiId.", searchableDmi: new SearchableDmi
                {
                    DmiId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                }),

                CreateSample("Returns DMI entries for a specific parameter (e.g., TEMP_DRY).",
                    searchableDmi: new SearchableDmi
                    {
                        ParameterId = DmiParameter.TEMP_DRY,
                    }),

                CreateSample("Returns DMI entries from a specific station.", searchableDmi: new SearchableDmi
                {
                    StationId = 12345,
                }),

                CreateSample("Returns DMI entries matching BOTH StationId = 12345 AND ParameterId = TEMP_DRY.",
                    searchableDmi: new SearchableDmi
                    {
                        StationId = 12345,
                        ParameterId = DmiParameter.TEMP_DRY,
                    }),

                CreateSample("Returns DMI entries matching DmiId AND StationId.", searchableDmi: new SearchableDmi
                {
                    DmiId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    StationId = 67890,
                }),

                CreateSample("Returns DMI entries matching ParameterId AND DmiId.", searchableDmi: new SearchableDmi
                {
                    ParameterId = DmiParameter.HUMIDITY,
                    DmiId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                }),

                CreateSample("Returns DMI entries matching ALL properties (Id, DmiId, ParameterId, and StationId).",
                    searchableDmi: new SearchableDmi
                    {
                        Id = 10,
                        DmiId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                        ParameterId = DmiParameter.TEMP_DRY,
                        StationId = 99999,
                    }),
            ];
        }

        private List<SearchableSampleDto> BuildComplexSearchableDmiSamples()
        {
            return
            [
                CreateSample(
                    "Returns DMI entries ordered by ObservedAt ascending. When retrieving a single entity, this can be used to get the first registered matching the query arguments.",
                    complexSearchableDmi: new ComplexSearchableDmi
                    {
                        OrderByObservedAt = OrderDirection.ASCENDING,
                    }),

                CreateSample(
                    "Returns DMI entries ordered by PulledAt descending. When retrieving a single entity, this can be used to get the last registered matching the query arguments.",
                    complexSearchableDmi: new ComplexSearchableDmi
                    {
                        OrderByPulledAt = OrderDirection.DESCENDING,
                    }),

                CreateSample("Returns DMI entries observed within the last 7 days.",
                    complexSearchableDmi: new ComplexSearchableDmi
                    {
                        LastXDaysObservedAt = 7,
                    }),

                CreateSample("Returns DMI entries pulled within the last 3 days.",
                    complexSearchableDmi: new ComplexSearchableDmi
                    {
                        LastXDaysPulledAt = 3,
                    }),

                CreateSample("Returns DMI entries observed after 1 March 2026 at 00:00 UTC.",
                    complexSearchableDmi: new ComplexSearchableDmi
                    {
                        ObservedAtAfterThisDateTime = new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                    }),

                CreateSample("Returns DMI entries observed before 15 March 2026 at 12:00 UTC.",
                    complexSearchableDmi: new ComplexSearchableDmi
                    {
                        ObservedAtBeforeThisDateTime = new DateTime(2026, 3, 15, 12, 0, 0, DateTimeKind.Utc),
                    }),

                CreateSample("Returns DMI entries pulled after 10 March 2026 at 08:00 UTC.",
                    complexSearchableDmi: new ComplexSearchableDmi
                    {
                        PulledAtAfterThisDateTime = new DateTime(2026, 3, 10, 8, 0, 0, DateTimeKind.Utc),
                    }),

                CreateSample("Returns DMI entries pulled before 20 March 2026 at 18:00 UTC.",
                    complexSearchableDmi: new ComplexSearchableDmi
                    {
                        PulledAtBeforeThisDateTime = new DateTime(2026, 3, 20, 18, 0, 0, DateTimeKind.Utc),
                    }),

                CreateSample("Returns DMI entries for StationId = 12345 observed within the last 7 days.",
                    complexSearchableDmi: new ComplexSearchableDmi
                    {
                        Searchable = new SearchableDmi
                        {
                            StationId = 12345,
                        },
                        LastXDaysObservedAt = 7,
                    }),

                CreateSample(
                    "Returns DMI entries for ParameterId = TEMP_DRY ordered by ObservedAt descending. When retrieving a single entity, this can be used to get the last registered matching the query arguments.",
                    complexSearchableDmi: new ComplexSearchableDmi
                    {
                        Searchable = new SearchableDmi
                        {
                            ParameterId = DmiParameter.TEMP_DRY,
                        },
                        OrderByObservedAt = OrderDirection.DESCENDING,
                    }),

                CreateSample("Returns DMI entries for StationId = 12345 AND ParameterId = HUMIDITY.",
                    complexSearchableDmi: new ComplexSearchableDmi
                    {
                        Searchable = new SearchableDmi
                        {
                            StationId = 12345,
                            ParameterId = DmiParameter.HUMIDITY,
                        },
                    }),

                CreateSample("Returns DMI entries for a specific DmiId observed after 1 March 2026 at 00:00 UTC.",
                    complexSearchableDmi: new ComplexSearchableDmi
                    {
                        Searchable = new SearchableDmi
                        {
                            DmiId = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"),
                        },
                        ObservedAtAfterThisDateTime = new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                    }),

                CreateSample(
                    "Returns DMI entries for StationId = 67890 AND ParameterId = TEMP_DRY, ordered by PulledAt descending. When retrieving a single entity, this can be used to get the last registered matching the query arguments.",
                    complexSearchableDmi: new ComplexSearchableDmi
                    {
                        Searchable = new SearchableDmi
                        {
                            StationId = 67890,
                            ParameterId = DmiParameter.TEMP_DRY,
                        },
                        OrderByPulledAt = OrderDirection.DESCENDING,
                    }),

                CreateSample(
                    "Returns DMI entries for StationId = 12345 observed between 1 March 2026 and 20 March 2026 UTC.",
                    complexSearchableDmi: new ComplexSearchableDmi
                    {
                        Searchable = new SearchableDmi
                        {
                            StationId = 12345,
                        },
                        ObservedAtAfterThisDateTime = new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                        ObservedAtBeforeThisDateTime = new DateTime(2026, 3, 20, 0, 0, 0, DateTimeKind.Utc),
                    }),

                CreateSample(
                    "Returns DMI entries for a specific DmiId and StationId, pulled within the last 2 days and ordered by PulledAt ascending. When retrieving a single entity, this can be used to get the first registered matching the query arguments.",
                    complexSearchableDmi: new ComplexSearchableDmi
                    {
                        Searchable = new SearchableDmi
                        {
                            DmiId = Guid.Parse("ffffffff-1111-2222-3333-444444444444"),
                            StationId = 99999,
                        },
                        LastXDaysPulledAt = 2,
                        OrderByPulledAt = OrderDirection.ASCENDING,
                    }),
                CreateSample("Returns DMI entries with Value above 5.", complexSearchableDmi: new ComplexSearchableDmi
                {
                    AboveValue = 5.0f,
                }),

                CreateSample("Returns DMI entries with Value below 10.", complexSearchableDmi: new ComplexSearchableDmi
                {
                    BelowValue = 10.0f,
                }),

                CreateSample("Returns DMI entries with Value between 2 and 8.",
                    complexSearchableDmi: new ComplexSearchableDmi
                    {
                        AboveValue = 2.0f,
                        BelowValue = 8.0f,
                    }),

                CreateSample("Returns DMI entries for StationId = 12345 with Value above 5.",
                    complexSearchableDmi: new ComplexSearchableDmi
                    {
                        Searchable = new SearchableDmi
                        {
                            StationId = 12345,
                        },
                        AboveValue = 5.0f,
                    }),

                CreateSample(
                    "Returns DMI entries for ParameterId = TEMP_DRY with Value between 5 and 15 ordered by ObservedAt ascending.",
                    complexSearchableDmi: new ComplexSearchableDmi
                    {
                        Searchable = new SearchableDmi
                        {
                            ParameterId = DmiParameter.TEMP_DRY,
                        },
                        AboveValue = 5.0f,
                        BelowValue = 15.0f,
                        OrderByObservedAt = OrderDirection.ASCENDING,
                    }),

                CreateSample(
                    "Returns DMI entries for a specific DmiId with Value between 0 and 20 observed within the last 2 days.",
                    complexSearchableDmi: new ComplexSearchableDmi
                    {
                        Searchable = new SearchableDmi
                        {
                            DmiId = Guid.Parse("ffffffff-eeee-dddd-cccc-bbbbbbbbbbbb"),
                        },
                        AboveValue = 0.0f,
                        BelowValue = 20.0f,
                        LastXDaysObservedAt = 2,
                    }),
            ];
        }

        private List<SearchableSampleDto> BuildSearchableDsSamples()
        {
            return
            [
                CreateSample("Returns DS18B20 entries with Id equal to 1.", searchableDs: new SearchableDs
                {
                    Id = 1,
                }),

                CreateSample("Returns DS18B20 entries from the OUTSIDE location.", searchableDs: new SearchableDs
                {
                    Location = Location.OUTSIDE,
                }),

                CreateSample("Returns DS18B20 entries produced by a specific reader.", searchableDs: new SearchableDs
                {
                    ReaderId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                }),

                CreateSample("Returns DS18B20 entries matching BOTH Id = 1 AND Location = OUTSIDE.",
                    searchableDs: new SearchableDs
                    {
                        Id = 1,
                        Location = Location.OUTSIDE,
                    }),

                CreateSample("Returns DS18B20 entries matching Location = INSIDE AND a specific ReaderId.",
                    searchableDs: new SearchableDs
                    {
                        Location = Location.INSIDE,
                        ReaderId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    }),

                CreateSample("Returns DS18B20 entries matching ALL properties (Id, Location, and ReaderId).",
                    searchableDs: new SearchableDs
                    {
                        Id = 5,
                        Location = Location.OUTSIDE,
                        ReaderId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                    }),
            ];
        }

        private List<SearchableSampleDto> BuildComplexSearchableDsSamples()
        {
            return
            [
                CreateSample(
                    "Returns DS18B20 entries ordered by ObservedAt ascending. When retrieving a single entity, this can be used to get the first registered matching the query arguments.",
                    complexSearchableDs: new ComplexSearchableDs
                    {
                        OrderByObservedAt = OrderDirection.ASCENDING,
                    }),

                CreateSample(
                    "Returns DS18B20 entries ordered by PulledAt descending. When retrieving a single entity, this can be used to get the last registered matching the query arguments.",
                    complexSearchableDs: new ComplexSearchableDs
                    {
                        OrderByPulledAt = OrderDirection.DESCENDING,
                    }),

                CreateSample("Returns DS18B20 entries observed within the last 7 days.",
                    complexSearchableDs: new ComplexSearchableDs
                    {
                        LastXDaysObservedAt = 7,
                    }),

                CreateSample("Returns DS18B20 entries pulled within the last 3 days.",
                    complexSearchableDs: new ComplexSearchableDs
                    {
                        LastXDaysPulledAt = 3,
                    }),

                CreateSample("Returns DS18B20 entries observed after 1 March 2026 at 00:00 UTC.",
                    complexSearchableDs: new ComplexSearchableDs
                    {
                        ObservedAtAfterThisDateTime = new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                    }),

                CreateSample("Returns DS18B20 entries observed before 15 March 2026 at 12:00 UTC.",
                    complexSearchableDs: new ComplexSearchableDs
                    {
                        ObservedAtBeforeThisDateTime = new DateTime(2026, 3, 15, 12, 0, 0, DateTimeKind.Utc),
                    }),

                CreateSample("Returns DS18B20 entries pulled after 10 March 2026 at 08:00 UTC.",
                    complexSearchableDs: new ComplexSearchableDs
                    {
                        PulledAtAfterThisDateTime = new DateTime(2026, 3, 10, 8, 0, 0, DateTimeKind.Utc),
                    }),

                CreateSample("Returns DS18B20 entries pulled before 20 March 2026 at 18:00 UTC.",
                    complexSearchableDs: new ComplexSearchableDs
                    {
                        PulledAtBeforeThisDateTime = new DateTime(2026, 3, 20, 18, 0, 0, DateTimeKind.Utc),
                    }),

                CreateSample("Returns DS18B20 entries from the OUTSIDE location observed within the last 7 days.",
                    complexSearchableDs: new ComplexSearchableDs
                    {
                        Searchable = new SearchableDs
                        {
                            Location = Location.OUTSIDE,
                        },
                        LastXDaysObservedAt = 7,
                    }),

                CreateSample(
                    "Returns DS18B20 entries from the INSIDE location ordered by ObservedAt descending. When retrieving a single entity, this can be used to get the last registered matching the query arguments.",
                    complexSearchableDs: new ComplexSearchableDs
                    {
                        Searchable = new SearchableDs
                        {
                            Location = Location.INSIDE,
                        },
                        OrderByObservedAt = OrderDirection.DESCENDING,
                    }),

                CreateSample("Returns DS18B20 entries for a specific reader observed after 1 March 2026 at 00:00 UTC.",
                    complexSearchableDs: new ComplexSearchableDs
                    {
                        Searchable = new SearchableDs
                        {
                            ReaderId = Guid.Parse("12121212-3434-5656-7878-909090909090"),
                        },
                        ObservedAtAfterThisDateTime = new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                    }),

                CreateSample(
                    "Returns DS18B20 entries matching BOTH Id = 5 and Location = OUTSIDE, ordered by PulledAt descending. When retrieving a single entity, this can be used to get the last registered matching the query arguments.",
                    complexSearchableDs: new ComplexSearchableDs
                    {
                        Searchable = new SearchableDs
                        {
                            Id = 5,
                            Location = Location.OUTSIDE,
                        },
                        OrderByPulledAt = OrderDirection.DESCENDING,
                    }),

                CreateSample(
                    "Returns DS18B20 entries from the OUTSIDE location observed between 1 March 2026 and 20 March 2026 UTC.",
                    complexSearchableDs: new ComplexSearchableDs
                    {
                        Searchable = new SearchableDs
                        {
                            Location = Location.OUTSIDE,
                        },
                        ObservedAtAfterThisDateTime = new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                        ObservedAtBeforeThisDateTime = new DateTime(2026, 3, 20, 0, 0, 0, DateTimeKind.Utc),
                    }),

                CreateSample(
                    "Returns DS18B20 entries for a specific reader from the INSIDE location, pulled within the last 2 days and ordered by PulledAt ascending. When retrieving a single entity, this can be used to get the first registered matching the query arguments.",
                    complexSearchableDs: new ComplexSearchableDs
                    {
                        Searchable = new SearchableDs
                        {
                            ReaderId = Guid.Parse("abababab-cdcd-efef-1212-343434343434"),
                            Location = Location.INSIDE,
                        },
                        LastXDaysPulledAt = 2,
                        OrderByPulledAt = OrderDirection.ASCENDING,
                    }),
                CreateSample("Returns DS18B20 entries with Temperature above 20°C.",
                    complexSearchableDs: new ComplexSearchableDs
                    {
                        AboveTemperature = 20.0f,
                    }),

                CreateSample("Returns DS18B20 entries with Temperature below 10°C.",
                    complexSearchableDs: new ComplexSearchableDs
                    {
                        BelowTemperature = 10.0f,
                    }),

                CreateSample("Returns DS18B20 entries with Temperature between 18°C and 25°C.",
                    complexSearchableDs: new ComplexSearchableDs
                    {
                        AboveTemperature = 18.0f,
                        BelowTemperature = 25.0f,
                    }),

                CreateSample("Returns DS18B20 entries from the OUTSIDE location with Temperature above 5°C.",
                    complexSearchableDs: new ComplexSearchableDs
                    {
                        Searchable = new SearchableDs
                        {
                            Location = Location.OUTSIDE,
                        },
                        AboveTemperature = 5.0f,
                    }),

                CreateSample(
                    "Returns DS18B20 entries from a specific reader with Temperature between 15°C and 22°C ordered by ObservedAt descending.",
                    complexSearchableDs: new ComplexSearchableDs
                    {
                        Searchable = new SearchableDs
                        {
                            ReaderId = Guid.Parse("abababab-1111-2222-3333-cccccccccccc"),
                        },
                        AboveTemperature = 15.0f,
                        BelowTemperature = 22.0f,
                        OrderByObservedAt = OrderDirection.DESCENDING,
                    }),

                CreateSample(
                    "Returns DS18B20 entries from the INSIDE location with Temperature between 20°C and 24°C pulled within the last 1 day.",
                    complexSearchableDs: new ComplexSearchableDs
                    {
                        Searchable = new SearchableDs
                        {
                            Location = Location.INSIDE,
                        },
                        AboveTemperature = 20.0f,
                        BelowTemperature = 24.0f,
                        LastXDaysPulledAt = 1,
                    }),
            ];
        }

        private List<SearchableSampleDto> BuildSearchableScdSamples()
        {
            return
            [
                CreateSample("Returns SCD41 entries with Id equal to 1.", searchableScd: new SearchableScd
                {
                    Id = 1,
                }),

                CreateSample("Returns SCD41 entries produced by a specific reader.", searchableScd: new SearchableScd
                {
                    ReaderId = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                }),

                CreateSample("Returns SCD41 entries with CarbonDioxide equal to 400 ppm.",
                    searchableScd: new SearchableScd
                    {
                        CarbonDioxide = 400,
                    }),

                CreateSample("Returns SCD41 entries matching BOTH CarbonDioxide = 400 AND a specific ReaderId.",
                    searchableScd: new SearchableScd
                    {
                        CarbonDioxide = 400,
                        ReaderId = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                    }),

                CreateSample("Returns SCD41 entries matching Id AND CarbonDioxide.", searchableScd: new SearchableScd
                {
                    Id = 2,
                    CarbonDioxide = 800,
                }),

                CreateSample("Returns SCD41 entries matching ALL properties (Id, ReaderId, and CarbonDioxide).",
                    searchableScd: new SearchableScd
                    {
                        Id = 10,
                        ReaderId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                        CarbonDioxide = 1200,
                    }),
            ];
        }

        private List<SearchableSampleDto> BuildComplexSearchableScdSamples()
        {
            return
            [
                CreateSample(
                    "Returns SCD41 entries ordered by ObservedAt ascending. When retrieving a single entity, this can be used to get the first registered matching the query arguments.",
                    complexSearchableScd: new ComplexSearchableScd
                    {
                        OrderByObservedAt = OrderDirection.ASCENDING,
                    }),

                CreateSample(
                    "Returns SCD41 entries ordered by PulledAt descending. When retrieving a single entity, this can be used to get the last registered matching the query arguments.",
                    complexSearchableScd: new ComplexSearchableScd
                    {
                        OrderByPulledAt = OrderDirection.DESCENDING,
                    }),

                CreateSample("Returns SCD41 entries observed within the last 7 days.",
                    complexSearchableScd: new ComplexSearchableScd
                    {
                        LastXDaysObservedAt = 7,
                    }),

                CreateSample("Returns SCD41 entries pulled within the last 3 days.",
                    complexSearchableScd: new ComplexSearchableScd
                    {
                        LastXDaysPulledAt = 3,
                    }),

                CreateSample("Returns SCD41 entries observed after 1 March 2026 at 00:00 UTC.",
                    complexSearchableScd: new ComplexSearchableScd
                    {
                        ObservedAtAfterThisDateTime = new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                    }),

                CreateSample("Returns SCD41 entries observed before 15 March 2026 at 12:00 UTC.",
                    complexSearchableScd: new ComplexSearchableScd
                    {
                        ObservedAtBeforeThisDateTime = new DateTime(2026, 3, 15, 12, 0, 0, DateTimeKind.Utc),
                    }),

                CreateSample("Returns SCD41 entries pulled after 10 March 2026 at 08:00 UTC.",
                    complexSearchableScd: new ComplexSearchableScd
                    {
                        PulledAtAfterThisDateTime = new DateTime(2026, 3, 10, 8, 0, 0, DateTimeKind.Utc),
                    }),

                CreateSample("Returns SCD41 entries pulled before 20 March 2026 at 18:00 UTC.",
                    complexSearchableScd: new ComplexSearchableScd
                    {
                        PulledAtBeforeThisDateTime = new DateTime(2026, 3, 20, 18, 0, 0, DateTimeKind.Utc),
                    }),

                CreateSample("Returns SCD41 entries with CarbonDioxide = 400 observed within the last 7 days.",
                    complexSearchableScd: new ComplexSearchableScd
                    {
                        Searchable = new SearchableScd
                        {
                            CarbonDioxide = 400,
                        },
                        LastXDaysObservedAt = 7,
                    }),

                CreateSample(
                    "Returns SCD41 entries for a specific reader ordered by ObservedAt descending. When retrieving a single entity, this can be used to get the last registered matching the query arguments.",
                    complexSearchableScd: new ComplexSearchableScd
                    {
                        Searchable = new SearchableScd
                        {
                            ReaderId = Guid.Parse("10101010-2020-3030-4040-505050505050"),
                        },
                        OrderByObservedAt = OrderDirection.DESCENDING,
                    }),

                CreateSample("Returns SCD41 entries matching ReaderId AND CarbonDioxide.",
                    complexSearchableScd: new ComplexSearchableScd
                    {
                        Searchable = new SearchableScd
                        {
                            ReaderId = Guid.Parse("60606060-7070-8080-9090-a0a0a0a0a0a0"),
                            CarbonDioxide = 800,
                        },
                    }),

                CreateSample("Returns SCD41 entries with Id = 2 observed after 1 March 2026 at 00:00 UTC.",
                    complexSearchableScd: new ComplexSearchableScd
                    {
                        Searchable = new SearchableScd
                        {
                            Id = 2,
                        },
                        ObservedAtAfterThisDateTime = new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                    }),

                CreateSample(
                    "Returns SCD41 entries matching CarbonDioxide = 1200, ordered by PulledAt descending. When retrieving a single entity, this can be used to get the last registered matching the query arguments.",
                    complexSearchableScd: new ComplexSearchableScd
                    {
                        Searchable = new SearchableScd
                        {
                            CarbonDioxide = 1200,
                        },
                        OrderByPulledAt = OrderDirection.DESCENDING,
                    }),

                CreateSample(
                    "Returns SCD41 entries for a specific reader observed between 1 March 2026 and 20 March 2026 UTC.",
                    complexSearchableScd: new ComplexSearchableScd
                    {
                        Searchable = new SearchableScd
                        {
                            ReaderId = Guid.Parse("b1b1b1b1-c2c2-d3d3-e4e4-f5f5f5f5f5f5"),
                        },
                        ObservedAtAfterThisDateTime = new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                        ObservedAtBeforeThisDateTime = new DateTime(2026, 3, 20, 0, 0, 0, DateTimeKind.Utc),
                    }),

                CreateSample(
                    "Returns SCD41 entries for a specific reader with CarbonDioxide = 400, pulled within the last 2 days and ordered by PulledAt ascending. When retrieving a single entity, this can be used to get the first registered matching the query arguments.",
                    complexSearchableScd: new ComplexSearchableScd
                    {
                        Searchable = new SearchableScd
                        {
                            ReaderId = Guid.Parse("f0f0f0f0-e1e1-d2d2-c3c3-b4b4b4b4b4b4"),
                            CarbonDioxide = 400,
                        },
                        LastXDaysPulledAt = 2,
                        OrderByPulledAt = OrderDirection.ASCENDING,
                    }),
                CreateSample("Returns SCD41 entries with CarbonDioxide above 600 ppm.",
                    complexSearchableScd: new ComplexSearchableScd
                    {
                        AboveCarbonDioxide = 600,
                    }),

                CreateSample("Returns SCD41 entries with CarbonDioxide below 800 ppm.",
                    complexSearchableScd: new ComplexSearchableScd
                    {
                        BelowCarbonDioxide = 800,
                    }),

                CreateSample("Returns SCD41 entries with CarbonDioxide between 600 and 900 ppm.",
                    complexSearchableScd: new ComplexSearchableScd
                    {
                        AboveCarbonDioxide = 600,
                        BelowCarbonDioxide = 900,
                    }),

                CreateSample("Returns SCD41 entries with Temperature above 20°C and Humidity below 60%.",
                    complexSearchableScd: new ComplexSearchableScd
                    {
                        AboveTemperature = 20.0f,
                        BelowHumidity = 60.0f,
                    }),

                CreateSample("Returns SCD41 entries for a specific reader with CarbonDioxide between 500 and 800 ppm.",
                    complexSearchableScd: new ComplexSearchableScd
                    {
                        Searchable = new SearchableScd
                        {
                            ReaderId = Guid.Parse("dddddddd-aaaa-bbbb-cccc-eeeeeeeeeeee"),
                        },
                        AboveCarbonDioxide = 500,
                        BelowCarbonDioxide = 800,
                    }),

                CreateSample(
                    "Returns SCD41 entries with Humidity between 40% and 55% and CarbonDioxide between 600 and 900 ppm ordered by ObservedAt ascending.",
                    complexSearchableScd: new ComplexSearchableScd
                    {
                        AboveHumidity = 40.0f,
                        BelowHumidity = 55.0f,
                        AboveCarbonDioxide = 600,
                        BelowCarbonDioxide = 900,
                        OrderByObservedAt = OrderDirection.ASCENDING,
                    }),

                CreateSample(
                    "Returns SCD41 entries for a specific reader with Temperature between 21°C and 25°C and CarbonDioxide below 800 ppm observed within the last 2 days.",
                    complexSearchableScd: new ComplexSearchableScd
                    {
                        Searchable = new SearchableScd
                        {
                            ReaderId = Guid.Parse("aaaaaaaa-9999-8888-7777-666666666666"),
                        },
                        AboveTemperature = 21.0f,
                        BelowTemperature = 25.0f,
                        BelowCarbonDioxide = 800,
                        LastXDaysObservedAt = 2,
                    }),
            ];
        }

        private SearchableSampleDto CreateSample(
            string comment, SearchableBme? searchableBme = null, ComplexSearchableBme? complexSearchableBme = null,
            SearchableDmi? searchableDmi = null, ComplexSearchableDmi? complexSearchableDmi = null,
            SearchableDs? searchableDs = null, ComplexSearchableDs? complexSearchableDs = null,
            SearchableScd? searchableScd = null, ComplexSearchableScd? complexSearchableScd = null)
        {
            Normalize(complexSearchableBme);
            Normalize(complexSearchableDmi);
            Normalize(complexSearchableDs);
            Normalize(complexSearchableScd);

            int numberOfAssignedSearchables = new object?[]
            {
                searchableBme,
                complexSearchableBme,
                searchableDmi,
                complexSearchableDmi,
                searchableDs,
                complexSearchableDs,
                searchableScd,
                complexSearchableScd,
            }.Count(x => x is not null);

            return numberOfAssignedSearchables switch
            {
                1 => new SearchableSampleDto
                {
                    Comment = comment,
                    SearchableBme = searchableBme,
                    ComplexSearchableBme = complexSearchableBme,
                    SearchableDmi = searchableDmi,
                    ComplexSearchableDmi = complexSearchableDmi,
                    SearchableDs = searchableDs,
                    ComplexSearchableDs = complexSearchableDs,
                    SearchableScd = searchableScd,
                    ComplexSearchableScd = complexSearchableScd,
                },
                var _ => throw new ArgumentException(
                    "A SearchableSampleDto must have exactly one searchable or complex searchable property assigned."),
            };
        }

        private void Normalize<TSearchable>(IComplexSearchable<TSearchable>? complex)
            where TSearchable : class, ISearchable, new()
        {
            if (complex?.Searchable is null)
                return;

            if (IsEmpty(complex.Searchable))
                complex.Searchable = null!;
        }

        private bool IsEmpty<T>(T obj)
        {
            var properties = typeof(T).GetProperties();

            return properties.All(prop =>
            {
                object? value = prop.GetValue(obj);

                if (value is null)
                    return true;

                object? defaultValue =
                    prop.PropertyType.IsValueType ? Activator.CreateInstance(prop.PropertyType) : null;

                return Equals(value, defaultValue);
            });
        }
    }
}