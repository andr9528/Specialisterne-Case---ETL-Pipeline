using FluentAssertions;
using Weather.Abstraction.Enum;
using Weather.Model.ComplexSearchable;
using Weather.Model.Entity;
using Weather.Model.Searchable;
using Weather.Tests.Core;
using Weather.Tests.Core.SystemUnderTests;

namespace Weather.Tests
{
    public class DmiQueryServiceTests
    {
        public class GetAllEntities : BaseDatabaseTest
        {
            [Test]
            public async Task ReturnsAllStoredEntities()
            {
                // Arrange
                using DmiQueryServiceSut sut = this.CreateDmiQueryServiceSut();

                await sut.Factory.AddDmi(1);
                await sut.Factory.AddDmi(2);
                await sut.Factory.AddDmi(3);

                // Act
                var result = await sut.Service.GetAllEntities();

                // Assert
                result.Should().HaveCount(3);
            }
        }

        public class GetEntity : BaseDatabaseTest
        {
            [Test]
            public async Task WithMatchingId_ReturnsEntity()
            {
                // Arrange
                using DmiQueryServiceSut sut = this.CreateDmiQueryServiceSut();

                Dmi created = await sut.Factory.AddDmi(1);

                var searchable = new SearchableDmi
                {
                    Id = created.Id,
                };

                // Act
                Dmi? result = await sut.Service.GetEntity(searchable);

                // Assert
                result.Should().NotBeNull();
                result!.Id.Should().Be(created.Id);
            }

            [Test]
            public async Task WithNoMatch_ReturnsNull()
            {
                // Arrange
                using DmiQueryServiceSut sut = this.CreateDmiQueryServiceSut();

                var searchable = new SearchableDmi
                {
                    Id = 999999,
                };

                // Act
                Dmi? result = await sut.Service.GetEntity(searchable);

                // Assert
                result.Should().BeNull();
            }
        }

        public class GetEntities : BaseDatabaseTest
        {
            [Test]
            public async Task WithDmiId_ReturnsOnlyMatchingEntities()
            {
                // Arrange
                using DmiQueryServiceSut sut = this.CreateDmiQueryServiceSut();

                var matchingDmiId = Guid.NewGuid();

                await sut.Factory.AddDmi(1, matchingDmiId);
                await sut.Factory.AddDmi(2, matchingDmiId);
                await sut.Factory.AddDmi(3, Guid.NewGuid());

                var searchable = new SearchableDmi
                {
                    DmiId = matchingDmiId,
                };

                // Act
                var result = (await sut.Service.GetEntities(searchable)).ToList();

                // Assert
                result.Should().HaveCount(2);
                result.Should().OnlyContain(x => x.DmiId == matchingDmiId);
            }

            [Test]
            public async Task WithParameterId_ReturnsOnlyMatchingEntities()
            {
                // Arrange
                using DmiQueryServiceSut sut = this.CreateDmiQueryServiceSut();

                await sut.Factory.AddDmi(1, parameterId: DmiParameter.TEMP_DRY);
                await sut.Factory.AddDmi(2, parameterId: DmiParameter.TEMP_DRY);
                await sut.Factory.AddDmi(3, parameterId: DmiParameter.HUMIDITY);

                var searchable = new SearchableDmi
                {
                    ParameterId = DmiParameter.TEMP_DRY,
                };

                // Act
                var result = (await sut.Service.GetEntities(searchable)).ToList();

                // Assert
                result.Should().HaveCount(2);
                result.Should().OnlyContain(x => x.ParameterId == DmiParameter.TEMP_DRY);
            }

            [Test]
            public async Task WithStationId_ReturnsOnlyMatchingEntities()
            {
                // Arrange
                using DmiQueryServiceSut sut = this.CreateDmiQueryServiceSut();

                await sut.Factory.AddDmi(1, stationId: 1001);
                await sut.Factory.AddDmi(2, stationId: 1001);
                await sut.Factory.AddDmi(3, stationId: 2002);

                var searchable = new SearchableDmi
                {
                    StationId = 1001,
                };

                // Act
                var result = (await sut.Service.GetEntities(searchable)).ToList();

                // Assert
                result.Should().HaveCount(2);
                result.Should().OnlyContain(x => x.StationId == 1001);
            }
        }

