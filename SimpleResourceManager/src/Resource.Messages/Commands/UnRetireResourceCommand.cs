using MediatR;

namespace Resource.Messages.Commands;

public class UnRetireResourceCommand : IRequest
{
    public Guid ResourceId { get; set; }
}