using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Server.Tests;

public class ApiIntegrationTests : IClassFixture<TestApiFactory>
{
    private readonly HttpClient client;

    // The shape of the login response
    private record LoginResponse(string Token);

    public ApiIntegrationTests(TestApiFactory factory)
    {
        client = factory.CreateClient();
    }

    // Logs in with the test details and attaches the token to every request after this
    private async Task LogInAsync()
    {
        var response = await client.PostAsJsonAsync("/api/auth/login", new
        {
            username = TestApiFactory.Username,
            password = TestApiFactory.Password
        });

        var login = await response.Content.ReadFromJsonAsync<LoginResponse>();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", login!.Token);
    }

    // ---------- Login ----------

    // Correct details should return a token.
    [Fact]
    public async Task Login_CorrectDetails_ReturnsToken()
    {
        var response = await client.PostAsJsonAsync("/api/auth/login", new
        {
            username = TestApiFactory.Username,
            password = TestApiFactory.Password
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var login = await response.Content.ReadFromJsonAsync<LoginResponse>();
        Assert.False(string.IsNullOrWhiteSpace(login?.Token));
    }

    // A wrong password should be refused with a readable message.
    [Fact]
    public async Task Login_WrongPassword_ReturnsUnauthorised()
    {
        var response = await client.PostAsJsonAsync("/api/auth/login", new
        {
            username = TestApiFactory.Username,
            password = "wrong-password"
        });
        var message = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Equal("Incorrect username or password.", message);
    }

    // ---------- Protecting the recruits endpoint ----------

    // Without a token, the API should refuse and tell the user to log in.
    [Fact]
    public async Task Recruits_NoToken_ReturnsUnauthorised()
    {
        var response = await client.GetAsync("/api/recruits?recruits=5&skip=1");
        var message = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Equal("Please log in to continue.", message);
    }

    // A made-up token should be refused.
    [Fact]
    public async Task Recruits_FakeToken_ReturnsUnauthorised()
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "not-a-real-token");

        var response = await client.GetAsync("/api/recruits?recruits=5&skip=1");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    // ---------- Using the recruits endpoint while logged in ----------

    // A valid request should return JSON with the field names the Vue page uses.
    [Fact]
    public async Task Recruits_LoggedInWithValidInput_ReturnsExpectedJson()
    {
        await LogInAsync();

        var response = await client.GetAsync("/api/recruits?recruits=5&skip=1");
        var json = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("\"selectedRecruits\":[2,4,1,5]", json);
        Assert.Contains("\"finalRecruit\":3", json);
    }

    // Bad input should be rejected with 400, even when logged in.
    [Theory]
    [InlineData("/api/recruits?recruits=abc&skip=1")]   
    [InlineData("/api/recruits")]                       
    [InlineData("/api/recruits?recruits=101&skip=1")] 
    public async Task Recruits_LoggedInWithBadInput_ReturnsBadRequest(string url)
    {
        await LogInAsync();

        var response = await client.GetAsync(url);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    // The validation message should reach the user as readable text.
    [Fact]
    public async Task Recruits_LoggedInWithZeroRecruits_ReturnsReadableMessage()
    {
        await LogInAsync();

        var response = await client.GetAsync("/api/recruits?recruits=0&skip=1");
        var message = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("At least one recruit is required.", message);
    }
}