        public class GetEntityComplex : BaseDatabaseTest
        {
            [Test]
            public async Task ReturnsFirstMatchingEntity()
            {
                // Arrange
                using DmiQueryServiceSut sut = this.CreateDmiQueryServiceSut();

                var dmiId = Guid.NewGuid();

                await sut.Factory.AddDmi(1, dmiId);
                await sut.Factory.AddDmi(2, dmiId);

                var complex = new ComplexSearchableDmi
                {
                    Searchable = new SearchableDmi
                    {
                        DmiId = dmiId,
                    },
                };

                // Act
                Dmi? result = await sut.Service.GetEntityComplex(complex);

                // Assert
                result.Should().NotBeNull();
                result!.DmiId.Should().Be(dmiId);
            }
        }

        public class GetEntitiesComplex : BaseDatabaseTest
        {
            [Test]
            public async Task WithObservedAtAfterThisDateTime_ReturnsOnlyMatchingEntities()
            {
                // Arrange
                using DmiQueryServiceSut sut = this.CreateDmiQueryServiceSut();
                DateTime now = DateTime.UtcNow;
                DateTime threshold = now.AddDays(-2);

                await sut.Factory.AddDmi(1, observedAt: now.AddDays(-4));
                await sut.Factory.AddDmi(2, observedAt: now.AddDays(-1));
                await sut.Factory.AddDmi(3, observedAt: now);

                var complex = new ComplexSearchableDmi
                {
                    Searchable = new SearchableDmi(),
                    ObservedAtAfterThisDateTime = threshold,
                };

                // Act
                var result = (await sut.Service.GetEntitiesComplex(complex)).ToList();

                // Assert
                result.Should().HaveCount(2);
                result.Should().OnlyContain(x => x.ObservedAt > threshold);
            }

            [Test]
            public async Task WithObservedAtBeforeThisDateTime_ReturnsOnlyMatchingEntities()
            {
                // Arrange
                using DmiQueryServiceSut sut = this.CreateDmiQueryServiceSut();
                DateTime now = DateTime.UtcNow;
                DateTime threshold = now.AddDays(-2);

                await sut.Factory.AddDmi(1, observedAt: now.AddDays(-5));
                await sut.Factory.AddDmi(2, observedAt: now.AddDays(-3));
                await sut.Factory.AddDmi(3, observedAt: now.AddDays(-1));

                var complex = new ComplexSearchableDmi
                {
                    Searchable = new SearchableDmi(),
                    ObservedAtBeforeThisDateTime = threshold,
                };

                // Act
                var result = (await sut.Service.GetEntitiesComplex(complex)).ToList();

                // Assert
                result.Should().HaveCount(2);
                result.Should().OnlyContain(x => x.ObservedAt < threshold);
            }

            [Test]
            public async Task WithLastXDaysObservedAt_ReturnsOnlyRecentEntities()
            {
                // Arrange
                using DmiQueryServiceSut sut = this.CreateDmiQueryServiceSut();
                DateTime now = DateTime.UtcNow;

                await sut.Factory.AddDmi(1, observedAt: now.AddDays(-10));
                await sut.Factory.AddDmi(2, observedAt: now.AddDays(-3));
                await sut.Factory.AddDmi(3, observedAt: now.AddDays(-1));

                var complex = new ComplexSearchableDmi
                {
                    Searchable = new SearchableDmi(),
                    LastXDaysObservedAt = 5,
                };

                // Act
                var result = (await sut.Service.GetEntitiesComplex(complex)).ToList();

                // Assert
                result.Should().HaveCount(2);
                result.Should().OnlyContain(x => x.ObservedAt >= now.AddDays(-5));
            }

            [Test]
            public async Task WithOrderByObservedAtAscending_ReturnsEntitiesInAscendingOrder()
            {
                // Arrange
                using DmiQueryServiceSut sut = this.CreateDmiQueryServiceSut();
                DateTime now = DateTime.UtcNow;

                Dmi third = await sut.Factory.AddDmi(1, observedAt: now.AddHours(3));
                Dmi first = await sut.Factory.AddDmi(2, observedAt: now.AddHours(1));
                Dmi second = await sut.Factory.AddDmi(3, observedAt: now.AddHours(2));

                var complex = new ComplexSearchableDmi
                {
                    Searchable = new SearchableDmi(),
                    OrderByObservedAt = OrderDirection.ASCENDING,
                };

                // Act
                var result = (await sut.Service.GetEntitiesComplex(complex)).ToList();

                // Assert
                result.Select(x => x.Id).Should().ContainInOrder(first.Id, second.Id, third.Id);
            }

