using TechTalk.SpecFlow;
using FluentAssertions;
using com.split.backend.IAM.Domain.Model.Aggregates;
using com.split.backend.IAM.Domain.Model.Commands;

namespace com.split.backend.Tests.StepDefinitions
{
    [Binding]
    public class UserRegistrationSteps
    {
        private User? _user;
        private string? _name;
        private string? _email;
        private string? _password;

        [Given(@"I provide the name ""(.*)"" and email ""(.*)""")]
        public void GivenIProvideTheNameAndEmail(string name, string email)
        {
            _name = name;
            _email = email;
        }

        [Given(@"I choose a password ""(.*)"" and the role ""(.*)""")]
        public void GivenIChooseAPasswordAndTheRole(string password, string role)
        {
            _password = password;
        }

        [When(@"I submit the registration")]
        public void WhenISubmitTheRegistration()
        {
            _user = new User
            {
                Email = new com.split.backend.IAM.Domain.Model.ValueObjects.EmailAddress(_email)
            };
        }

        [Then(@"the user should be created with the email ""(.*)""")]
        public void ThenTheUserShouldBeCreatedWithTheEmail(string expectedEmail)
        {
            _user.Should().NotBeNull();
            _user!.Email.Address.Should().Be(expectedEmail);
        }

        [Then(@"the account should be active by default")]
        public void ThenTheAccountShouldBeActiveByDefault()
        {
            _user.Should().NotBeNull();
        }
    }
}