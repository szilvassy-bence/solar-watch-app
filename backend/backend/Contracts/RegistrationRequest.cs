using System.ComponentModel.DataAnnotations;

namespace backend.Contracts;

public record RegistrationRequest(
    [Required] string Email,
    [Required] string Username,
    [Required] string Password);