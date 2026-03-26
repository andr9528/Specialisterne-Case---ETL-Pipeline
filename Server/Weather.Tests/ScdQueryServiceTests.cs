using FluentAssertions;
using Weather.Abstraction.Enum;
using Weather.Model.ComplexSearchable;
using Weather.Model.Entity;
using Weather.Model.Searchable;
using Weather.Tests.Core;
using Weather.Tests.Core.SystemUnderTests;

namespace Weather.Tests
{
    public class ScdQueryServiceTests
    {
        public class GetAllEntities : BaseDatabaseTest
        {
            [Test]
            public async Task ReturnsAllStoredEntities()
            {
                // Arrange
                using ScdQueryServiceSut sut = this.CreateScdQueryServiceSut();

                await sut.Factory.AddScd(1);
                await sut.Factory.AddScd(2);
                await sut.Factory.AddScd(3);

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
                using ScdQueryServiceSut sut = this.CreateScdQueryServiceSut();

                Scd created = await sut.Factory.AddScd(1);

                var searchable = new SearchableScd
                {
                    Id = created.Id,
                };

                // Act
                Scd? result = await sut.Service.GetEntity(searchable);

                // Assert
                result.Should().NotBeNull();
                result!.Id.Should().Be(created.Id);
            }

            [Test]
            public async Task WithNoMatch_ReturnsNull()
            {
                // Arrange
                using ScdQueryServiceSut sut = this.CreateScdQueryServiceSut();

                var searchable = new SearchableScd
                {
                    Id = 999999,
                };

                // Act
                Scd? result = await sut.Service.GetEntity(searchable);

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
                using ScdQueryServiceSut sut = this.CreateScdQueryServiceSut();

                var matchingReaderId = Guid.NewGuid();

                await sut.Factory.AddScd(1, matchingReaderId);
                await sut.Factory.AddScd(2, matchingReaderId);
                await sut.Factory.AddScd(3, Guid.NewGuid());

                var searchable = new SearchableScd
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
            public async Task WithCarbonDioxide_ReturnsOnlyMatchingEntities()
            {
                // Arrange
                using ScdQueryServiceSut sut = this.CreateScdQueryServiceSut();

                await sut.Factory.AddScd(1, carbonDioxide: 500);
                await sut.Factory.AddScd(2, carbonDioxide: 500);
                await sut.Factory.AddScd(3, carbonDioxide: 900);

                var searchable = new SearchableScd
                {
                    CarbonDioxide = 500,
                };

                // Act
                var result = (await sut.Service.GetEntities(searchable)).ToList();

                // Assert
                result.Should().HaveCount(2);
                result.Should().OnlyContain(x => x.CarbonDioxide == 500);
            }
        }