            [Test]
            public async Task WithOrderByObservedAtDescending_ReturnsEntitiesInDescendingOrder()
            {
                // Arrange
                using DmiQueryServiceSut sut = this.CreateDmiQueryServiceSut();
                DateTime now = DateTime.UtcNow;

                Dmi first = await sut.Factory.AddDmi(1, observedAt: now.AddHours(1));
                Dmi second = await sut.Factory.AddDmi(2, observedAt: now.AddHours(2));
                Dmi third = await sut.Factory.AddDmi(3, observedAt: now.AddHours(3));

                var complex = new ComplexSearchableDmi
                {
                    Searchable = new SearchableDmi(),
                    OrderByObservedAt = OrderDirection.DESCENDING,
                };

                // Act
                var result = (await sut.Service.GetEntitiesComplex(complex)).ToList();

                // Assert
                result.Select(x => x.Id).Should().ContainInOrder(third.Id, second.Id, first.Id);
            }

            [Test]
            public async Task WithOrderByPulledAtAscending_ReturnsEntitiesInAscendingOrder()
            {
                // Arrange
                using DmiQueryServiceSut sut = this.CreateDmiQueryServiceSut();
                DateTime now = DateTime.UtcNow;

                Dmi third = await sut.Factory.AddDmi(1, pulledAt: now.AddHours(3));
                Dmi first = await sut.Factory.AddDmi(2, pulledAt: now.AddHours(1));
                Dmi second = await sut.Factory.AddDmi(3, pulledAt: now.AddHours(2));

                var complex = new ComplexSearchableDmi
                {
                    Searchable = new SearchableDmi(),
                    OrderByPulledAt = OrderDirection.ASCENDING,
                };

                // Act
                var result = (await sut.Service.GetEntitiesComplex(complex)).ToList();

                // Assert
                result.Select(x => x.Id).Should().ContainInOrder(first.Id, second.Id, third.Id);
            }

            [Test]
            public async Task WithOrderByPulledAtDescending_ReturnsEntitiesInDescendingOrder()
            {
                // Arrange
                using DmiQueryServiceSut sut = this.CreateDmiQueryServiceSut();
                DateTime now = DateTime.UtcNow;

                Dmi first = await sut.Factory.AddDmi(1, pulledAt: now.AddHours(3));
                Dmi second = await sut.Factory.AddDmi(2, pulledAt: now.AddHours(2));
                Dmi third = await sut.Factory.AddDmi(3, pulledAt: now.AddHours(1));

                var complex = new ComplexSearchableDmi
                {
                    Searchable = new SearchableDmi(),
                    OrderByPulledAt = OrderDirection.DESCENDING,
                };

                // Act
                var result = (await sut.Service.GetEntitiesComplex(complex)).ToList();

                // Assert
                result.Select(x => x.Id).Should().ContainInOrder(first.Id, second.Id, third.Id);
            }

            [Test]
            public async Task
                WithStationIdAndParameterIdAndOrderByObservedAtDescending_ReturnsMatchingEntitiesInExpectedOrder()
            {
                // Arrange
                using DmiQueryServiceSut sut = this.CreateDmiQueryServiceSut();
                DateTime now = DateTime.UtcNow;

                await sut.Factory.AddDmi(1, stationId: 999, parameterId: DmiParameter.TEMP_DRY,
                    observedAt: now.AddHours(-1));

                Dmi second = await sut.Factory.AddDmi(2, stationId: 123, parameterId: DmiParameter.TEMP_DRY,
                    observedAt: now.AddHours(-3));

                Dmi first = await sut.Factory.AddDmi(3, stationId: 123, parameterId: DmiParameter.TEMP_DRY,
                    observedAt: now.AddHours(-2));

                await sut.Factory.AddDmi(4, stationId: 123, parameterId: DmiParameter.HUMIDITY,
                    observedAt: now.AddHours(-4));

                var complex = new ComplexSearchableDmi
                {
                    Searchable = new SearchableDmi
                    {
                        StationId = 123,
                        ParameterId = DmiParameter.TEMP_DRY,
                    },
                    OrderByObservedAt = OrderDirection.DESCENDING,
                };

                // Act
                var result = (await sut.Service.GetEntitiesComplex(complex)).ToList();

                // Assert
                result.Should().HaveCount(2);
                result.Select(x => x.Id).Should().ContainInOrder(first.Id, second.Id);
            }

