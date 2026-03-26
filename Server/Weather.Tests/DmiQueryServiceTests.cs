using FluentAssertions;
using Weather.Abstraction.Enum;
using Weather.Model.ComplexSearchable;
using Weather.Model.Searchable;
using Weather.Tests.Core;

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
                using var sut = this.CreateDmiQueryServiceSut();

                await sut.Factory.AddDmi(id: 1);
                await sut.Factory.AddDmi(id: 2);
                await sut.Factory.AddDmi(id: 3);

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
                using var sut = this.CreateDmiQueryServiceSut();

                var created = await sut.Factory.AddDmi(id: 1);

                var searchable = new SearchableDmi
                {
                    Id = created.Id
                };

                // Act
                var result = await sut.Service.GetEntity(searchable);

                // Assert
                result.Should().NotBeNull();
                result!.Id.Should().Be(created.Id);
            }

            [Test]
            public async Task WithNoMatch_ReturnsNull()
            {
                // Arrange
                using var sut = this.CreateDmiQueryServiceSut();

                var searchable = new SearchableDmi
                {
                    Id = 999999
                };

                // Act
                var result = await sut.Service.GetEntity(searchable);

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
                using var sut = this.CreateDmiQueryServiceSut();

                var matchingDmiId = Guid.NewGuid();

                await sut.Factory.AddDmi(id: 1, dmiId: matchingDmiId);
                await sut.Factory.AddDmi(id: 2, dmiId: matchingDmiId);
                await sut.Factory.AddDmi(id: 3, dmiId: Guid.NewGuid());

                var searchable = new SearchableDmi
                {
                    DmiId = matchingDmiId
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
                using var sut = this.CreateDmiQueryServiceSut();

                await sut.Factory.AddDmi(id: 1, parameterId: DmiParameter.TEMP_DRY);
                await sut.Factory.AddDmi(id: 2, parameterId: DmiParameter.TEMP_DRY);
                await sut.Factory.AddDmi(id: 3, parameterId: DmiParameter.HUMIDITY);

                var searchable = new SearchableDmi
                {
                    ParameterId = DmiParameter.TEMP_DRY
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
                using var sut = this.CreateDmiQueryServiceSut();

                await sut.Factory.AddDmi(id: 1, stationId: 1001);
                await sut.Factory.AddDmi(id: 2, stationId: 1001);
                await sut.Factory.AddDmi(id: 3, stationId: 2002);

                var searchable = new SearchableDmi
                {
                    StationId = 1001
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
                using var sut = this.CreateDmiQueryServiceSut();

                var dmiId = Guid.NewGuid();

                await sut.Factory.AddDmi(id: 1, dmiId: dmiId);
                await sut.Factory.AddDmi(id: 2, dmiId: dmiId);

                var complex = new ComplexSearchableDmi
                {
                    Searchable = new SearchableDmi
                    {
                        DmiId = dmiId
                    }
                };

                // Act
                var result = await sut.Service.GetEntityComplex(complex);

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
                using var sut = this.CreateDmiQueryServiceSut();
                var now = DateTime.UtcNow;
                var threshold = now.AddDays(-2);

                await sut.Factory.AddDmi(id: 1, observedAt: now.AddDays(-4));
                await sut.Factory.AddDmi(id: 2, observedAt: now.AddDays(-1));
                await sut.Factory.AddDmi(id: 3, observedAt: now);

                var complex = new ComplexSearchableDmi
                {
                    Searchable = new SearchableDmi(),
                    ObservedAtAfterThisDateTime = threshold
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
                using var sut = this.CreateDmiQueryServiceSut();
                var now = DateTime.UtcNow;
                var threshold = now.AddDays(-2);

                await sut.Factory.AddDmi(id: 1, observedAt: now.AddDays(-5));
                await sut.Factory.AddDmi(id: 2, observedAt: now.AddDays(-3));
                await sut.Factory.AddDmi(id: 3, observedAt: now.AddDays(-1));

                var complex = new ComplexSearchableDmi
                {
                    Searchable = new SearchableDmi(),
                    ObservedAtBeforeThisDateTime = threshold
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
                using var sut = this.CreateDmiQueryServiceSut();
                var now = DateTime.UtcNow;

                await sut.Factory.AddDmi(id: 1, observedAt: now.AddDays(-10));
                await sut.Factory.AddDmi(id: 2, observedAt: now.AddDays(-3));
                await sut.Factory.AddDmi(id: 3, observedAt: now.AddDays(-1));

                var complex = new ComplexSearchableDmi
                {
                    Searchable = new SearchableDmi(),
                    LastXDaysObservedAt = 5
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
                using var sut = this.CreateDmiQueryServiceSut();
                var now = DateTime.UtcNow;

                var third = await sut.Factory.AddDmi(id: 1, observedAt: now.AddHours(3));
                var first = await sut.Factory.AddDmi(id: 2, observedAt: now.AddHours(1));
                var second = await sut.Factory.AddDmi(id: 3, observedAt: now.AddHours(2));

                var complex = new ComplexSearchableDmi
                {
                    Searchable = new SearchableDmi(),
                    OrderByObservedAt = OrderDirection.ASCENDING
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
                using var sut = this.CreateDmiQueryServiceSut();
                var now = DateTime.UtcNow;

                var first = await sut.Factory.AddDmi(id: 1, observedAt: now.AddHours(1));
                var second = await sut.Factory.AddDmi(id: 2, observedAt: now.AddHours(2));
                var third = await sut.Factory.AddDmi(id: 3, observedAt: now.AddHours(3));

                var complex = new ComplexSearchableDmi
                {
                    Searchable = new SearchableDmi(),
                    OrderByObservedAt = OrderDirection.DESCENDING
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
                using var sut = this.CreateDmiQueryServiceSut();
                var now = DateTime.UtcNow;

                var third = await sut.Factory.AddDmi(id: 1, pulledAt: now.AddHours(3));
                var first = await sut.Factory.AddDmi(id: 2, pulledAt: now.AddHours(1));
                var second = await sut.Factory.AddDmi(id: 3, pulledAt: now.AddHours(2));

                var complex = new ComplexSearchableDmi
                {
                    Searchable = new SearchableDmi(),
                    OrderByPulledAt = OrderDirection.ASCENDING
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
                using var sut = this.CreateDmiQueryServiceSut();
                var now = DateTime.UtcNow;

                var first = await sut.Factory.AddDmi(id: 1, pulledAt: now.AddHours(3));
                var second = await sut.Factory.AddDmi(id: 2, pulledAt: now.AddHours(2));
                var third = await sut.Factory.AddDmi(id: 3, pulledAt: now.AddHours(1));

                var complex = new ComplexSearchableDmi
                {
                    Searchable = new SearchableDmi(),
                    OrderByPulledAt = OrderDirection.DESCENDING
                };

                // Act
                var result = (await sut.Service.GetEntitiesComplex(complex)).ToList();

                // Assert
                result.Select(x => x.Id).Should().ContainInOrder(first.Id, second.Id, third.Id);
            }

            [Test]
            public async Task WithStationIdAndParameterIdAndOrderByObservedAtDescending_ReturnsMatchingEntitiesInExpectedOrder()
            {
                // Arrange
                using var sut = this.CreateDmiQueryServiceSut();
                var now = DateTime.UtcNow;

                await sut.Factory.AddDmi(
                    id: 1,
                    stationId: 999,
                    parameterId: DmiParameter.TEMP_DRY,
                    observedAt: now.AddHours(-1));

                var second = await sut.Factory.AddDmi(
                    id: 2,
                    stationId: 123,
                    parameterId: DmiParameter.TEMP_DRY,
                    observedAt: now.AddHours(-3));

                var first = await sut.Factory.AddDmi(
                    id: 3,
                    stationId: 123,
                    parameterId: DmiParameter.TEMP_DRY,
                    observedAt: now.AddHours(-2));

                await sut.Factory.AddDmi(
                    id: 4,
                    stationId: 123,
                    parameterId: DmiParameter.HUMIDITY,
                    observedAt: now.AddHours(-4));

                var complex = new ComplexSearchableDmi
                {
                    Searchable = new SearchableDmi
                    {
                        StationId = 123,
                        ParameterId = DmiParameter.TEMP_DRY
                    },
                    OrderByObservedAt = OrderDirection.DESCENDING
                };

                // Act
                var result = (await sut.Service.GetEntitiesComplex(complex)).ToList();

                // Assert
                result.Should().HaveCount(2);
                result.Select(x => x.Id).Should().ContainInOrder(first.Id, second.Id);
            }

            [Test]
            public async Task WithDmiIdAndObservedAtAfterAndOrderByObservedAtAscending_ReturnsMatchingEntitiesInExpectedOrder()
            {
                // Arrange
                using var sut = this.CreateDmiQueryServiceSut();
                var now = DateTime.UtcNow;
                var threshold = now.AddHours(-4);
                var matchingDmiId = Guid.NewGuid();

                await sut.Factory.AddDmi(
                    id: 1,
                    dmiId: Guid.NewGuid(),
                    observedAt: now.AddHours(-2));

                var first = await sut.Factory.AddDmi(
                    id: 2,
                    dmiId: matchingDmiId,
                    observedAt: now.AddHours(-3));

                var second = await sut.Factory.AddDmi(
                    id: 3,
                    dmiId: matchingDmiId,
                    observedAt: now.AddHours(-1));

                await sut.Factory.AddDmi(
                    id: 4,
                    dmiId: matchingDmiId,
                    observedAt: now.AddHours(-5));

                var complex = new ComplexSearchableDmi
                {
                    Searchable = new SearchableDmi
                    {
                        DmiId = matchingDmiId
                    },
                    ObservedAtAfterThisDateTime = threshold,
                    OrderByObservedAt = OrderDirection.ASCENDING
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
                using var sut = this.CreateDmiQueryServiceSut();
                var now = DateTime.UtcNow;

                var observedAfter = now.AddHours(-6);
                var observedBefore = now.AddHours(-2);
                var pulledAfter = now.AddHours(-5);
                var pulledBefore = now.AddHours(-1);

                var match = await sut.Factory.AddDmi(
                    id: 1,
                    stationId: 123,
                    observedAt: now.AddHours(-4),
                    pulledAt: now.AddHours(-3));

                await sut.Factory.AddDmi(
                    id: 2,
                    stationId: 999,
                    observedAt: now.AddHours(-4),
                    pulledAt: now.AddHours(-3));

                await sut.Factory.AddDmi(
                    id: 3,
                    stationId: 123,
                    observedAt: now.AddHours(-7),
                    pulledAt: now.AddHours(-3));

                await sut.Factory.AddDmi(
                    id: 4,
                    stationId: 123,
                    observedAt: now.AddHours(-4),
                    pulledAt: now);

                var complex = new ComplexSearchableDmi
                {
                    Searchable = new SearchableDmi
                    {
                        StationId = 123
                    },
                    ObservedAtAfterThisDateTime = observedAfter,
                    ObservedAtBeforeThisDateTime = observedBefore,
                    PulledAtAfterThisDateTime = pulledAfter,
                    PulledAtBeforeThisDateTime = pulledBefore
                };

                // Act
                var result = (await sut.Service.GetEntitiesComplex(complex)).ToList();

                // Assert
                result.Should().HaveCount(1);
                result.Single().Id.Should().Be(match.Id);
            }
        }
    }
}