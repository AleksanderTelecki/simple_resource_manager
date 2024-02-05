using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Resource.Domain.Entity;
using Resource.Domain.Options;
using Resource.Domain.Repositories;
using Resource.Messages.Commands;

namespace Resource.Domain.CommandHandlers;

public class ReservationCommandHandler:
    IRequestHandler<ReserveResourceTemporarilyCommand>,
    IRequestHandler<ReserveResourceCommand>,
    IRequestHandler<CancelReservationCommand>
{
    private readonly IResourceRepository _resourceRepository;
    private readonly IOptions<ReservationOptions> _reservationOptions;
    private readonly UserManager<ReservationHolder> _reservationHolderManager;

    public ReservationCommandHandler(IResourceRepository resourceRepository, IOptions<ReservationOptions> reservationOptions, UserManager<ReservationHolder> reservationHolderManager)
    {
        _resourceRepository = resourceRepository;
        _reservationOptions = reservationOptions;
        _reservationHolderManager = reservationHolderManager;
    }

    public async Task Handle(ReserveResourceTemporarilyCommand request, CancellationToken cancellationToken)
    {
        var reservation = await _resourceRepository.Get(request.ResourceId);
        
        if (reservation is null)
            throw new ArgumentNullException($"Resource with id: {request.ResourceId} , does not exist",
                nameof(request.ResourceId));

        var reservationHolder = await _reservationHolderManager.Users.FirstAsync(x => x.Id == request.ReservationHolderId);
        
        reservation.TemporarilyReserve(DateTime.Now.AddMinutes(_reservationOptions.Value.ReservationTimeInMinutes), reservationHolder);

        await _resourceRepository.Save(reservation);
    }

    public async Task Handle(ReserveResourceCommand request, CancellationToken cancellationToken)
    {
        var reservation = await _resourceRepository.Get(request.ResourceId);
        
        if (reservation is null)
            throw new ArgumentNullException($"Resource with id: {request.ResourceId} , does not exist",
                nameof(request.ResourceId));

        var reservationHolder = await _reservationHolderManager.Users.FirstAsync(x => x.Id == request.ReservationHolderId);
        
        reservation.Reserve(reservationHolder);

        await _resourceRepository.Save(reservation);
    }

    public async Task Handle(CancelReservationCommand request, CancellationToken cancellationToken)
    {
        var reservation = await _resourceRepository.Get(request.ResourceId);
        
        if (reservation is null)
            throw new ArgumentNullException($"Resource with id: {request.ResourceId} , does not exist",
                nameof(request.ResourceId));

        var reservationHolder = await _reservationHolderManager.Users.FirstAsync(x => x.Id == request.ReservationHolderId);
        
        reservation.CancelReservation(reservationHolder);

        await _resourceRepository.Save(reservation);
    }
}