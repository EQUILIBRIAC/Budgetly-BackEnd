using FluentAssertions;
using Xunit;
using com.split.backend.Contributions.Domain.Model.Aggregates;
using com.split.backend.Contributions.Domain.Model.Commands;
using com.split.backend.Contributions.Domain.Model.ValueObjects;

namespace com.split.backend.Tests.Contributions.Domain
{
    public class ContributionTests
    {
        [Fact]
        public void Constructor_WithValidCommand_ShouldInitializeCorrectly()
        {
            // Arrange
            var expectedBillId = "BG-999";
            var expectedHouseholdId = "HH-001";
            var strategy = EStrategy.Even;
            var deadline = "2026-12-31";
            int gracePeriod = (int)strategy;

            var command = new CreateContributionCommand(
                expectedBillId,
                expectedHouseholdId,
                strategy.ToString(),
                deadline,
                gracePeriod
            );

            // Act
            var contribution = new Contribution(command);

            // Assert
            contribution.Should().NotBeNull();
            contribution.BillId.Should().Be(expectedBillId);
            contribution.HouseholdId.Should().Be(expectedHouseholdId);
            contribution.Strategy.Should().Be(strategy);
        }

        [Fact]
        public void Constructor_ShouldGenerateIdStartingWithCN()
        {
            // Arrange
            var command = new CreateContributionCommand("BG-1", "HH-1", "Even", null, 1);

            // Act
            var contribution = new Contribution(command);

            // Assert
            contribution.Id.Should().StartWith("CN");
        }
    }
}