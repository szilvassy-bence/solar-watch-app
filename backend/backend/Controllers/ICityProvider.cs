namespace backend.Controllers;

public interface ICityProvider
{
    Task<string> GetCity(string city);
}