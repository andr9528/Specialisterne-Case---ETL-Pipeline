using FluentAssertions;
using Weather.Abstraction.Enum;
using Weather.Model.ComplexSearchable;
using Weather.Model.Entity;
using Weather.Model.Searchable;
using Weather.Tests.Core;
using Weather.Tests.Core.SystemUnderTests;

namespace Weather.Tests
{
    public class BmeQueryServiceTests
    {
        public class GetAllEntities : BaseDatabaseTest
        {
            [Test]
            public async Task ReturnsAllStoredEntities()
            {
                // Arrange
                using BmeQueryServiceSut sut = this.CreateBmeQueryServiceSut();

                await sut.Factory.AddBme();
                await sut.Factory.AddBme();
                await sut.Factory.AddBme();

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
                using BmeQueryServiceSut sut = this.CreateBmeQueryServiceSut();

                Bme created = await sut.Factory.AddBme();

                var searchable = new SearchableBme
                {
                    Id = created.Id,
                };

                // Act
                Bme? result = await sut.Service.GetEntity(searchable);

                // Assert
                result.Should().NotBeNull();
                result!.Id.Should().Be(created.Id);
            }

            [Test]
            public async Task WithNoMatch_ReturnsNull()
            {
                // Arrange
                using BmeQueryServiceSut sut = this.CreateBmeQueryServiceSut();

                var searchable = new SearchableBme
                {
                    Id = 999999,
                };

                // Act
                Bme? result = await sut.Service.GetEntity(searchable);

                // Assert
                result.Should().BeNull();
            }
        }

        public class GetEntities : BaseDatabaseTest
        {
            [Test]
            public async Task WithLocation_ReturnsOnlyMatchingEntities()
            {
                // Arrange
                using BmeQueryServiceSut sut = this.CreateBmeQueryServiceSut();

                await sut.Factory.AddBme(location: Location.INSIDE);
                await sut.Factory.AddBme(location: Location.INSIDE);
                await sut.Factory.AddBme(location: Location.OUTSIDE);

                var searchable = new SearchableBme
                {
                    Location = Location.INSIDE,
                };

                // Act
                var result = (await sut.Service.GetEntities(searchable)).ToList();

                // Assert
                result.Should().HaveCount(2);
                result.Should().OnlyContain(x => x.Location == Location.INSIDE);
            }

            [Test]
            public async Task WithReaderId_ReturnsOnlyMatchingEntities()
            {
                // Arrange
                using BmeQueryServiceSut sut = this.CreateBmeQueryServiceSut();

                var matchingReaderId = Guid.NewGuid();

                await sut.Factory.AddBme(readerId: matchingReaderId);
                await sut.Factory.AddBme(readerId: matchingReaderId);
                await sut.Factory.AddBme(readerId: Guid.NewGuid());

                var searchable = new SearchableBme
                {
                    ReaderId = matchingReaderId,
                };

                // Act
                var result = (await sut.Service.GetEntities(searchable)).ToList();

                // Assert
                result.Should().HaveCount(2);
                result.Should().OnlyContain(x => x.ReaderId == matchingReaderId);
            }
        }

        public class GetEntityComplex : BaseDatabaseTest
        {
            [Test]
            public async Task ReturnsFirstMatchingEntity()
            {
                // Arrange
                using BmeQueryServiceSut sut = this.CreateBmeQueryServiceSut();

                var readerId = Guid.NewGuid();

                await sut.Factory.AddBme(readerId: readerId);
                await sut.Factory.AddBme(readerId: readerId);

                var complex = new ComplexSearchableBme
                {
                    Searchable = new SearchableBme
                    {
                        ReaderId = readerId,
                    },
                };

                // Act
                Bme? result = await sut.Service.GetEntityComplex(complex);

                // Assert
                result.Should().NotBeNull();
                result!.ReaderId.Should().Be(readerId);
            }
        }

