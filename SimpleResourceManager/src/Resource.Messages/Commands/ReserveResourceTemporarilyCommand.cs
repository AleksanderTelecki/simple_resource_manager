using MediatR;

namespace Resource.Messages.Commands;

public class ReserveResourceTemporarilyCommand: IRequest
{
    public Guid ResourceId { get; set; }
    public string ReservationHolderId { get; set; }
}