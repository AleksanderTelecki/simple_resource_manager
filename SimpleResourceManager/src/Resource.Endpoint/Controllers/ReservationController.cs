using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resource.Messages.Commands;
using Resource.Messages.Models;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ReservationController : ControllerBase
{
    private readonly ILogger<ReservationController> _logger;
    private readonly IMediator _mediator;
    
    public ReservationController(ILogger<ReservationController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost]
    [Route("ReserveTemp")]
    public async Task<IActionResult> ReserveResourceTemporarily(ReserveResourceTemporarilyCommand command)
    {
        try
        {
            await _mediator.Send(command);
            return Ok($"Resource was successfully reserved!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
    
    [HttpPost]
    [Route("Reserve")]
    public async Task<IActionResult> ReserveResource(ReserveResourceCommand command)
    {
        try
        {
            await _mediator.Send(command);
            return Ok($"Resource was successfully reserved!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
    
    [HttpPost]
    [Route("Retire")]
    public async Task<IActionResult> RetireResource(RetireResourceCommand command)
    {
        try
        {
            await _mediator.Send(command);
            return Ok($"Resource was successfully retired!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
    
    [HttpPost]
    [Route("Cancel")]
    public async Task<IActionResult> CancelReservation(CancelReservationCommand command)
    {
        try
        {
            await _mediator.Send(command);
            return Ok($"Resource was successfully cancelled!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}