            [Test]
            public async Task
                WithDmiIdAndObservedAtAfterAndOrderByObservedAtAscending_ReturnsMatchingEntitiesInExpectedOrder()
            {
                // Arrange
                using DmiQueryServiceSut sut = this.CreateDmiQueryServiceSut();
                DateTime now = DateTime.UtcNow;
                DateTime threshold = now.AddHours(-4);
                var matchingDmiId = Guid.NewGuid();

                await sut.Factory.AddDmi(1, Guid.NewGuid(), observedAt: now.AddHours(-2));

                Dmi first = await sut.Factory.AddDmi(2, matchingDmiId, observedAt: now.AddHours(-3));

                Dmi second = await sut.Factory.AddDmi(3, matchingDmiId, observedAt: now.AddHours(-1));

                await sut.Factory.AddDmi(4, matchingDmiId, observedAt: now.AddHours(-5));

                var complex = new ComplexSearchableDmi
                {
                    Searchable = new SearchableDmi
                    {
                        DmiId = matchingDmiId,
                    },
                    ObservedAtAfterThisDateTime = threshold,
                    OrderByObservedAt = OrderDirection.ASCENDING,
                };

                // Act
                var result = (await sut.Service.GetEntitiesComplex(complex)).ToList();

                // Assert
                result.Should().HaveCount(2);
                result.Should().OnlyContain(x => x.DmiId == matchingDmiId && x.ObservedAt > threshold);
                result.Select(x => x.Id).Should().ContainInOrder(first.Id, second.Id);
            }

            [Test]
            public async Task WithStationIdAndObservedAtRangeAndPulledAtRange_ReturnsOnlyEntitiesMatchingAllCriteria()
            {
                // Arrange
                using DmiQueryServiceSut sut = this.CreateDmiQueryServiceSut();
                DateTime now = DateTime.UtcNow;

                DateTime observedAfter = now.AddHours(-6);
                DateTime observedBefore = now.AddHours(-2);
                DateTime pulledAfter = now.AddHours(-5);
                DateTime pulledBefore = now.AddHours(-1);

                Dmi match = await sut.Factory.AddDmi(1, stationId: 123, observedAt: now.AddHours(-4),
                    pulledAt: now.AddHours(-3));

                await sut.Factory.AddDmi(2, stationId: 999, observedAt: now.AddHours(-4), pulledAt: now.AddHours(-3));

                await sut.Factory.AddDmi(3, stationId: 123, observedAt: now.AddHours(-7), pulledAt: now.AddHours(-3));

                await sut.Factory.AddDmi(4, stationId: 123, observedAt: now.AddHours(-4), pulledAt: now);

                var complex = new ComplexSearchableDmi
                {
                    Searchable = new SearchableDmi
                    {
                        StationId = 123,
                    },
                    ObservedAtAfterThisDateTime = observedAfter,
                    ObservedAtBeforeThisDateTime = observedBefore,
                    PulledAtAfterThisDateTime = pulledAfter,
                    PulledAtBeforeThisDateTime = pulledBefore,
                };

                // Act
                var result = (await sut.Service.GetEntitiesComplex(complex)).ToList();

                // Assert
                result.Should().HaveCount(1);
                result.Single().Id.Should().Be(match.Id);
            }

            [Test]
            public async Task WithAboveValue_ReturnsOnlyMatchingEntities()
            {
                // Arrange
                using DmiQueryServiceSut sut = this.CreateDmiQueryServiceSut();

                await sut.Factory.AddDmi(1, value: 5.0f);
                await sut.Factory.AddDmi(2, value: 10.0f);
                await sut.Factory.AddDmi(3, value: 15.0f);

                var complex = new ComplexSearchableDmi
                {
                    Searchable = new SearchableDmi(),
                    AboveValue = 10.0f,
                };

                // Act
                var result = (await sut.Service.GetEntitiesComplex(complex)).ToList();

                // Assert
                result.Should().HaveCount(2);
                result.Should().OnlyContain(x => x.Value >= 10.0f);
            }

            [Test]
            public async Task WithBelowValue_ReturnsOnlyMatchingEntities()
            {
                // Arrange
                using DmiQueryServiceSut sut = this.CreateDmiQueryServiceSut();

                await sut.Factory.AddDmi(1, value: 5.0f);
                await sut.Factory.AddDmi(2, value: 10.0f);
                await sut.Factory.AddDmi(3, value: 15.0f);

                var complex = new ComplexSearchableDmi
                {
                    Searchable = new SearchableDmi(),
                    BelowValue = 10.0f,
                };

                // Act
                var result = (await sut.Service.GetEntitiesComplex(complex)).ToList();

                // Assert
                result.Should().HaveCount(2);
                result.Should().OnlyContain(x => x.Value <= 10.0f);
            }

