namespace Reservas.API.Contracts
{
    public record AuthResponse(string Email, string Password, string Token);
}
