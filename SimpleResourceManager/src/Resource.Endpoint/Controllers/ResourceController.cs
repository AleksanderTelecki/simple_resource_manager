using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resource.Messages.Commands;
using Resource.Messages.Models;
using Resource.Messages.Queries;

[ApiController]
[Route("[controller]")]
public class ResourceController : ControllerBase
{
    private readonly ILogger<ResourceController> _logger;
    private readonly IMediator _mediator;
    
    public ResourceController(ILogger<ResourceController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [Route("Create")]
    public async Task<IActionResult> CreateResource(CreateResourceCommand command)
    {
        try
        {
            await _mediator.Send(command);
            return Ok($"Resource {command.Name} was successfully created!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin")]
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
    [Authorize(Roles = "Admin")]
    [Route("Unretire")]
    public async Task<IActionResult> UnRetireResource(UnRetireResourceCommand command)
    {
        try
        {
            await _mediator.Send(command);
            return Ok($"Resource was successfully unretired!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
    
    [HttpGet]
    [Authorize]
    [Route("All")]
    public async Task<IActionResult> GetAllResources()
    {
        try
        {
            var resources = await _mediator.Send(new GetAllResourcesQuery());
            return Ok(resources);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

}