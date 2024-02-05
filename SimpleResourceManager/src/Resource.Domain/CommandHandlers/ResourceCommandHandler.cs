using MediatR;
using Resource.Domain.Repositories;
using Resource.Messages.Commands;

namespace Resource.Domain.CommandHandlers;

public class ResourceCommandHandler:
    IRequestHandler<CreateResourceCommand>,
    IRequestHandler<RetireResourceCommand>,
    IRequestHandler<UnRetireResourceCommand>
{

    private readonly IResourceRepository _resourceRepository;

    public ResourceCommandHandler(IResourceRepository resourceRepository)
    {
        _resourceRepository = resourceRepository;
    }

    public async Task Handle(CreateResourceCommand request, CancellationToken cancellationToken)
    {
        var resource = new Entity.Resource(request.Name, request.Description);

        await _resourceRepository.Create(resource);
    }

    public async Task Handle(RetireResourceCommand request, CancellationToken cancellationToken)
    {
        var resource = await _resourceRepository.Get(request.ResourceId);

        if (resource is null)
            throw new ArgumentNullException(nameof(request.ResourceId),
                $"Resource with id: {request.ResourceId} , does not exist");

        resource.RetireResource();
        await _resourceRepository.Save(resource);
    }

    public async Task Handle(UnRetireResourceCommand request, CancellationToken cancellationToken)
    {
        var resource = await _resourceRepository.Get(request.ResourceId);

        if (resource is null)
            throw new ArgumentNullException(nameof(request.ResourceId),
                $"Resource with id: {request.ResourceId} , does not exist");

        resource.UnRetireResource();
        await _resourceRepository.Save(resource);
    }
}