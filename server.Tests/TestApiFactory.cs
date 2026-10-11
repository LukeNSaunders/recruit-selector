using Microsoft.AspNetCore.Mvc.Testing;

namespace Server.Tests;

// Starts the real API in memory for the tests, with test-only login details,
// so the tests never depend on anyone's personal user secrets.
public class TestApiFactory : WebApplicationFactory<Program>
{
    public const string Username = "test-user";
    public const string Password = "test-password";

    public TestApiFactory()
    {
        // Environment variables override user secrets, so the tests always use these.
        // (The double underscore means the same as the colon in "Jwt:Key".)
        Environment.SetEnvironmentVariable("Jwt__Key", "test-signing-key-that-is-at-least-32-chars");
        Environment.SetEnvironmentVariable("Auth__Username", Username);
        Environment.SetEnvironmentVariable("Auth__Password", Password);
    }
}