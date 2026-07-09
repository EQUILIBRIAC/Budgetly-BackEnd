using com.split.backend.Bills.Application.Internal.CommandServices;
using com.split.backend.Bills.Domain.Models.Aggregates;
using com.split.backend.Bills.Domain.Models.Commands;
using com.split.backend.Bills.Infrastructure.Persistence.EFC.Repositories;
using com.split.backend.Shared.Infrastructure.Persistence.EFC.Configuration;
using com.split.backend.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using FluentAssertions;
using Xunit;
using Cortex.Mediator;

namespace com.split.backend.Tests.Bills.Application
{
    public class BillIntegrationTests
    {
        [Fact]
        public async Task HandleCreateBill_ShouldSaveToDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            context.Database.EnsureCreated();

            var repository = new BillRepository(context);

            var unitOfWork = new UnitOfWork(context);
            var mediator = new Mock<IMediator>();

            var service = new BillCommandService(repository, unitOfWork, mediator.Object);

            var billDescription = "Servicio de Luz Test";
            var command = new CreateBillCommand("HH-99", billDescription, 150.0m, 1L, "2026-07-01");

            // Act
            var result = await service.Handle(command);

            // Assert
            result.Should().NotBeNull();

            var billInDb = await context.Set<Bill>().FirstOrDefaultAsync(b => b.Description == billDescription);
            billInDb.Should().NotBeNull();
            billInDb.Amount.Should().Be(150.0m);
        }
    }
}