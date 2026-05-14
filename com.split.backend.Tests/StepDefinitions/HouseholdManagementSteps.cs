using TechTalk.SpecFlow;
using FluentAssertions;
using com.split.backend.Households.Domain.Models.Aggregates;
using com.split.backend.Households.Domain.Models.Commands;

namespace com.split.backend.Tests.StepDefinitions
{
    [Binding]
    public class HouseholdManagementSteps
    {
        private long _currentUserId;
        private CreateHouseholdCommand _command;
        private HouseHold _household;

        [Given(@"I am a registered user with ID (.*)")]
        public void GivenIAmARegisteredUserWithID(long userId)
        {
            _currentUserId = userId;
        }

        [When(@"I request to create a household named ""(.*)"" with a limit of (.*) members")]
        public void WhenIRequestToCreateAHouseholdNamedWithALimitOfMembers(string name, int limit)
        {
            _command = new CreateHouseholdCommand(
                null,
                name,
                _currentUserId,
                "PEN",
                "Test BDD",
                limit,
                DateTime.UtcNow,
                null,
                null
            );

            _household = new HouseHold(_command);
        }

        [Then(@"the system should generate the household aggregate successfully")]
        public void ThenTheSystemShouldGenerateTheHouseholdAggregateSuccessfully()
        {
            _household.Should().NotBeNull();
        }

        [Then(@"the household name should be ""(.*)""")]
        public void ThenTheHouseholdNameShouldBe(string expectedName)
        {
            _household.Name.Should().Be(expectedName);
        }

        [Then(@"I should be assigned as the representative")]
        public void ThenIShouldBeAssignedAsTheRepresentative()
        {
            _household.RepresentativeId.Should().Be(_currentUserId);
        }
    }
}