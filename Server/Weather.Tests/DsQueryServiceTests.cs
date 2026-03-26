using FluentAssertions;
using Weather.Abstraction.Enum;
using Weather.Model.ComplexSearchable;
using Weather.Model.Entity;
using Weather.Model.Searchable;
using Weather.Tests.Core;
using Weather.Tests.Core.SystemUnderTests;

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
                using DsQueryServiceSut sut = this.CreateDsQueryServiceSut();

                await sut.Factory.AddDs(1);
                await sut.Factory.AddDs(2);
                await sut.Factory.AddDs(3);

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
                using DsQueryServiceSut sut = this.CreateDsQueryServiceSut();

                Ds created = await sut.Factory.AddDs(1);

                var searchable = new SearchableDs
                {
                    Id = created.Id,
                };

                // Act
                Ds? result = await sut.Service.GetEntity(searchable);

                // Assert
                result.Should().NotBeNull();
                result!.Id.Should().Be(created.Id);
            }

            [Test]
            public async Task WithNoMatch_ReturnsNull()
            {
                // Arrange
                using DsQueryServiceSut sut = this.CreateDsQueryServiceSut();

                var searchable = new SearchableDs
                {
                    Id = 999999,
                };

                // Act
                Ds? result = await sut.Service.GetEntity(searchable);

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
                using DsQueryServiceSut sut = this.CreateDsQueryServiceSut();

                var matchingReaderId = Guid.NewGuid();

                await sut.Factory.AddDs(1, matchingReaderId);
                await sut.Factory.AddDs(2, matchingReaderId);
                await sut.Factory.AddDs(3, Guid.NewGuid());

                var searchable = new SearchableDs
                {
                    ReaderId = matchingReaderId,
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
                using DsQueryServiceSut sut = this.CreateDsQueryServiceSut();

                await sut.Factory.AddDs(1, location: Location.INSIDE);
                await sut.Factory.AddDs(2, location: Location.INSIDE);
                await sut.Factory.AddDs(3, location: Location.OUTSIDE);

                var searchable = new SearchableDs
                {
                    Location = Location.INSIDE,
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
                using DsQueryServiceSut sut = this.CreateDsQueryServiceSut();

                var readerId = Guid.NewGuid();

                await sut.Factory.AddDs(1, readerId);
                await sut.Factory.AddDs(2, readerId);

                var complex = new ComplexSearchableDs
                {
                    Searchable = new SearchableDs
                    {
                        ReaderId = readerId,
                    },
                };

                // Act
                Ds? result = await sut.Service.GetEntityComplex(complex);

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
                using DsQueryServiceSut sut = this.CreateDsQueryServiceSut();
                DateTime now = DateTime.UtcNow;
                DateTime threshold = now.AddDays(-2);

                await sut.Factory.AddDs(1, observedAt: now.AddDays(-4));
                await sut.Factory.AddDs(2, observedAt: now.AddDays(-1));
                await sut.Factory.AddDs(3, observedAt: now);

                var complex = new ComplexSearchableDs
                {
                    Searchable = new SearchableDs(),
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
                using DsQueryServiceSut sut = this.CreateDsQueryServiceSut();
                DateTime now = DateTime.UtcNow;
                DateTime threshold = now.AddDays(-2);

                await sut.Factory.AddDs(1, observedAt: now.AddDays(-5));
                await sut.Factory.AddDs(2, observedAt: now.AddDays(-3));
                await sut.Factory.AddDs(3, observedAt: now.AddDays(-1));

                var complex = new ComplexSearchableDs
                {
                    Searchable = new SearchableDs(),
                    ObservedAtBeforeThisDateTime = threshold,
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
                using DsQueryServiceSut sut = this.CreateDsQueryServiceSut();
                DateTime now = DateTime.UtcNow;
                DateTime threshold = now.AddDays(-2);

                await sut.Factory.AddDs(1, pulledAt: now.AddDays(-5));
                await sut.Factory.AddDs(2, pulledAt: now.AddDays(-1));
                await sut.Factory.AddDs(3, pulledAt: now);

                var complex = new ComplexSearchableDs
                {
                    Searchable = new SearchableDs(),
                    PulledAtAfterThisDateTime = threshold,
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
                using DsQueryServiceSut sut = this.CreateDsQueryServiceSut();
                DateTime now = DateTime.UtcNow;
                DateTime threshold = now.AddDays(-2);

                await sut.Factory.AddDs(1, pulledAt: now.AddDays(-5));
                await sut.Factory.AddDs(2, pulledAt: now.AddDays(-3));
                await sut.Factory.AddDs(3, pulledAt: now.AddDays(-1));

                var complex = new ComplexSearchableDs
                {
                    Searchable = new SearchableDs(),
                    PulledAtBeforeThisDateTime = threshold,
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
                using DsQueryServiceSut sut = this.CreateDsQueryServiceSut();
                DateTime now = DateTime.UtcNow;

                await sut.Factory.AddDs(1, observedAt: now.AddDays(-10));
                await sut.Factory.AddDs(2, observedAt: now.AddDays(-3));
                await sut.Factory.AddDs(3, observedAt: now.AddDays(-1));

                var complex = new ComplexSearchableDs
                {
                    Searchable = new SearchableDs(),
                    LastXDaysObservedAt = 5,
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
                using DsQueryServiceSut sut = this.CreateDsQueryServiceSut();
                DateTime now = DateTime.UtcNow;

                await sut.Factory.AddDs(1, pulledAt: now.AddDays(-10));
                await sut.Factory.AddDs(2, pulledAt: now.AddDays(-3));
                await sut.Factory.AddDs(3, pulledAt: now.AddDays(-1));

                var complex = new ComplexSearchableDs
                {
                    Searchable = new SearchableDs(),
                    LastXDaysPulledAt = 5,
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
                using DsQueryServiceSut sut = this.CreateDsQueryServiceSut();
                DateTime now = DateTime.UtcNow;

                Ds third = await sut.Factory.AddDs(1, observedAt: now.AddHours(3));
                Ds first = await sut.Factory.AddDs(2, observedAt: now.AddHours(1));
                Ds second = await sut.Factory.AddDs(3, observedAt: now.AddHours(2));

                var complex = new ComplexSearchableDs
                {
                    Searchable = new SearchableDs(),
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
                using DsQueryServiceSut sut = this.CreateDsQueryServiceSut();
                DateTime now = DateTime.UtcNow;

                Ds first = await sut.Factory.AddDs(1, observedAt: now.AddHours(1));
                Ds second = await sut.Factory.AddDs(2, observedAt: now.AddHours(2));
                Ds third = await sut.Factory.AddDs(3, observedAt: now.AddHours(3));

                var complex = new ComplexSearchableDs
                {
                    Searchable = new SearchableDs(),
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
                using DsQueryServiceSut sut = this.CreateDsQueryServiceSut();
                DateTime now = DateTime.UtcNow;

                Ds third = await sut.Factory.AddDs(1, pulledAt: now.AddHours(3));
                Ds first = await sut.Factory.AddDs(2, pulledAt: now.AddHours(1));
                Ds second = await sut.Factory.AddDs(3, pulledAt: now.AddHours(2));

                var complex = new ComplexSearchableDs
                {
                    Searchable = new SearchableDs(),
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
                using DsQueryServiceSut sut = this.CreateDsQueryServiceSut();
                DateTime now = DateTime.UtcNow;

                Ds first = await sut.Factory.AddDs(1, pulledAt: now.AddHours(3));
                Ds second = await sut.Factory.AddDs(2, pulledAt: now.AddHours(2));
                Ds third = await sut.Factory.AddDs(3, pulledAt: now.AddHours(1));

                var complex = new ComplexSearchableDs
                {
                    Searchable = new SearchableDs(),
                    OrderByPulledAt = OrderDirection.DESCENDING,
                };

                // Act
                var result = (await sut.Service.GetEntitiesComplex(complex)).ToList();

                // Assert
                result.Select(x => x.Id).Should().ContainInOrder(first.Id, second.Id, third.Id);
            }

            [Test]
            public async Task
                WithLocationAndObservedAtAfterAndOrderByObservedAtAscending_ReturnsMatchingEntitiesInExpectedOrder()
            {
                // Arrange
                using DsQueryServiceSut sut = this.CreateDsQueryServiceSut();
                DateTime now = DateTime.UtcNow;
                DateTime threshold = now.AddHours(-4);

                await sut.Factory.AddDs(1, location: Location.OUTSIDE, observedAt: now.AddHours(-3));

                Ds first = await sut.Factory.AddDs(2, location: Location.INSIDE, observedAt: now.AddHours(-2));
                Ds second = await sut.Factory.AddDs(3, location: Location.INSIDE, observedAt: now.AddHours(-1));

                await sut.Factory.AddDs(4, location: Location.INSIDE, observedAt: now.AddHours(-5));

                var complex = new ComplexSearchableDs
                {
                    Searchable = new SearchableDs
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
                using DsQueryServiceSut sut = this.CreateDsQueryServiceSut();
                DateTime now = DateTime.UtcNow;
                var readerId = Guid.NewGuid();
                DateTime threshold = now.AddHours(-1);

                await sut.Factory.AddDs(1, Guid.NewGuid(), pulledAt: now.AddHours(-2));

                Ds second = await sut.Factory.AddDs(2, readerId, pulledAt: now.AddHours(-3));
                Ds first = await sut.Factory.AddDs(3, readerId, pulledAt: now.AddHours(-2));

                await sut.Factory.AddDs(4, readerId, pulledAt: now);

                var complex = new ComplexSearchableDs
                {
                    Searchable = new SearchableDs
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
                using DsQueryServiceSut sut = this.CreateDsQueryServiceSut();
                DateTime now = DateTime.UtcNow;

                await sut.Factory.AddDs(1, location: Location.INSIDE, observedAt: now.AddDays(-10));

                Ds second = await sut.Factory.AddDs(2, location: Location.INSIDE, observedAt: now.AddDays(-3));
                Ds first = await sut.Factory.AddDs(3, location: Location.INSIDE, observedAt: now.AddDays(-1));

                await sut.Factory.AddDs(4, location: Location.OUTSIDE, observedAt: now.AddDays(-2));

                var complex = new ComplexSearchableDs
                {
                    Searchable = new SearchableDs
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
                using DsQueryServiceSut sut = this.CreateDsQueryServiceSut();
                DateTime now = DateTime.UtcNow;

                DateTime observedAfter = now.AddHours(-6);
                DateTime observedBefore = now.AddHours(-2);
                DateTime pulledAfter = now.AddHours(-5);
                DateTime pulledBefore = now.AddHours(-1);

                Ds match = await sut.Factory.AddDs(1, location: Location.INSIDE, observedAt: now.AddHours(-4),
                    pulledAt: now.AddHours(-3));

                await sut.Factory.AddDs(2, location: Location.OUTSIDE, observedAt: now.AddHours(-4),
                    pulledAt: now.AddHours(-3));

                await sut.Factory.AddDs(3, location: Location.INSIDE, observedAt: now.AddHours(-7),
                    pulledAt: now.AddHours(-3));

                await sut.Factory.AddDs(4, location: Location.INSIDE, observedAt: now.AddHours(-4), pulledAt: now);

                var complex = new ComplexSearchableDs
                {
                    Searchable = new SearchableDs
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
        }
    }
}