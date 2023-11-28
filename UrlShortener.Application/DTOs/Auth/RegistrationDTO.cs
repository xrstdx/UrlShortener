namespace UrlShortener.Application.DTOs.Auth;

public record RegistrationDTO(
    string FullName,
    string Email,
    string Password);
