using System.ComponentModel.DataAnnotations;

namespace Resource.Messages.Models;

public class RegistrationModel
{
    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
}