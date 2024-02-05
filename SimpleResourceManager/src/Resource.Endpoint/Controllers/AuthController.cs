using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resource.Domain.Entity;
using Resource.Endpoint.Abstract;
using Resource.Messages.Models;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }
    
    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login(LoginModel model)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid payload");
            var (status, message) = await _authService.Login(model);
            
            if (status == 0)
                return BadRequest(message);
            
            return Ok(message);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register(RegistrationModel model)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid payload");
            
            var (status, message) = await _authService.Registeration(model, ReservationHolderRoles.User);
            
            if (status == 0)
                return BadRequest(message);
            
            return CreatedAtAction(nameof(Register), model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
    
    [HttpPost]
    [Authorize(Roles ="Admin")]
    [Route("Admin")]
    public async Task<IActionResult> AdminRegister(RegistrationModel model)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid payload");
            var (status, message) = await _authService.Registeration(model, ReservationHolderRoles.Admin);
            if (status == 0)
                return BadRequest(message);
            
            return CreatedAtAction(nameof(Register), model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}