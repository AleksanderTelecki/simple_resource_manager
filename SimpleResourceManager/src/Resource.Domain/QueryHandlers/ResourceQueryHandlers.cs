using MediatR;
using Resource.Domain.Repositories;
using Resource.Messages.Models;
using Resource.Messages.Queries;

namespace Resource.Domain.QueryHandlers;

public class ResourceQueryHandlers:
    IRequestHandler<GetAllResourcesQuery, List<ResourceModel>>
{
    
    private readonly IResourceRepository _resourceRepository;

    public ResourceQueryHandlers(IResourceRepository resourceRepository)
    {
        _resourceRepository = resourceRepository;
    }

    public async Task<List<ResourceModel>> Handle(GetAllResourcesQuery request, CancellationToken cancellationToken)
    {
        var resources = await _resourceRepository.GetResources();
        return resources.Select(x => new ResourceModel(x.Id, x.Name, x.Description)).ToList();
    }
}