            [Test]
            public async Task WithAboveValueAndBelowValue_ReturnsOnlyEntitiesWithinValueRange()
            {
                // Arrange
                using DmiQueryServiceSut sut = this.CreateDmiQueryServiceSut();

                await sut.Factory.AddDmi(1, value: 2.0f);
                Dmi first = await sut.Factory.AddDmi(2, value: 5.0f);
                Dmi second = await sut.Factory.AddDmi(3, value: 8.0f);
                await sut.Factory.AddDmi(4, value: 12.0f);

                var complex = new ComplexSearchableDmi
                {
                    Searchable = new SearchableDmi(),
                    AboveValue = 5.0f,
                    BelowValue = 8.0f,
                };

                // Act
                var result = (await sut.Service.GetEntitiesComplex(complex)).ToList();

                // Assert
                result.Should().HaveCount(2);
                result.Select(x => x.Id).Should().BeEquivalentTo([first.Id, second.Id,]);
                result.Should().OnlyContain(x => x.Value >= 5.0f && x.Value <= 8.0f);
            }

            [Test]
            public async Task WithStationIdAndParameterIdAndValueRange_ReturnsOnlyEntitiesMatchingAllCriteria()
            {
                // Arrange
                using DmiQueryServiceSut sut = this.CreateDmiQueryServiceSut();

                Dmi match = await sut.Factory.AddDmi(1, stationId: 123, parameterId: DmiParameter.TEMP_DRY,
                    value: 7.5f);

                await sut.Factory.AddDmi(2, stationId: 999, parameterId: DmiParameter.TEMP_DRY, value: 7.5f);

                await sut.Factory.AddDmi(3, stationId: 123, parameterId: DmiParameter.HUMIDITY, value: 7.5f);

                await sut.Factory.AddDmi(4, stationId: 123, parameterId: DmiParameter.TEMP_DRY, value: 12.0f);

                var complex = new ComplexSearchableDmi
                {
                    Searchable = new SearchableDmi
                    {
                        StationId = 123,
                        ParameterId = DmiParameter.TEMP_DRY,
                    },
                    AboveValue = 5.0f,
                    BelowValue = 10.0f,
                };

                // Act
                var result = (await sut.Service.GetEntitiesComplex(complex)).ToList();

                // Assert
                result.Should().HaveCount(1);
                result.Single().Id.Should().Be(match.Id);
            }

            [Test]
            public async Task
                WithDmiIdAndValueRangeAndOrderByObservedAtAscending_ReturnsMatchingEntitiesInExpectedOrder()
            {
                // Arrange
                using DmiQueryServiceSut sut = this.CreateDmiQueryServiceSut();

                DateTime now = DateTime.UtcNow;
                var matchingDmiId = Guid.NewGuid();

                await sut.Factory.AddDmi(1, Guid.NewGuid(), value: 7.0f, observedAt: now.AddHours(-2));
                await sut.Factory.AddDmi(2, matchingDmiId, value: 4.0f, observedAt: now.AddHours(-3));

                Dmi first = await sut.Factory.AddDmi(3, matchingDmiId, value: 6.0f, observedAt: now.AddHours(-2));
                Dmi second = await sut.Factory.AddDmi(4, matchingDmiId, value: 9.0f, observedAt: now.AddHours(-1));

                await sut.Factory.AddDmi(5, matchingDmiId, value: 11.0f, observedAt: now);

                var complex = new ComplexSearchableDmi
                {
                    Searchable = new SearchableDmi
                    {
                        DmiId = matchingDmiId,
                    },
                    AboveValue = 5.0f,
                    BelowValue = 10.0f,
                    OrderByObservedAt = OrderDirection.ASCENDING,
                };

                // Act
                var result = (await sut.Service.GetEntitiesComplex(complex)).ToList();

                // Assert
                result.Should().HaveCount(2);
                result.Should().OnlyContain(x => x.DmiId == matchingDmiId && x.Value >= 5.0f && x.Value <= 10.0f);

                result.Select(x => x.Id).Should().ContainInOrder(first.Id, second.Id);
            }
        }
    }
}