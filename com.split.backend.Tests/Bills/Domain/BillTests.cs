using FluentAssertions;
using Xunit;
using System;
using com.split.backend.Bills.Domain.Models.Aggregates;
using com.split.backend.Bills.Domain.Models.Commands;

namespace com.split.backend.Tests.Bills.Domain
{
    public class BillTests
    {
        // Prueba 1: Validamos que el constructor,, usando el Command, asigne todo correctamente
        [Fact]
        public void Constructor_WithCreateBillCommand_ShouldInitializePropertiesCorrectly()
        {
            // Arrange
            var expectedHouseholdId = "HH-12345";
            var expectedDescription = "Recibo de Electricidad";
            var expectedAmount = 150.75m;
            var expectedCreatedBy = 1L;
            var expectedPaymentDate = "2026-05-15";

            var command = new CreateBillCommand(
                expectedHouseholdId,
                expectedDescription,
                expectedAmount,
                expectedCreatedBy,
                expectedPaymentDate
            );

            // Act
            var bill = new Bill(command);

            // Assert
            bill.Should().NotBeNull();
            bill.Id.Should().StartWith("BG");
            bill.HouseholdId.Should().Be(expectedHouseholdId);
            bill.Description.Should().Be(expectedDescription);
            bill.Amount.Should().Be(expectedAmount);
            bill.CreatedBy.Should().Be(expectedCreatedBy);
            bill.PaymentDate.Should().Be(DateTime.Parse(expectedPaymentDate));
        }

        // Prueba 2: Validamos el otro constructor
        [Fact]
        public void Constructor_WithDirectParameters_ShouldInitializePropertiesCorrectly()
        {
            // Arrange
            var householdId = "HH-98765";
            var description = "Recibo de Agua";
            var amount = 45.0m;
            var createdBy = 2L;
            var paymentDate = "2026-06-01";

            // Act
            var bill = new Bill(householdId, description, amount, createdBy, paymentDate);

            // Assert
            bill.Should().NotBeNull();
            bill.Id.Should().StartWith("BG");
            bill.HouseholdId.Should().Be(householdId);
            bill.Description.Should().Be(description);
            bill.Amount.Should().Be(amount);
            bill.CreatedBy.Should().Be(createdBy);
            bill.CreatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }
    }
}