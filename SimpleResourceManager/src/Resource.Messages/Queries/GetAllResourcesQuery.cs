using MediatR;
using Resource.Messages.Models;

namespace Resource.Messages.Queries;

public class GetAllResourcesQuery: IRequest<List<ResourceModel>>
{
    
}