namespace Unit_Tests.Authentication.V1.AuthServiceSetup;

public class AuthServiceTestCases
{
    private const string ExpectedToken = "SampleToken";
    private const string ExpectedRefreshToken = "SampleRefreshToken";
    
    public static IEnumerable<object[]> ValidCredentialsTestCases()
    {
        yield return new object[] { new TestCase("validemail1@gmail.com", "Valid Password1", ExpectedToken, ExpectedRefreshToken) };
        yield return new object[] { new TestCase("Validemail2@gmail.com", "Valid Password2", ExpectedToken, ExpectedRefreshToken) };
    }
    
    public class TestCase
    {
        public string Email { get; }
        public string Password { get; }
        public string ExpectedToken { get; }
        public string ExpectedRefreshToken { get; }

        public TestCase(string email, string password, string expectedToken, string expectedRefreshToken)
        {
            Email = email;
            Password = password;
            ExpectedToken = expectedToken;
            ExpectedRefreshToken = expectedRefreshToken;
        }
    }
}