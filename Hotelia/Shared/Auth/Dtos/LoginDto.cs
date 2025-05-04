namespace Hotelia.Shared.Auth.Dtos;

public record LoginDto(string Email, string Password, string AccessToken);