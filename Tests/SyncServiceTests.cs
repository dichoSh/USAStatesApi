using DataAccess;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using RestClients.Interfaces;
using RestClients.Models;
using Services;
using Services.Interfaces;

namespace Tests
{
    public class SyncServiceTests : TestsBase
    {
        private IServiceScope scope;
        private EsriDbContext dbContext;
        private ISyncService syncService;

        private IUSACountiesClient usaCountiesClientMock;
        private ISyncLogService syncLogServiceMock;

        [SetUp]
        public void Setup()
        {
            scope = serviceProvider.CreateScope();
            dbContext = scope.ServiceProvider.GetRequiredService<EsriDbContext>();

            usaCountiesClientMock = Substitute.For<IUSACountiesClient>();
            syncLogServiceMock = Substitute.For<ISyncLogService>();

            syncService = new SyncService(usaCountiesClientMock, new USAStatesService(dbContext, TimeProvider.System), syncLogServiceMock);
        }

        [Test]
        public async Task Test1()
        {
            //Mock
            usaCountiesClientMock.GetCounties(Arg.Any<CancellationToken>())
                 .Returns(
                     [new County { Population = 1, StateName = "test" },
                    new County { Population = 1, StateName = "test" }]);

            //Act
            await syncService.SyncStatesPopulation(CancellationToken.None);

            //Assert
            Assert.That(dbContext.States.Count, Is.EqualTo(1));
            Assert.That(dbContext.States.First().Name, Is.EqualTo("test"));
            Assert.That(dbContext.States.First().Population, Is.EqualTo(2));
        }

        [TearDown]
        public void TearDown()
        {
            scope.Dispose();
            dbContext.Dispose();
        }
    }
}
