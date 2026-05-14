using FluentAssertions;
using Xunit;
using com.split.backend.MemberContributions.Domain.Model.Aggregates;
using com.split.backend.MemberContributions.Domain.Model.Commands;
using com.split.backend.MemberContributions.Domain.Model.ValueObjects;

namespace com.split.backend.Tests.MemberContributions.Domain
{
    public class MemberContributionTests
    {
        [Fact]
        public void Constructor_WithValidCommand_ShouldInitializeCorrectly()
        {
            // Arrange
            var expectedContributionId = "BG-789";
            var expectedMemberId = "MB-001";
            var expectedAmount = 45.50m;

            var command = new CreateMemberContributionCommand(
                expectedContributionId,
                expectedMemberId,
                expectedAmount
            );

            // Act
            var contribution = new MemberContribution(command);

            // Assert
            contribution.Should().NotBeNull();
            contribution.MemberId.Should().Be(expectedMemberId);
            contribution.ContributionId.Should().Be(expectedContributionId);
            contribution.Amount.Should().Be(expectedAmount);
            contribution.Status.Should().Be(EStatus.Pending);
        }

        [Fact]
        public void Constructor_WithNegativeAmount_ShouldKeepAmountAsDefined()
        {
            // Arrange
            var command = new CreateMemberContributionCommand("BG-123", "MB-001", -10.0m);

            // Act
            var contribution = new MemberContribution(command);

            // Assert
            contribution.Amount.Should().Be(-10.0m);
        }

        [Fact]
        public void Constructor_ShouldGenerateIdStartingWithMC()
        {
            // Arrange
            var command = new CreateMemberContributionCommand("BG-999", "MB-002", 100m);

            // Act
            var contribution = new MemberContribution(command);

            // Assert
            contribution.Id.Should().StartWith("MC");
        }
    }
}