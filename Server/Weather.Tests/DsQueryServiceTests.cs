using FluentAssertions;
using Weather.Abstraction.Enum;
using Weather.Model.ComplexSearchable;
using Weather.Model.Searchable;
using Weather.Tests.Core;

namespace Weather.Tests
{
    public class DsQueryServiceTests
    {
        public class GetAllEntities : BaseDatabaseTest
        {
            [Test]
            public async Task ReturnsAllStoredEntities()
            {
                // Arrange
                using var sut = this.CreateDsQueryServiceSut();

                await sut.Factory.AddDs(id: 1);
                await sut.Factory.AddDs(id: 2);
                await sut.Factory.AddDs(id: 3);

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
                using var sut = this.CreateDsQueryServiceSut();

                var created = await sut.Factory.AddDs(id: 1);

                var searchable = new SearchableDs
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
                using var sut = this.CreateDsQueryServiceSut();

                var searchable = new SearchableDs
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
            public async Task WithReaderId_ReturnsOnlyMatchingEntities()
            {
                // Arrange
                using var sut = this.CreateDsQueryServiceSut();

                var matchingReaderId = Guid.NewGuid();

                await sut.Factory.AddDs(id: 1, readerId: matchingReaderId);
                await sut.Factory.AddDs(id: 2, readerId: matchingReaderId);
                await sut.Factory.AddDs(id: 3, readerId: Guid.NewGuid());

                var searchable = new SearchableDs
                {
                    ReaderId = matchingReaderId
                };

                // Act
                var result = (await sut.Service.GetEntities(searchable)).ToList();

                // Assert
                result.Should().HaveCount(2);
                result.Should().OnlyContain(x => x.ReaderId == matchingReaderId);
            }

            [Test]
            public async Task WithLocation_ReturnsOnlyMatchingEntities()
            {
                // Arrange
                using var sut = this.CreateDsQueryServiceSut();

                await sut.Factory.AddDs(id: 1, location: Location.INSIDE);
                await sut.Factory.AddDs(id: 2, location: Location.INSIDE);
                await sut.Factory.AddDs(id: 3, location: Location.OUTSIDE);

                var searchable = new SearchableDs
                {
                    Location = Location.INSIDE
                };

                // Act
                var result = (await sut.Service.GetEntities(searchable)).ToList();

                // Assert
                result.Should().HaveCount(2);
                result.Should().OnlyContain(x => x.Location == Location.INSIDE);
            }
        }

