using MediatR;

namespace Resource.Messages.Commands;

public class RetireResourceCommand: IRequest
{
    public Guid ResourceId { get; set; }
}