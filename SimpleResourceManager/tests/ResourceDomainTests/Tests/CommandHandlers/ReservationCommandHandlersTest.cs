namespace ResourceDomainTests.Tests.Commands;


[TestFixture]
public class ReservationCommandHandlerTests
{
    private Mock<IResourceRepository> _mockResourceRepository;
    private Mock<IOptions<ReservationOptions>> _mockReservationOptions;
    private Mock<UserManager<ReservationHolder>> _mockUserManager;

    [SetUp]
    public void Setup()
    {
        _mockResourceRepository = new Mock<IResourceRepository>();
        _mockReservationOptions = new Mock<IOptions<ReservationOptions>>();
        _mockUserManager = new Mock<UserManager<ReservationHolder>>(
            Mock.Of<IUserStore<ReservationHolder>>(), null, null, null, null, null, null, null, null);

        // Setup mock reservation options
        _mockReservationOptions.Setup(ro => ro.Value).Returns(new ReservationOptions { ReservationTimeInMinutes = 60 });
    }

    private ReservationCommandHandler CreateHandler()
    {
        return new ReservationCommandHandler(_mockResourceRepository.Object, _mockReservationOptions.Object, _mockUserManager.Object);
    }
    
    [Test]
    public async Task Handle_ReserveResourceTemporarilyCommand_ShouldReserveResource()
    {
        // Arrange
        var handler = CreateHandler();
        var resource = new ResourceBuilder().WithName("Meeting Room").Build();
        var reservationHolder = new ReservationHolderBuilder().WithId("holder-id").Build();
        var command = new ReserveResourceTemporarilyCommand { ResourceId = resource.Id, ReservationHolderId = reservationHolder.Id };

        _mockResourceRepository.Setup(repo => repo.Get(command.ResourceId)).ReturnsAsync(resource);
        _mockUserManager.Setup(um => um.Users).Returns(new List<ReservationHolder> { reservationHolder }.AsQueryable().BuildMock());

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        resource.BlockedUntil.Should().BeCloseTo(DateTime.Now.AddMinutes(60), TimeSpan.FromSeconds(1));
        resource.ReservationHolder.Should().Be(reservationHolder);
    }
    
    [Test]
    public async Task Handle_ReserveResourceCommand_ShouldPermanentlyReserveResource()
    {
        // Arrange
        var handler = CreateHandler();
        var resource = new ResourceBuilder().WithName("Conference Room").Build();
        var reservationHolder = new ReservationHolderBuilder().WithId("holder-id").Build();
        var command = new ReserveResourceCommand { ResourceId = resource.Id, ReservationHolderId = reservationHolder.Id };

        _mockResourceRepository.Setup(repo => repo.Get(command.ResourceId)).ReturnsAsync(resource);
        _mockUserManager.Setup(um => um.Users).Returns(new List<ReservationHolder> { reservationHolder }.AsQueryable().BuildMock());

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        resource.IsBlockedPermanently.Should().BeTrue();
        resource.ReservationHolder.Should().BeEquivalentTo(reservationHolder);
        _mockResourceRepository.Verify(repo => repo.Save(resource), Times.Once);
    }
    
    [Test]
    public async Task Handle_CancelReservationCommand_ShouldCancelReservation()
    {
        // Arrange
        var handler = CreateHandler();
        var reservationHolder = new ReservationHolderBuilder().WithId("holder-id").Build();
        var resource = new ResourceBuilder()
            .WithName("Library Room")
            .WithReservationHolder(reservationHolder)
            .IsBlockedPermanently(true)
            .Build();
        
        var command = new CancelReservationCommand { ResourceId = resource.Id, ReservationHolderId = reservationHolder.Id };

        _mockResourceRepository.Setup(repo => repo.Get(command.ResourceId)).ReturnsAsync(resource);
        _mockUserManager.Setup(um => um.Users).Returns(new List<ReservationHolder> { reservationHolder }.AsQueryable().BuildMock());

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        resource.IsBlockedPermanently.Should().BeFalse();
        resource.ReservationHolder.Should().BeNull();
        _mockResourceRepository.Verify(repo => repo.Save(resource), Times.Once);
    }


}