        public class GetEntityComplex : BaseDatabaseTest
        {
            [Test]
            public async Task ReturnsFirstMatchingEntity()
            {
                // Arrange
                using var sut = this.CreateDsQueryServiceSut();

                var readerId = Guid.NewGuid();

                await sut.Factory.AddDs(id: 1, readerId: readerId);
                await sut.Factory.AddDs(id: 2, readerId: readerId);

                var complex = new ComplexSearchableDs
                {
                    Searchable = new SearchableDs
                    {
                        ReaderId = readerId
                    }
                };

                // Act
                var result = await sut.Service.GetEntityComplex(complex);

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
                using var sut = this.CreateDsQueryServiceSut();
                var now = DateTime.UtcNow;
                var threshold = now.AddDays(-2);

                await sut.Factory.AddDs(id: 1, observedAt: now.AddDays(-4));
                await sut.Factory.AddDs(id: 2, observedAt: now.AddDays(-1));
                await sut.Factory.AddDs(id: 3, observedAt: now);

                var complex = new ComplexSearchableDs
                {
                    Searchable = new SearchableDs(),
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
                using var sut = this.CreateDsQueryServiceSut();
                var now = DateTime.UtcNow;
                var threshold = now.AddDays(-2);

                await sut.Factory.AddDs(id: 1, observedAt: now.AddDays(-5));
                await sut.Factory.AddDs(id: 2, observedAt: now.AddDays(-3));
                await sut.Factory.AddDs(id: 3, observedAt: now.AddDays(-1));

                var complex = new ComplexSearchableDs
                {
                    Searchable = new SearchableDs(),
                    ObservedAtBeforeThisDateTime = threshold
                };

                // Act
                var result = (await sut.Service.GetEntitiesComplex(complex)).ToList();

                // Assert
                result.Should().HaveCount(2);
                result.Should().OnlyContain(x => x.ObservedAt < threshold);
            }

            [Test]
            public async Task WithPulledAtAfterThisDateTime_ReturnsOnlyMatchingEntities()
            {
                // Arrange
                using var sut = this.CreateDsQueryServiceSut();
                var now = DateTime.UtcNow;
                var threshold = now.AddDays(-2);

                await sut.Factory.AddDs(id: 1, pulledAt: now.AddDays(-5));
                await sut.Factory.AddDs(id: 2, pulledAt: now.AddDays(-1));
                await sut.Factory.AddDs(id: 3, pulledAt: now);

                var complex = new ComplexSearchableDs
                {
                    Searchable = new SearchableDs(),
                    PulledAtAfterThisDateTime = threshold
                };

                // Act
                var result = (await sut.Service.GetEntitiesComplex(complex)).ToList();

                // Assert
                result.Should().HaveCount(2);
                result.Should().OnlyContain(x => x.PulledAt > threshold);
            }

            [Test]
            public async Task WithPulledAtBeforeThisDateTime_ReturnsOnlyMatchingEntities()
            {
                // Arrange
                using var sut = this.CreateDsQueryServiceSut();
                var now = DateTime.UtcNow;
                var threshold = now.AddDays(-2);

                await sut.Factory.AddDs(id: 1, pulledAt: now.AddDays(-5));
                await sut.Factory.AddDs(id: 2, pulledAt: now.AddDays(-3));
                await sut.Factory.AddDs(id: 3, pulledAt: now.AddDays(-1));

                var complex = new ComplexSearchableDs
                {
                    Searchable = new SearchableDs(),
                    PulledAtBeforeThisDateTime = threshold
                };

                // Act
                var result = (await sut.Service.GetEntitiesComplex(complex)).ToList();

                // Assert
                result.Should().HaveCount(2);
                result.Should().OnlyContain(x => x.PulledAt < threshold);
            }

            [Test]
            public async Task WithLastXDaysObservedAt_ReturnsOnlyRecentEntities()
            {
                // Arrange
                using var sut = this.CreateDsQueryServiceSut();
                var now = DateTime.UtcNow;

                await sut.Factory.AddDs(id: 1, observedAt: now.AddDays(-10));
                await sut.Factory.AddDs(id: 2, observedAt: now.AddDays(-3));
                await sut.Factory.AddDs(id: 3, observedAt: now.AddDays(-1));

                var complex = new ComplexSearchableDs
                {
                    Searchable = new SearchableDs(),
                    LastXDaysObservedAt = 5
                };

                // Act
                var result = (await sut.Service.GetEntitiesComplex(complex)).ToList();

                // Assert
                result.Should().HaveCount(2);
                result.Should().OnlyContain(x => x.ObservedAt >= now.AddDays(-5));
            }

            [Test]
            public async Task WithLastXDaysPulledAt_ReturnsOnlyRecentEntities()
            {
                // Arrange
                using var sut = this.CreateDsQueryServiceSut();
                var now = DateTime.UtcNow;

                await sut.Factory.AddDs(id: 1, pulledAt: now.AddDays(-10));
                await sut.Factory.AddDs(id: 2, pulledAt: now.AddDays(-3));
                await sut.Factory.AddDs(id: 3, pulledAt: now.AddDays(-1));

                var complex = new ComplexSearchableDs
                {
                    Searchable = new SearchableDs(),
                    LastXDaysPulledAt = 5
                };

                // Act
                var result = (await sut.Service.GetEntitiesComplex(complex)).ToList();

                // Assert
                result.Should().HaveCount(2);
                result.Should().OnlyContain(x => x.PulledAt >= now.AddDays(-5));
            }

            [Test]
            public async Task WithOrderByObservedAtAscending_ReturnsEntitiesInAscendingOrder()
            {
                // Arrange
                using var sut = this.CreateDsQueryServiceSut();
                var now = DateTime.UtcNow;

                var third = await sut.Factory.AddDs(id: 1, observedAt: now.AddHours(3));
                var first = await sut.Factory.AddDs(id: 2, observedAt: now.AddHours(1));
                var second = await sut.Factory.AddDs(id: 3, observedAt: now.AddHours(2));

                var complex = new ComplexSearchableDs
                {
                    Searchable = new SearchableDs(),
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
                using var sut = this.CreateDsQueryServiceSut();
                var now = DateTime.UtcNow;

                var first = await sut.Factory.AddDs(id: 1, observedAt: now.AddHours(1));
                var second = await sut.Factory.AddDs(id: 2, observedAt: now.AddHours(2));
                var third = await sut.Factory.AddDs(id: 3, observedAt: now.AddHours(3));

                var complex = new ComplexSearchableDs
                {
                    Searchable = new SearchableDs(),
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
                using var sut = this.CreateDsQueryServiceSut();
                var now = DateTime.UtcNow;

                var third = await sut.Factory.AddDs(id: 1, pulledAt: now.AddHours(3));
                var first = await sut.Factory.AddDs(id: 2, pulledAt: now.AddHours(1));
                var second = await sut.Factory.AddDs(id: 3, pulledAt: now.AddHours(2));

                var complex = new ComplexSearchableDs
                {
                    Searchable = new SearchableDs(),
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
                using var sut = this.CreateDsQueryServiceSut();
                var now = DateTime.UtcNow;

                var first = await sut.Factory.AddDs(id: 1, pulledAt: now.AddHours(3));
                var second = await sut.Factory.AddDs(id: 2, pulledAt: now.AddHours(2));
                var third = await sut.Factory.AddDs(id: 3, pulledAt: now.AddHours(1));

                var complex = new ComplexSearchableDs
                {
                    Searchable = new SearchableDs(),
                    OrderByPulledAt = OrderDirection.DESCENDING
                };

                // Act
                var result = (await sut.Service.GetEntitiesComplex(complex)).ToList();

                // Assert
                result.Select(x => x.Id).Should().ContainInOrder(first.Id, second.Id, third.Id);
            }

            [Test]
            public async Task WithLocationAndObservedAtAfterAndOrderByObservedAtAscending_ReturnsMatchingEntitiesInExpectedOrder()
            {
                // Arrange
                using var sut = this.CreateDsQueryServiceSut();
                var now = DateTime.UtcNow;
                var threshold = now.AddHours(-4);

                await sut.Factory.AddDs(id: 1, location: Location.OUTSIDE, observedAt: now.AddHours(-3));

                var first = await sut.Factory.AddDs(id: 2, location: Location.INSIDE, observedAt: now.AddHours(-2));
                var second = await sut.Factory.AddDs(id: 3, location: Location.INSIDE, observedAt: now.AddHours(-1));

                await sut.Factory.AddDs(id: 4, location: Location.INSIDE, observedAt: now.AddHours(-5));

                var complex = new ComplexSearchableDs
                {
                    Searchable = new SearchableDs
                    {
                        Location = Location.INSIDE
                    },
                    ObservedAtAfterThisDateTime = threshold,
                    OrderByObservedAt = OrderDirection.ASCENDING
                };

                // Act
                var result = (await sut.Service.GetEntitiesComplex(complex)).ToList();

                // Assert
                result.Should().HaveCount(2);
                result.Should().OnlyContain(x => x.Location == Location.INSIDE && x.ObservedAt > threshold);
                result.Select(x => x.Id).Should().ContainInOrder(first.Id, second.Id);
            }

            [Test]
            public async Task WithReaderIdAndPulledAtBeforeAndOrderByPulledAtDescending_ReturnsMatchingEntitiesInExpectedOrder()
            {
                // Arrange
                using var sut = this.CreateDsQueryServiceSut();
                var now = DateTime.UtcNow;
                var readerId = Guid.NewGuid();
                var threshold = now.AddHours(-1);

                await sut.Factory.AddDs(id: 1, readerId: Guid.NewGuid(), pulledAt: now.AddHours(-2));

                var second = await sut.Factory.AddDs(id: 2, readerId: readerId, pulledAt: now.AddHours(-3));
                var first = await sut.Factory.AddDs(id: 3, readerId: readerId, pulledAt: now.AddHours(-2));

                await sut.Factory.AddDs(id: 4, readerId: readerId, pulledAt: now);

                var complex = new ComplexSearchableDs
                {
                    Searchable = new SearchableDs
                    {
                        ReaderId = readerId
                    },
                    PulledAtBeforeThisDateTime = threshold,
                    OrderByPulledAt = OrderDirection.DESCENDING
                };

                // Act
                var result = (await sut.Service.GetEntitiesComplex(complex)).ToList();

                // Assert
                result.Should().HaveCount(2);
                result.Should().OnlyContain(x => x.ReaderId == readerId && x.PulledAt < threshold);
                result.Select(x => x.Id).Should().ContainInOrder(first.Id, second.Id);
            }

            [Test]
            public async Task WithLocationAndLastXDaysObservedAtAndOrderByObservedAtDescending_ReturnsOnlyRecentMatchingEntitiesInExpectedOrder()
            {
                // Arrange
                using var sut = this.CreateDsQueryServiceSut();
                var now = DateTime.UtcNow;

                await sut.Factory.AddDs(id: 1, location: Location.INSIDE, observedAt: now.AddDays(-10));

                var second = await sut.Factory.AddDs(id: 2, location: Location.INSIDE, observedAt: now.AddDays(-3));
                var first = await sut.Factory.AddDs(id: 3, location: Location.INSIDE, observedAt: now.AddDays(-1));

                await sut.Factory.AddDs(id: 4, location: Location.OUTSIDE, observedAt: now.AddDays(-2));

                var complex = new ComplexSearchableDs
                {
                    Searchable = new SearchableDs
                    {
                        Location = Location.INSIDE
                    },
                    LastXDaysObservedAt = 5,
                    OrderByObservedAt = OrderDirection.DESCENDING
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
                using var sut = this.CreateDsQueryServiceSut();
                var now = DateTime.UtcNow;

                var observedAfter = now.AddHours(-6);
                var observedBefore = now.AddHours(-2);
                var pulledAfter = now.AddHours(-5);
                var pulledBefore = now.AddHours(-1);

                var match = await sut.Factory.AddDs(
                    id: 1,
                    location: Location.INSIDE,
                    observedAt: now.AddHours(-4),
                    pulledAt: now.AddHours(-3));

                await sut.Factory.AddDs(
                    id: 2,
                    location: Location.OUTSIDE,
                    observedAt: now.AddHours(-4),
                    pulledAt: now.AddHours(-3));

                await sut.Factory.AddDs(
                    id: 3,
                    location: Location.INSIDE,
                    observedAt: now.AddHours(-7),
                    pulledAt: now.AddHours(-3));

                await sut.Factory.AddDs(
                    id: 4,
                    location: Location.INSIDE,
                    observedAt: now.AddHours(-4),
                    pulledAt: now);

                var complex = new ComplexSearchableDs
                {
                    Searchable = new SearchableDs
                    {
                        Location = Location.INSIDE
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