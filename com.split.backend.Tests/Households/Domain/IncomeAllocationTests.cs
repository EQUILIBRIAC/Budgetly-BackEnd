using FluentAssertions;
using Xunit;
using System;
using com.split.backend.Households.Domain.Models.Aggregates;
using com.split.backend.Households.Domain.Models.Commands;

namespace com.split.backend.Tests.Households.Domain
{
    public class IncomeAllocationTests
    {
        [Fact]
        public void Constructor_WithCreateIncomeAllocationCommand_ShouldInitializeCorrectly()
        {
            // Arrange
            var expectedUserId = 10L;
            var expectedHouseholdId = "HH-12345";
            var expectedPercentage = 25.5m;

            var command = new CreateIncomeAllocationCommand(
                expectedUserId,
                expectedHouseholdId,
                expectedPercentage
            );

            // Act
            var allocation = new IncomeAllocation(command);

            // Assert
            allocation.Should().NotBeNull();
            allocation.Id.Should().StartWith("IA-");
            allocation.UserId.Should().Be(expectedUserId);
            allocation.HouseholdId.Should().Be(expectedHouseholdId);
            allocation.Percentage.Should().Be(expectedPercentage);
        }

        [Fact]
        public void Update_WithValidCommand_ShouldUpdateProperties()
        {
            // Arrange
            var allocation = new IncomeAllocation(1L, "HH-OLD", 10m);
            var newUserId = 20L;
            var newHouseholdId = "HH-NEW";
            var newPercentage = 50m;

            var updateCommand = new UpdateIncomeAllocationCommand(
                allocation.Id,
                newUserId,
                newHouseholdId,
                newPercentage
            );

            // Act
            allocation.Update(updateCommand);

            // Assert
            allocation.UserId.Should().Be(newUserId);
            allocation.HouseholdId.Should().Be(newHouseholdId);
            allocation.Percentage.Should().Be(newPercentage);
        }

        [Fact]
        public void Constructor_Empty_ShouldHaveDefaultValues()
        {
            // Arrange & Act
            var allocation = new IncomeAllocation();

            // Assert
            allocation.UserId.Should().Be(-1);
            allocation.HouseholdId.Should().BeEmpty();
            allocation.Percentage.Should().Be(0);
            allocation.Id.Should().StartWith("IA-");
        }
    }
}