using com.split.backend.Invitations.Application.Internal.CommandServices;
using com.split.backend.Invitations.Domain.Models.Aggregates;
using com.split.backend.Invitations.Domain.Models.Commands;
using com.split.backend.Invitations.Infrastructure.Persistence.EFC.Repositories;
using com.split.backend.Households.Domain.Models.Aggregates;
using com.split.backend.Households.Domain.Models.ValueObjects;
using com.split.backend.Households.Infrastructure.Persistence.EFC.Repositories;
using com.split.backend.HouseholdMembers.Infrastructure.Persistence.EFC.Repositories;
using com.split.backend.Shared.Infrastructure.Persistence.EFC.Configuration;
using com.split.backend.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using Xunit;

namespace com.split.backend.Tests.Invitations.Application
{
    public class InvitationIntegrationTests
    {
        [Fact]
        public async Task HandleCreateInvitation_ShouldSaveToDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            context.Database.EnsureCreated();

            var existingHousehold = new HouseHold
            {
                Id = "HH-555",
                Name = "Test Household",
                MemberCount = 5,
                Currency = ECurrency.USD
            };

            context.Set<HouseHold>().Add(existingHousehold);
            await context.SaveChangesAsync();

            var invitationRepo = new InvitationRepository(context);
            var householdRepo = new HouseHoldRepository(context);
            var memberRepo = new HouseholdMemberRepository(context);
            var unitOfWork = new UnitOfWork(context);

            var service = new InvitationCommandService(
                invitationRepo,
                householdRepo,
                memberRepo,
                unitOfWork);

            var command = new CreateInvitationCommand("guest@example.com", "HH-555", "Bienvenido");

            // Act
            var result = await service.Handle(command);

            // Assert
            result.Should().NotBeNull();

            var invitationInDb = await context.Invitations
                .FirstOrDefaultAsync(i => i.Email == "guest@example.com");

            invitationInDb.Should().NotBeNull();
        }
    }
}