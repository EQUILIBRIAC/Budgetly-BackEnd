using com.split.backend.Households.Application.CommandServices;
using com.split.backend.Households.Domain.Models.Aggregates;
using com.split.backend.Households.Domain.Models.Commands;
using com.split.backend.Households.Infrastructure.Persistence.EFC.Repositories;
using com.split.backend.Households.Domain.Repositories;
using com.split.backend.Shared.Infrastructure.Persistence.EFC.Configuration;
using com.split.backend.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;
using Xunit;

namespace com.split.backend.Tests.Households.Application
{
    public class IncomeAllocationIntegrationTests
    {
        [Fact]
        public async Task HandleCreateIncomeAllocation_ShouldPersistCorrectly()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            context.Database.EnsureCreated();

            var repository = new IncomeAllocationRepository(context);
            var unitOfWork = new UnitOfWork(context);
            var householdRepo = new Mock<IHouseHoldRepository>();
            var logger = new Mock<ILogger<IncomeAllocationCommandService>>();

            householdRepo.Setup(r => r.ExistsById("HH-123")).Returns(true);

            var service = new IncomeAllocationCommandService(repository, householdRepo.Object, unitOfWork, logger.Object);
            var command = new CreateIncomeAllocationCommand(1L, "HH-123", 25.0m);

            var result = await service.Handle(command);

            result.Should().NotBeNull();
            var allocationInDb = await context.Set<IncomeAllocation>().FirstOrDefaultAsync(a => a.HouseholdId == "HH-123");
            allocationInDb.Should().NotBeNull();
        }
    }
}