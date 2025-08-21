namespace SmartTaskPro.DTOs;

public record RegisterRequest(string Email, string Password, string? FullName);
public record LoginRequest(string Email, string Password);
public record AuthResponse(string UserId, string Email, string Token);

