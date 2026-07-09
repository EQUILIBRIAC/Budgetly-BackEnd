using com.split.backend.IAM.Application.Internal.CommandServices;
using com.split.backend.IAM.Domain.Model.Aggregates;
using com.split.backend.IAM.Domain.Model.Commands;
using com.split.backend.IAM.Infrastructure.Persistence.EFC.Repositories;
using com.split.backend.IAM.Domain.Repositories;
using com.split.backend.IAM.Application.Internal.OutboundServices;
using com.split.backend.Invitations.Domain.Repositories;
using com.split.backend.Households.Domain.Repositories;
using com.split.backend.HouseholdMembers.Domain.Repositories;
using com.split.backend.Shared.Infrastructure.Persistence.EFC.Configuration;
using com.split.backend.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using FluentAssertions;
using Xunit;

namespace com.split.backend.Tests.IAM.Application
{
    public class UserIntegrationTests
    {
        [Fact]
        public async Task HandleSignUp_ShouldPersistUserInDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            context.Database.EnsureCreated();

            var repository = new UserRepository(context);
            var unitOfWork = new UnitOfWork(context);
            var hashingService = new Mock<IHashingService>();
            var tokenService = new Mock<ITokenService>();
            var invitationRepo = new Mock<IInvitationRepository>();
            var memberRepo = new Mock<IHouseholdMemberRepository>();
            var householdRepo = new Mock<IHouseHoldRepository>();

            hashingService.Setup(h => h.HashPassword(It.IsAny<string>())).Returns("hashed_pass");

            var service = new UserCommandService(
                repository,
                tokenService.Object,
                hashingService.Object,
                invitationRepo.Object,
                memberRepo.Object,
                householdRepo.Object,
                unitOfWork);

            var command = new SignUpCommand(
                "carlos@test.com",
                "Password123!",
                "Carlos Perez",
                "Admin",
                1,
                null);

            // Act
            await service.Handle(command);

            // Assert
            var userInDb = await context.Set<User>()
                .FirstOrDefaultAsync(u => u.Email.Address == "carlos@test.com");

            userInDb.Should().NotBeNull();
        }
    }
}