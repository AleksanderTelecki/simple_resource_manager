using MediatR;

namespace Resource.Messages.Commands;

public class CreateResourceCommand: IRequest
{
    public string Name { get; set; }
    
    public string Description { get; set; }
}