namespace ResourceDomainTests.Tests.Commands;

[TestFixture]
public class ResourceCommandHandlerTests
{
    private Mock<IResourceRepository> _mockResourceRepository;
    private ResourceCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _mockResourceRepository = new Mock<IResourceRepository>();
        _handler = new ResourceCommandHandler(_mockResourceRepository.Object);
    }

    [Test]
    public async Task Handle_CreateResourceCommand_ShouldCreateResource()
    {
        // Arrange
        var command = new CreateResourceCommand { Name = "New Resource", Description = "Resource Description" };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockResourceRepository.Verify(repo => repo.Create(It.Is<Resource.Domain.Entity.Resource>(r => r.Name == command.Name && r.Description == command.Description)), Times.Once);
    }

    [Test]
    public async Task Handle_RetireResourceCommand_ResourceExists_ShouldRetireResource()
    {
        // Arrange
        var resourceId = Guid.NewGuid();
        var resource = new ResourceBuilder()
            .WithName("Test Resource")
            .WithDescription("Test Description")
            .Build();
        
        var command = new RetireResourceCommand { ResourceId = resourceId };

        _mockResourceRepository.Setup(repo => repo.Get(resourceId)).ReturnsAsync(resource);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        resource.IsAvailable.Should().BeFalse();
        _mockResourceRepository.Verify(repo => repo.Save(resource), Times.Once);
    }

    [Test]
    public async Task Handle_UnRetireResourceCommand_ResourceExists_ShouldUnRetireResource()
    {
        // Arrange
        var resourceId = Guid.NewGuid();
        var resource = new ResourceBuilder()
            .WithName("Test Resource")
            .WithDescription("Test Description")
            .IsAvailable(false)
            .Build();
        
        var command = new UnRetireResourceCommand { ResourceId = resourceId };

        _mockResourceRepository.Setup(repo => repo.Get(resourceId)).ReturnsAsync(resource);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        resource.IsAvailable.Should().BeTrue();
        _mockResourceRepository.Verify(repo => repo.Save(resource), Times.Once);
    }
}
