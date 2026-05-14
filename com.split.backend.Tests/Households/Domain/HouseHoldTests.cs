using System;
using FluentAssertions;
using Xunit;
using com.split.backend.Households.Domain.Models.Aggregates;
using com.split.backend.Households.Domain.Models.Commands;
using com.split.backend.Households.Domain.Models.ValueObjects;

namespace com.split.backend.Tests.Households.Domain
{
    public class HouseHoldTests
    {
        [Fact]
        public void Constructor_WithValidCommand_ShouldInitializeCorrectly()
        {
            // Arrange
            var command = new CreateHouseholdCommand(
                Id: null,
                Name: "Familia Pérez",
                RepresentativeId: 1L,
                Currency: "PEN",
                Description: "Casa principal",
                MemberCount: 4,
                StartDate: DateTime.UtcNow,
                CreatedAt: null,
                UpdatedAt: null
            );

            // Act
            var houseHold = new HouseHold(command);

            // Assert
            houseHold.Should().NotBeNull();
            houseHold.Id.Should().StartWith("HH");
            houseHold.Name.Should().Be("Familia Pérez");
            houseHold.MemberCount.Should().Be(4);
        }

        [Fact]
        public void Constructor_WithNegativeOrZeroMembers_ShouldDefaultToOne()
        {
            // Arrange
            var command = new CreateHouseholdCommand(
                Id: null, Name: "Casa Sola", RepresentativeId: 1L, Currency: "USD",
                Description: "Test", MemberCount: 0,
                StartDate: DateTime.UtcNow, CreatedAt: null, UpdatedAt: null
            );

            // Act
            var houseHold = new HouseHold(command);

            // Assert
            houseHold.MemberCount.Should().Be(1);
        }

        [Fact]
        public void Constructor_WithInvalidCurrency_ShouldDefaultToUSD()
        {
            // Arrange
            var command = new CreateHouseholdCommand(
                Id: null, Name: "Casa Internacional", RepresentativeId: 1L,
                Currency: "MONEDA_INVENTADA",
                Description: "Test", MemberCount: 2,
                StartDate: DateTime.UtcNow, CreatedAt: null, UpdatedAt: null
            );

            // Act
            var houseHold = new HouseHold(command);

            // Assert
            houseHold.Currency.Should().Be(ECurrency.USD);
        }
    }
}