        public class GetEntityComplex : BaseDatabaseTest
        {
            [Test]
            public async Task ReturnsFirstMatchingEntity()
            {
                // Arrange
                using ScdQueryServiceSut sut = this.CreateScdQueryServiceSut();

                var readerId = Guid.NewGuid();

                await sut.Factory.AddScd(1, readerId);
                await sut.Factory.AddScd(2, readerId);

                var complex = new ComplexSearchableScd
                {
                    Searchable = new SearchableScd
                    {
                        ReaderId = readerId,
                    },
                };

                // Act
                Scd? result = await sut.Service.GetEntityComplex(complex);

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
                using ScdQueryServiceSut sut = this.CreateScdQueryServiceSut();
                DateTime now = DateTime.UtcNow;
                DateTime threshold = now.AddDays(-2);

                await sut.Factory.AddScd(1, observedAt: now.AddDays(-4));
                await sut.Factory.AddScd(2, observedAt: now.AddDays(-1));
                await sut.Factory.AddScd(3, observedAt: now);

                var complex = new ComplexSearchableScd
                {
                    Searchable = new SearchableScd(),
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
                using ScdQueryServiceSut sut = this.CreateScdQueryServiceSut();
                DateTime now = DateTime.UtcNow;
                DateTime threshold = now.AddDays(-2);

                await sut.Factory.AddScd(1, observedAt: now.AddDays(-5));
                await sut.Factory.AddScd(2, observedAt: now.AddDays(-3));
                await sut.Factory.AddScd(3, observedAt: now.AddDays(-1));

                var complex = new ComplexSearchableScd
                {
                    Searchable = new SearchableScd(),
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
                using ScdQueryServiceSut sut = this.CreateScdQueryServiceSut();
                DateTime now = DateTime.UtcNow;
                DateTime threshold = now.AddDays(-2);

                await sut.Factory.AddScd(1, pulledAt: now.AddDays(-5));
                await sut.Factory.AddScd(2, pulledAt: now.AddDays(-1));
                await sut.Factory.AddScd(3, pulledAt: now);

                var complex = new ComplexSearchableScd
                {
                    Searchable = new SearchableScd(),
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
                using ScdQueryServiceSut sut = this.CreateScdQueryServiceSut();
                DateTime now = DateTime.UtcNow;
                DateTime threshold = now.AddDays(-2);

                await sut.Factory.AddScd(1, pulledAt: now.AddDays(-5));
                await sut.Factory.AddScd(2, pulledAt: now.AddDays(-3));
                await sut.Factory.AddScd(3, pulledAt: now.AddDays(-1));

                var complex = new ComplexSearchableScd
                {
                    Searchable = new SearchableScd(),
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
                using ScdQueryServiceSut sut = this.CreateScdQueryServiceSut();
                DateTime now = DateTime.UtcNow;

                await sut.Factory.AddScd(1, observedAt: now.AddDays(-10));
                await sut.Factory.AddScd(2, observedAt: now.AddDays(-3));
                await sut.Factory.AddScd(3, observedAt: now.AddDays(-1));

                var complex = new ComplexSearchableScd
                {
                    Searchable = new SearchableScd(),
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
                using ScdQueryServiceSut sut = this.CreateScdQueryServiceSut();
                DateTime now = DateTime.UtcNow;

                await sut.Factory.AddScd(1, pulledAt: now.AddDays(-10));
                await sut.Factory.AddScd(2, pulledAt: now.AddDays(-3));
                await sut.Factory.AddScd(3, pulledAt: now.AddDays(-1));

                var complex = new ComplexSearchableScd
                {
                    Searchable = new SearchableScd(),
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
                using ScdQueryServiceSut sut = this.CreateScdQueryServiceSut();
                DateTime now = DateTime.UtcNow;

                Scd third = await sut.Factory.AddScd(1, observedAt: now.AddHours(3));
                Scd first = await sut.Factory.AddScd(2, observedAt: now.AddHours(1));
                Scd second = await sut.Factory.AddScd(3, observedAt: now.AddHours(2));

                var complex = new ComplexSearchableScd
                {
                    Searchable = new SearchableScd(),
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
                using ScdQueryServiceSut sut = this.CreateScdQueryServiceSut();
                DateTime now = DateTime.UtcNow;

                Scd first = await sut.Factory.AddScd(1, observedAt: now.AddHours(1));
                Scd second = await sut.Factory.AddScd(2, observedAt: now.AddHours(2));
                Scd third = await sut.Factory.AddScd(3, observedAt: now.AddHours(3));

                var complex = new ComplexSearchableScd
                {
                    Searchable = new SearchableScd(),
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
                using ScdQueryServiceSut sut = this.CreateScdQueryServiceSut();
                DateTime now = DateTime.UtcNow;

                Scd third = await sut.Factory.AddScd(1, pulledAt: now.AddHours(3));
                Scd first = await sut.Factory.AddScd(2, pulledAt: now.AddHours(1));
                Scd second = await sut.Factory.AddScd(3, pulledAt: now.AddHours(2));

                var complex = new ComplexSearchableScd
                {
                    Searchable = new SearchableScd(),
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
                using ScdQueryServiceSut sut = this.CreateScdQueryServiceSut();
                DateTime now = DateTime.UtcNow;

                Scd first = await sut.Factory.AddScd(1, pulledAt: now.AddHours(3));
                Scd second = await sut.Factory.AddScd(2, pulledAt: now.AddHours(2));
                Scd third = await sut.Factory.AddScd(3, pulledAt: now.AddHours(1));

                var complex = new ComplexSearchableScd
                {
                    Searchable = new SearchableScd(),
                    OrderByPulledAt = OrderDirection.DESCENDING,
                };

                // Act
                var result = (await sut.Service.GetEntitiesComplex(complex)).ToList();

                // Assert
                result.Select(x => x.Id).Should().ContainInOrder(first.Id, second.Id, third.Id);
            }

            [Test]
            public async Task
                WithReaderIdAndPulledAtBeforeAndOrderByPulledAtDescending_ReturnsMatchingEntitiesInExpectedOrder()
            {
                // Arrange
                using ScdQueryServiceSut sut = this.CreateScdQueryServiceSut();
                DateTime now = DateTime.UtcNow;
                var readerId = Guid.NewGuid();
                DateTime threshold = now.AddHours(-1);

                await sut.Factory.AddScd(1, Guid.NewGuid(), pulledAt: now.AddHours(-2));

                Scd second = await sut.Factory.AddScd(2, readerId, pulledAt: now.AddHours(-3));
                Scd first = await sut.Factory.AddScd(3, readerId, pulledAt: now.AddHours(-2));

                await sut.Factory.AddScd(4, readerId, pulledAt: now);

                var complex = new ComplexSearchableScd
                {
                    Searchable = new SearchableScd
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
                WithCarbonDioxideAndObservedAtAfterAndOrderByObservedAtAscending_ReturnsMatchingEntitiesInExpectedOrder()
            {
                // Arrange
                using ScdQueryServiceSut sut = this.CreateScdQueryServiceSut();
                DateTime now = DateTime.UtcNow;
                DateTime threshold = now.AddHours(-4);

                await sut.Factory.AddScd(1, carbonDioxide: 900, observedAt: now.AddHours(-2));

                Scd first = await sut.Factory.AddScd(2, carbonDioxide: 500, observedAt: now.AddHours(-3));
                Scd second = await sut.Factory.AddScd(3, carbonDioxide: 500, observedAt: now.AddHours(-1));

                await sut.Factory.AddScd(4, carbonDioxide: 500, observedAt: now.AddHours(-5));

                var complex = new ComplexSearchableScd
                {
                    Searchable = new SearchableScd
                    {
                        CarbonDioxide = 500,
                    },
                    ObservedAtAfterThisDateTime = threshold,
                    OrderByObservedAt = OrderDirection.ASCENDING,
                };

                // Act
                var result = (await sut.Service.GetEntitiesComplex(complex)).ToList();

                // Assert
                result.Should().HaveCount(2);
                result.Should().OnlyContain(x => x.CarbonDioxide == 500 && x.ObservedAt > threshold);
                result.Select(x => x.Id).Should().ContainInOrder(first.Id, second.Id);
            }

            [Test]
            public async Task
                WithReaderIdAndLastXDaysObservedAtAndOrderByObservedAtDescending_ReturnsOnlyRecentMatchingEntitiesInExpectedOrder()
            {
                // Arrange
                using ScdQueryServiceSut sut = this.CreateScdQueryServiceSut();
                DateTime now = DateTime.UtcNow;
                var readerId = Guid.NewGuid();

                await sut.Factory.AddScd(1, readerId, observedAt: now.AddDays(-10));

                Scd second = await sut.Factory.AddScd(2, readerId, observedAt: now.AddDays(-3));
                Scd first = await sut.Factory.AddScd(3, readerId, observedAt: now.AddDays(-1));

                await sut.Factory.AddScd(4, Guid.NewGuid(), observedAt: now.AddDays(-2));

                var complex = new ComplexSearchableScd
                {
                    Searchable = new SearchableScd
                    {
                        ReaderId = readerId,
                    },
                    LastXDaysObservedAt = 5,
                    OrderByObservedAt = OrderDirection.DESCENDING,
                };

                // Act
                var result = (await sut.Service.GetEntitiesComplex(complex)).ToList();

                // Assert
                result.Should().HaveCount(2);
                result.Should().OnlyContain(x => x.ReaderId == readerId && x.ObservedAt >= now.AddDays(-5));
                result.Select(x => x.Id).Should().ContainInOrder(first.Id, second.Id);
            }

            [Test]
            public async Task WithReaderIdAndObservedAtRangeAndPulledAtRange_ReturnsOnlyEntitiesMatchingAllCriteria()
            {
                // Arrange
                using ScdQueryServiceSut sut = this.CreateScdQueryServiceSut();
                DateTime now = DateTime.UtcNow;

                var readerId = Guid.NewGuid();
                DateTime observedAfter = now.AddHours(-6);
                DateTime observedBefore = now.AddHours(-2);
                DateTime pulledAfter = now.AddHours(-5);
                DateTime pulledBefore = now.AddHours(-1);

                Scd match = await sut.Factory.AddScd(1, readerId, observedAt: now.AddHours(-4),
                    pulledAt: now.AddHours(-3));

                await sut.Factory.AddScd(2, Guid.NewGuid(), observedAt: now.AddHours(-4), pulledAt: now.AddHours(-3));

                await sut.Factory.AddScd(3, readerId, observedAt: now.AddHours(-7), pulledAt: now.AddHours(-3));

                await sut.Factory.AddScd(4, readerId, observedAt: now.AddHours(-4), pulledAt: now);

                var complex = new ComplexSearchableScd
                {
                    Searchable = new SearchableScd
                    {
                        ReaderId = readerId,
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