        public class GetEntitiesComplex : BaseDatabaseTest
        {
            [Test]
            public async Task WithObservedAtAfterThisDateTime_ReturnsOnlyMatchingEntities()
            {
                // Arrange
                using BmeQueryServiceSut sut = this.CreateBmeQueryServiceSut();

                DateTime threshold = DateTime.UtcNow.AddDays(-2);

                await sut.Factory.AddBme(observedAt: DateTime.UtcNow.AddDays(-4));
                await sut.Factory.AddBme(observedAt: DateTime.UtcNow.AddDays(-1));
                await sut.Factory.AddBme(observedAt: DateTime.UtcNow);

                var complex = new ComplexSearchableBme
                {
                    Searchable = new SearchableBme(),
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
                using BmeQueryServiceSut sut = this.CreateBmeQueryServiceSut();

                DateTime threshold = DateTime.UtcNow.AddDays(-2);

                await sut.Factory.AddBme(observedAt: DateTime.UtcNow.AddDays(-5));
                await sut.Factory.AddBme(observedAt: DateTime.UtcNow.AddDays(-3));
                await sut.Factory.AddBme(observedAt: DateTime.UtcNow.AddDays(-1));

                var complex = new ComplexSearchableBme
                {
                    Searchable = new SearchableBme(),
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
                using BmeQueryServiceSut sut = this.CreateBmeQueryServiceSut();
                DateTime now = DateTime.UtcNow;

                await sut.Factory.AddBme(observedAt: now.AddDays(-10));
                await sut.Factory.AddBme(observedAt: now.AddDays(-3));
                await sut.Factory.AddBme(observedAt: now.AddDays(-1));

                var complex = new ComplexSearchableBme
                {
                    Searchable = new SearchableBme(),
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
                using BmeQueryServiceSut sut = this.CreateBmeQueryServiceSut();

                DateTime now = DateTime.UtcNow;

                Bme third = await sut.Factory.AddBme(observedAt: now.AddHours(3));
                Bme first = await sut.Factory.AddBme(observedAt: now.AddHours(1));
                Bme second = await sut.Factory.AddBme(observedAt: now.AddHours(2));

                var complex = new ComplexSearchableBme
                {
                    Searchable = new SearchableBme(),
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
                using BmeQueryServiceSut sut = this.CreateBmeQueryServiceSut();

                DateTime now = DateTime.UtcNow;

                Bme first = await sut.Factory.AddBme(observedAt: now.AddHours(1));
                Bme second = await sut.Factory.AddBme(observedAt: now.AddHours(2));
                Bme third = await sut.Factory.AddBme(observedAt: now.AddHours(3));

                var complex = new ComplexSearchableBme
                {
                    Searchable = new SearchableBme(),
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
                using BmeQueryServiceSut sut = this.CreateBmeQueryServiceSut();

                DateTime now = DateTime.UtcNow;

                Bme third = await sut.Factory.AddBme(pulledAt: now.AddHours(3));
                Bme first = await sut.Factory.AddBme(pulledAt: now.AddHours(1));
                Bme second = await sut.Factory.AddBme(pulledAt: now.AddHours(2));

                var complex = new ComplexSearchableBme
                {
                    Searchable = new SearchableBme(),
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
                using BmeQueryServiceSut sut = this.CreateBmeQueryServiceSut();

                DateTime now = DateTime.UtcNow;

                Bme first = await sut.Factory.AddBme(pulledAt: now.AddHours(1));
                Bme second = await sut.Factory.AddBme(pulledAt: now.AddHours(2));
                Bme third = await sut.Factory.AddBme(pulledAt: now.AddHours(3));

                var complex = new ComplexSearchableBme
                {
                    Searchable = new SearchableBme(),
                    OrderByPulledAt = OrderDirection.DESCENDING,
                };

                // Act
                var result = (await sut.Service.GetEntitiesComplex(complex)).ToList();

                // Assert
                result.Select(x => x.Id).Should().ContainInOrder(third.Id, second.Id, first.Id);
            }

            [Test]
            public async Task
                WithLocationAndObservedAtAfterAndOrderByObservedAtAscending_ReturnsMatchingEntitiesInExpectedOrder()
            {
                // Arrange
                using BmeQueryServiceSut sut = this.CreateBmeQueryServiceSut();

                DateTime now = DateTime.UtcNow;
                DateTime threshold = now.AddHours(-4);

                await sut.Factory.AddBme(location: Location.OUTSIDE, observedAt: now.AddHours(-3));

                Bme first = await sut.Factory.AddBme(location: Location.INSIDE, observedAt: now.AddHours(-2));
                Bme second = await sut.Factory.AddBme(location: Location.INSIDE, observedAt: now.AddHours(-1));

                await sut.Factory.AddBme(location: Location.INSIDE, observedAt: now.AddHours(-5));

                var complex = new ComplexSearchableBme
                {
                    Searchable = new SearchableBme
                    {
                        Location = Location.INSIDE,
                    },
                    ObservedAtAfterThisDateTime = threshold,
                    OrderByObservedAt = OrderDirection.ASCENDING,
                };

                // Act
                var result = (await sut.Service.GetEntitiesComplex(complex)).ToList();

                // Assert
                result.Should().HaveCount(2);
                result.Should().OnlyContain(x => x.Location == Location.INSIDE && x.ObservedAt > threshold);
                result.Select(x => x.Id).Should().ContainInOrder(first.Id, second.Id);
            }

            [Test]
            public async Task
                WithReaderIdAndPulledAtBeforeAndOrderByPulledAtDescending_ReturnsMatchingEntitiesInExpectedOrder()
            {
                // Arrange
                using BmeQueryServiceSut sut = this.CreateBmeQueryServiceSut();

                DateTime now = DateTime.UtcNow;
                var readerId = Guid.NewGuid();
                DateTime threshold = now.AddHours(-1);

                await sut.Factory.AddBme(readerId: Guid.NewGuid(), pulledAt: now.AddHours(-2));

                Bme second = await sut.Factory.AddBme(readerId: readerId, pulledAt: now.AddHours(-3));
                Bme first = await sut.Factory.AddBme(readerId: readerId, pulledAt: now.AddHours(-2));

                await sut.Factory.AddBme(readerId: readerId, pulledAt: now);

                var complex = new ComplexSearchableBme
                {
                    Searchable = new SearchableBme
                    {
                        ReaderId = readerId,
                    },
                    PulledAtBeforeThisDateTime = threshold,
                    OrderByPulledAt = OrderDirection.DESCENDING,
                };

                // Act
                var result = (await sut.Service.GetEntitiesComplex(complex)).ToList();

                // Assert
                result.Should().HaveCount(2);
                result.Should().OnlyContain(x => x.ReaderId == readerId && x.PulledAt < threshold);
                result.Select(x => x.Id).Should().ContainInOrder(first.Id, second.Id);
            }

            [Test]
            public async Task
                WithLocationAndLastXDaysObservedAtAndOrderByObservedAtDescending_ReturnsOnlyRecentMatchingEntitiesInExpectedOrder()
            {
                // Arrange
                using BmeQueryServiceSut sut = this.CreateBmeQueryServiceSut();

                DateTime now = DateTime.UtcNow;

                await sut.Factory.AddBme(location: Location.INSIDE, observedAt: now.AddDays(-10));

                Bme second = await sut.Factory.AddBme(location: Location.INSIDE, observedAt: now.AddDays(-3));
                Bme first = await sut.Factory.AddBme(location: Location.INSIDE, observedAt: now.AddDays(-1));

                await sut.Factory.AddBme(location: Location.OUTSIDE, observedAt: now.AddDays(-2));

                var complex = new ComplexSearchableBme
                {
                    Searchable = new SearchableBme
                    {
                        Location = Location.INSIDE,
                    },
                    LastXDaysObservedAt = 5,
                    OrderByObservedAt = OrderDirection.DESCENDING,
                };

                // Act
                var result = (await sut.Service.GetEntitiesComplex(complex)).ToList();

                // Assert
                result.Should().HaveCount(2);
                result.Should().OnlyContain(x => x.Location == Location.INSIDE && x.ObservedAt >= now.AddDays(-5));
                result.Select(x => x.Id).Should().ContainInOrder(first.Id, second.Id);
            }

            [Test]
            public async Task WithLocationAndObservedAtRangeAndPulledAtRange_ReturnsOnlyEntitiesMatchingAllCriteria()
            {
                // Arrange
                using BmeQueryServiceSut sut = this.CreateBmeQueryServiceSut();

                DateTime now = DateTime.UtcNow;

                DateTime observedAfter = now.AddHours(-6);
                DateTime observedBefore = now.AddHours(-2);
                DateTime pulledAfter = now.AddHours(-5);
                DateTime pulledBefore = now.AddHours(-1);

                Bme match = await sut.Factory.AddBme(location: Location.INSIDE, observedAt: now.AddHours(-4),
                    pulledAt: now.AddHours(-3));

                await sut.Factory.AddBme(location: Location.OUTSIDE, observedAt: now.AddHours(-4),
                    pulledAt: now.AddHours(-3));

                await sut.Factory.AddBme(location: Location.INSIDE, observedAt: now.AddHours(-7),
                    pulledAt: now.AddHours(-3));

                await sut.Factory.AddBme(location: Location.INSIDE, observedAt: now.AddHours(-4), pulledAt: now);

                var complex = new ComplexSearchableBme
                {
                    Searchable = new SearchableBme
                    {
                        Location = Location.INSIDE,
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
            public async Task WithAboveTemperature_ReturnsOnlyMatchingEntities()
            {
                // Arrange
                using BmeQueryServiceSut sut = this.CreateBmeQueryServiceSut();

                await sut.Factory.AddBme(temperature: 19.5f);
                await sut.Factory.AddBme(temperature: 22.0f);
                await sut.Factory.AddBme(temperature: 25.5f);

                var complex = new ComplexSearchableBme
                {
                    Searchable = new SearchableBme(),
                    AboveTemperature = 22.0f,
                };

                // Act
                var result = (await sut.Service.GetEntitiesComplex(complex)).ToList();

                // Assert
                result.Should().HaveCount(2);
                result.Should().OnlyContain(x => x.Temperature >= 22.0f);
            }

            [Test]
            public async Task WithBelowHumidity_ReturnsOnlyMatchingEntities()
            {
                // Arrange
                using BmeQueryServiceSut sut = this.CreateBmeQueryServiceSut();

                await sut.Factory.AddBme(humidity: 35.0f);
                await sut.Factory.AddBme(humidity: 50.0f);
                await sut.Factory.AddBme(humidity: 65.0f);

                var complex = new ComplexSearchableBme
                {
                    Searchable = new SearchableBme(),
                    BelowHumidity = 50.0f,
                };

                // Act
                var result = (await sut.Service.GetEntitiesComplex(complex)).ToList();

                // Assert
                result.Should().HaveCount(2);
                result.Should().OnlyContain(x => x.Humidity <= 50.0f);
            }

            [Test]
            public async Task WithAbovePressureAndBelowPressure_ReturnsOnlyEntitiesWithinPressureRange()
            {
                // Arrange
                using BmeQueryServiceSut sut = this.CreateBmeQueryServiceSut();

                await sut.Factory.AddBme(pressure: 995.0f);
                Bme first = await sut.Factory.AddBme(pressure: 1005.0f);
                Bme second = await sut.Factory.AddBme(pressure: 1010.0f);
                await sut.Factory.AddBme(pressure: 1025.0f);

                var complex = new ComplexSearchableBme
                {
                    Searchable = new SearchableBme(),
                    AbovePressure = 1000.0f,
                    BelowPressure = 1015.0f,
                };

                // Act
                var result = (await sut.Service.GetEntitiesComplex(complex)).ToList();

                // Assert
                result.Should().HaveCount(2);
                result.Select(x => x.Id).Should().BeEquivalentTo([first.Id, second.Id,]);
                result.Should().OnlyContain(x => x.Pressure >= 1000.0f && x.Pressure <= 1015.0f);
            }

            [Test]
            public async Task WithLocationAndTemperatureRangeAndHumidityRange_ReturnsOnlyEntitiesMatchingAllCriteria()
            {
                // Arrange
                using BmeQueryServiceSut sut = this.CreateBmeQueryServiceSut();

                Bme match = await sut.Factory.AddBme(location: Location.INSIDE, temperature: 22.5f, humidity: 45.0f);

                await sut.Factory.AddBme(location: Location.OUTSIDE, temperature: 22.5f, humidity: 45.0f);

                await sut.Factory.AddBme(location: Location.INSIDE, temperature: 18.0f, humidity: 45.0f);

                await sut.Factory.AddBme(location: Location.INSIDE, temperature: 22.5f, humidity: 60.0f);

                var complex = new ComplexSearchableBme
                {
                    Searchable = new SearchableBme
                    {
                        Location = Location.INSIDE,
                    },
                    AboveTemperature = 20.0f,
                    BelowTemperature = 25.0f,
                    AboveHumidity = 40.0f,
                    BelowHumidity = 50.0f,
                };

                // Act
                var result = (await sut.Service.GetEntitiesComplex(complex)).ToList();

                // Assert
                result.Should().HaveCount(1);
                result.Single().Id.Should().Be(match.Id);
            }

            [Test]
            public async Task
                WithReaderIdAndTemperatureRangeAndOrderByTemperatureRelevantObservedAtOrdering_ReturnsMatchingEntities()
            {
                // Arrange
                using BmeQueryServiceSut sut = this.CreateBmeQueryServiceSut();

                DateTime now = DateTime.UtcNow;
                var readerId = Guid.NewGuid();

                await sut.Factory.AddBme(readerId: Guid.NewGuid(), temperature: 22.0f, observedAt: now.AddHours(-2));
                await sut.Factory.AddBme(readerId: readerId, temperature: 18.0f, observedAt: now.AddHours(-3));

                Bme first = await sut.Factory.AddBme(readerId: readerId, temperature: 21.0f,
                    observedAt: now.AddHours(-2));
                Bme second = await sut.Factory.AddBme(readerId: readerId, temperature: 24.0f,
                    observedAt: now.AddHours(-1));

                await sut.Factory.AddBme(readerId: readerId, temperature: 27.0f, observedAt: now);

                var complex = new ComplexSearchableBme
                {
                    Searchable = new SearchableBme
                    {
                        ReaderId = readerId,
                    },
                    AboveTemperature = 20.0f,
                    BelowTemperature = 25.0f,
                    OrderByObservedAt = OrderDirection.ASCENDING,
                };

                // Act
                var result = (await sut.Service.GetEntitiesComplex(complex)).ToList();

                // Assert
                result.Should().HaveCount(2);
                result.Should().OnlyContain(x =>
                    x.ReaderId == readerId && x.Temperature >= 20.0f && x.Temperature <= 25.0f);

                result.Select(x => x.Id).Should().ContainInOrder(first.Id, second.Id);
            }
        }
    }
}