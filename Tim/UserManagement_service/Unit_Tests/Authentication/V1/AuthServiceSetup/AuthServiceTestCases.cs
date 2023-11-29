namespace Unit_Tests.Authentication.V1.AuthServiceSetup;

public class AuthServiceTestCases
{
    public const string ExpectedToken = "SampleToken";
    public const string ExpectedRefreshToken = "SampleRefreshToken";

    public static IEnumerable<object[]> ValidCredentialsTestCases()
    {
        yield return new object[] { new TestCase("validemail1@gmail.com", "ValidPassword1", ExpectedToken, ExpectedRefreshToken) };
        yield return new object[] { new TestCase("validemail2@gmail.com", "ValidPassword2", ExpectedToken, ExpectedRefreshToken) };
    }

    public static IEnumerable<object[]> InvalidEmailTestCases()
    {
        yield return new object[] { new TestCase("invalidemail", "ValidPassword1", null, null) };
        yield return new object[] { new TestCase("", "ValidPassword1", null, null) };
        yield return new object[] { new TestCase(null, "ValidPassword1", null, null) };
    }

    public static IEnumerable<object[]> InvalidPasswordTestCases()
    {
        yield return new object[] { new TestCase("validemail1@gmail.com", "short", null, null) };
        yield return new object[] { new TestCase("validemail1@gmail.com", "", null, null) };
        yield return new object[] { new TestCase("validemail1@gmail.com", null, null, null) };
    }

    public static IEnumerable<object[]> NonexistentUserTestCases()
    {
        yield return new object[] { new TestCase("nonexistent@gmail.com", "ValidPassword1", null, null) };
    }

    public static IEnumerable<object[]> IncorrectPasswordTestCases()
    {
        yield return new object[] { new TestCase("existing@gmail.com", "IncorrectPassword", null, null) };
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
