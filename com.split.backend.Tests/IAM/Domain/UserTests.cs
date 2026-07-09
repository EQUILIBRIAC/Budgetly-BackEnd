using System;
using FluentAssertions;
using Xunit;
using com.split.backend.IAM.Domain.Model.Aggregates;
using com.split.backend.IAM.Domain.Model.Commands;
using com.split.backend.IAM.Domain.Model.ValueObjects;

namespace com.split.backend.Tests.IAM.Domain
{
    public class UserTests
    {
        [Fact]
        public void Constructor_WithSignUpCommand_ShouldInitializeCorrectly()
        {
            // Arrange
            var command = new SignUpCommand(
                EmailAddress: "carlos@test.com",
                Password: "PlainPassword123",
                Name: "Carlos Ing",
                Role: "Admin",
                Plan: 2,
                HouseholdId: "HH-123"
            );
            var hashedPassword = "hashed_password_mock";

            // Act
            var user = new User(command, hashedPassword);

            // Assert
            user.Should().NotBeNull();
            user.Email.Address.Should().Be("carlos@test.com");
            user.Password.Should().Be(hashedPassword);
        }

        [Fact]
        public void Constructor_WithEmptyHouseholdId_ShouldAutoGenerateHouseholdId()
        {
            // Arrange
            var command = new SignUpCommand(
                EmailAddress: "nuevo@test.com",
                Password: "pwd",
                Name: "Nuevo",
                Role: "Member",
                Plan: 1,
                HouseholdId: ""
            );

            // Act
            var user = new User(command, "hash");

            // Assert
            user.HouseholdId.Should().StartWith("HH");
        }
    }
}