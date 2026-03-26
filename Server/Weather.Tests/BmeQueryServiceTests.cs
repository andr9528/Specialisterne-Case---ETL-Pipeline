using FluentAssertions;
using Weather.Abstraction.Enum;
using Weather.Model.ComplexSearchable;
using Weather.Model.Searchable;
using Weather.Tests.Core;

namespace Weather.Tests
{
    public class BmeQueryServiceTests : BaseDatabaseTest
    {
        [Test]
        public async Task AddBme_SmokeTest()
        {
            using var sut = this.CreateBmeQueryServiceSut();

            var action = async () => await sut.Factory.AddBme();

            await action.Should().NotThrowAsync();
        }

        public class GetAllEntities : BaseDatabaseTest
        {
            [Test]
            public async Task ReturnsAllStoredEntities()
            {
                // Arrange
                using var sut = this.CreateBmeQueryServiceSut();

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
                using var sut = this.CreateBmeQueryServiceSut();

                var created = await sut.Factory.AddBme();

                var searchable = new SearchableBme
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
                using var sut = this.CreateBmeQueryServiceSut();

                var searchable = new SearchableBme
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
            public async Task WithLocation_ReturnsOnlyMatchingEntities()
            {
                // Arrange
                using var sut = this.CreateBmeQueryServiceSut();

                await sut.Factory.AddBme(location: Location.INSIDE);
                await sut.Factory.AddBme(location: Location.INSIDE);
                await sut.Factory.AddBme(location: Location.OUTSIDE);

                var searchable = new SearchableBme
                {
                    Location = Location.INSIDE
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
                using var sut = this.CreateBmeQueryServiceSut();

                var matchingReaderId = Guid.NewGuid();

                await sut.Factory.AddBme(readerId: matchingReaderId);
                await sut.Factory.AddBme(readerId: matchingReaderId);
                await sut.Factory.AddBme(readerId: Guid.NewGuid());

                var searchable = new SearchableBme
                {
                    ReaderId = matchingReaderId
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
                using var sut = this.CreateBmeQueryServiceSut();

                var readerId = Guid.NewGuid();

                await sut.Factory.AddBme(readerId: readerId);
                await sut.Factory.AddBme(readerId: readerId);

                var complex = new ComplexSearchableBme
                {
                    Searchable = new SearchableBme
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
                using var sut = this.CreateBmeQueryServiceSut();

                var threshold = DateTime.UtcNow.AddDays(-2);

                await sut.Factory.AddBme(observedAt: DateTime.UtcNow.AddDays(-4));
                await sut.Factory.AddBme(observedAt: DateTime.UtcNow.AddDays(-1));
                await sut.Factory.AddBme(observedAt: DateTime.UtcNow);

                var complex = new ComplexSearchableBme
                {
                    Searchable = new SearchableBme(),
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
                using var sut = this.CreateBmeQueryServiceSut();

                var threshold = DateTime.UtcNow.AddDays(-2);

                await sut.Factory.AddBme(observedAt: DateTime.UtcNow.AddDays(-5));
                await sut.Factory.AddBme(observedAt: DateTime.UtcNow.AddDays(-3));
                await sut.Factory.AddBme(observedAt: DateTime.UtcNow.AddDays(-1));

                var complex = new ComplexSearchableBme
                {
                    Searchable = new SearchableBme(),
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
                using var sut = this.CreateBmeQueryServiceSut();
                var now = DateTime.UtcNow;

                await sut.Factory.AddBme(observedAt: now.AddDays(-10));
                await sut.Factory.AddBme(observedAt: now.AddDays(-3));
                await sut.Factory.AddBme(observedAt: now.AddDays(-1));

                var complex = new ComplexSearchableBme
                {
                    Searchable = new SearchableBme(),
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
                using var sut = this.CreateBmeQueryServiceSut();

                var now = DateTime.UtcNow;

                var third = await sut.Factory.AddBme(observedAt: now.AddHours(3));
                var first = await sut.Factory.AddBme(observedAt: now.AddHours(1));
                var second = await sut.Factory.AddBme(observedAt: now.AddHours(2));

                var complex = new ComplexSearchableBme
                {
                    Searchable = new SearchableBme(),
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
                using var sut = this.CreateBmeQueryServiceSut();

                var now = DateTime.UtcNow;

                var first = await sut.Factory.AddBme(observedAt: now.AddHours(1));
                var second = await sut.Factory.AddBme(observedAt: now.AddHours(2));
                var third = await sut.Factory.AddBme(observedAt: now.AddHours(3));

                var complex = new ComplexSearchableBme
                {
                    Searchable = new SearchableBme(),
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
                using var sut = this.CreateBmeQueryServiceSut();

                var now = DateTime.UtcNow;

                var third = await sut.Factory.AddBme(pulledAt: now.AddHours(3));
                var first = await sut.Factory.AddBme(pulledAt: now.AddHours(1));
                var second = await sut.Factory.AddBme(pulledAt: now.AddHours(2));

                var complex = new ComplexSearchableBme
                {
                    Searchable = new SearchableBme(),
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
                using var sut = this.CreateBmeQueryServiceSut();

                var now = DateTime.UtcNow;

                var first = await sut.Factory.AddBme(pulledAt: now.AddHours(1));
                var second = await sut.Factory.AddBme(pulledAt: now.AddHours(2));
                var third = await sut.Factory.AddBme(pulledAt: now.AddHours(3));

                var complex = new ComplexSearchableBme
                {
                    Searchable = new SearchableBme(),
                    OrderByPulledAt = OrderDirection.DESCENDING
                };

                // Act
                var result = (await sut.Service.GetEntitiesComplex(complex)).ToList();

                // Assert
                result.Select(x => x.Id).Should().ContainInOrder(third.Id, second.Id, first.Id);
            }

            [Test]
            public async Task WithLocationAndObservedAtAfterAndOrderByObservedAtAscending_ReturnsMatchingEntitiesInExpectedOrder()
            {
                // Arrange
                using var sut = this.CreateBmeQueryServiceSut();

                var now = DateTime.UtcNow;
                var threshold = now.AddHours(-4);

                await sut.Factory.AddBme(location: Location.OUTSIDE, observedAt: now.AddHours(-3));

                var first = await sut.Factory.AddBme(location: Location.INSIDE, observedAt: now.AddHours(-2));
                var second = await sut.Factory.AddBme(location: Location.INSIDE, observedAt: now.AddHours(-1));

                await sut.Factory.AddBme(location: Location.INSIDE, observedAt: now.AddHours(-5));

                var complex = new ComplexSearchableBme
                {
                    Searchable = new SearchableBme
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
                using var sut = this.CreateBmeQueryServiceSut();

                var now = DateTime.UtcNow;
                var readerId = Guid.NewGuid();
                var threshold = now.AddHours(-1);

                await sut.Factory.AddBme(readerId: Guid.NewGuid(), pulledAt: now.AddHours(-2));

                var second = await sut.Factory.AddBme(readerId: readerId, pulledAt: now.AddHours(-3));
                var first = await sut.Factory.AddBme(readerId: readerId, pulledAt: now.AddHours(-2));

                await sut.Factory.AddBme(readerId: readerId, pulledAt: now);

                var complex = new ComplexSearchableBme
                {
                    Searchable = new SearchableBme
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
                using var sut = this.CreateBmeQueryServiceSut();

                var now = DateTime.UtcNow;

                await sut.Factory.AddBme(location: Location.INSIDE, observedAt: now.AddDays(-10));

                var second = await sut.Factory.AddBme(location: Location.INSIDE, observedAt: now.AddDays(-3));
                var first = await sut.Factory.AddBme(location: Location.INSIDE, observedAt: now.AddDays(-1));

                await sut.Factory.AddBme(location: Location.OUTSIDE, observedAt: now.AddDays(-2));

                var complex = new ComplexSearchableBme
                {
                    Searchable = new SearchableBme
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
                using var sut = this.CreateBmeQueryServiceSut();

                var now = DateTime.UtcNow;

                var observedAfter = now.AddHours(-6);
                var observedBefore = now.AddHours(-2);
                var pulledAfter = now.AddHours(-5);
                var pulledBefore = now.AddHours(-1);

                var match = await sut.Factory.AddBme(
                    location: Location.INSIDE,
                    observedAt: now.AddHours(-4),
                    pulledAt: now.AddHours(-3));

                await sut.Factory.AddBme(
                    location: Location.OUTSIDE,
                    observedAt: now.AddHours(-4),
                    pulledAt: now.AddHours(-3));

                await sut.Factory.AddBme(
                    location: Location.INSIDE,
                    observedAt: now.AddHours(-7),
                    pulledAt: now.AddHours(-3));

                await sut.Factory.AddBme(
                    location: Location.INSIDE,
                    observedAt: now.AddHours(-4),
                    pulledAt: now);

                var complex = new ComplexSearchableBme
                {
                    Searchable = new SearchableBme
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