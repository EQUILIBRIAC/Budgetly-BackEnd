using FluentAssertions;
using Xunit;
using com.split.backend.Invitations.Domain.Models.Aggregates;

namespace com.split.backend.Tests.Invitations.Domain
{
    public class InvitationTests
    {
        [Fact]
        public void Invitation_ShouldInitializeWithDefaultValues()
        {
            // Arrange
            var expectedEmail = "test@budgetly.com";
            var expectedHouseholdId = "HH-123";
            var expectedDescription = "Invitación de prueba";

            // Act
            var invitation = new Invitation
            {
                Email = expectedEmail,
                HouseholdId = expectedHouseholdId,
                Description = expectedDescription
            };

            // Assert
            invitation.Should().NotBeNull();
            invitation.Email.Should().Be(expectedEmail);
            invitation.HouseholdId.Should().Be(expectedHouseholdId);
            invitation.Status.Should().Be(InvitationStatus.Pending);
            invitation.Token.Should().NotBeNullOrEmpty();
            invitation.Token.Length.Should().Be(32);
            invitation.ExpiresAt.Should().BeAfter(DateTime.UtcNow);
        }

        [Fact]
        public void Invitation_Token_ShouldBeUniqueForEachInstance()
        {
            // Arrange
            // No se requiere preparación externa para instanciar

            // Act
            var invitation1 = new Invitation();
            var invitation2 = new Invitation();

            // Assert
            invitation1.Token.Should().NotBe(invitation2.Token);
        }
    }
}