using com.split.backend.Households.Application.CommandServices;
using com.split.backend.Households.Domain.Models.Aggregates;
using com.split.backend.Households.Domain.Models.Commands;
using com.split.backend.Households.Infrastructure.Persistence.EFC.Repositories;
using com.split.backend.Shared.Infrastructure.Persistence.EFC.Configuration;
using com.split.backend.Shared.Infrastructure.Persistence.EFC.Repositories;
using com.split.backend.IAM.Domain.Repositories;
using com.split.backend.IAM.Domain.Model.Aggregates;
using com.split.backend.HouseholdMembers.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using Xunit;
using Moq;

namespace com.split.backend.Tests.Households.Application
{
    public class HouseHoldIntegrationTests
    {
        [Fact]
        public async Task HandleCreateHousehold_ShouldPersistInDatabase()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDb_Households")
                .Options;

            using var context = new AppDbContext(options);
            context.Database.EnsureCreated();

            var repository = new HouseHoldRepository(context);
            var unitOfWork = new UnitOfWork(context);

            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(r => r.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new User { Id = 1 });

            var memberRepository = new Mock<IHouseholdMemberRepository>();

            var service = new HouseHoldCommandService(
                repository,
                userRepository.Object,
                memberRepository.Object,
                unitOfWork
            );

            var householdName = "Hogar de Integracion Test";

            var command = new CreateHouseholdCommand(
                Id: null,
                Name: householdName,
                RepresentativeId: 1L,
                Currency: "USD",
                Description: "Test",
                MemberCount: 2,
                StartDate: DateTime.UtcNow,
                CreatedAt: null,
                UpdatedAt: null
            );

            var result = await service.Handle(command);

            result.Should().NotBeNull();

            var household = await context.Set<HouseHold>()
                .FirstOrDefaultAsync(h => h.Name == householdName);

            household.Should().NotBeNull();
            household.Name.Should().Be(householdName);
        }
    }
}