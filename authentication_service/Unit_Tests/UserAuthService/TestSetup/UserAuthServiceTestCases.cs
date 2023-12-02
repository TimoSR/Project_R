using _SharedKernel.Patterns.ResultPattern;
using _SharedKernel.Patterns.ResultPattern._Enums;
using Domain._Shared.Events.Subscribed.UserManagement;

namespace Unit_Tests.UserAuthService.TestSetup
{
    public class UserAuthServiceTestCases
    {
        // SetUserAuthDetailsAsync Test Cases
        public static IEnumerable<object[]> SetUserAuthDetailsTestCases()
        {
            // Existing and new edge cases
            yield return new object[] { new SetUserAuthDetailsTestCase(new UserRegInitEvent { Email = "newuser@example.com", Password = "ValidPassword" }, true) };
            yield return new object[] { new SetUserAuthDetailsTestCase(new UserRegInitEvent { Email = "existinguser@example.com", Password = "ValidPassword" }, false) };
            yield return new object[] { new SetUserAuthDetailsTestCase(new UserRegInitEvent { Email = null, Password = "ValidPassword" }, false) };
            yield return new object[] { new SetUserAuthDetailsTestCase(new UserRegInitEvent { Email = "", Password = "ValidPassword" }, false) };
            yield return new object[] { new SetUserAuthDetailsTestCase(new UserRegInitEvent { Email = "newuser@example.com", Password = null }, false) };
            yield return new object[] { new SetUserAuthDetailsTestCase(new UserRegInitEvent { Email = "newuser@example.com", Password = "" }, false) };
            yield return new object[] { new SetUserAuthDetailsTestCase(new UserRegInitEvent { Email = new string('a', 10000), Password = "ValidPassword" }, false) };
        }

        public class SetUserAuthDetailsTestCase
        {
            public UserRegInitEvent UserAuthDetails { get; }
            public bool ExpectedSuccess { get; }

            public SetUserAuthDetailsTestCase(UserRegInitEvent userAuthDetails, bool expectedSuccess)
            {
                UserAuthDetails = userAuthDetails;
                ExpectedSuccess = expectedSuccess;
            }
        }

        // RefreshTokenAsync Test Cases
        public static IEnumerable<object[]> ValidRefreshTokenTestCases()
        {
            yield return new object[] { new RefreshTokenTestCase("validRefreshToken", "userId", "newToken", "newRefreshToken", true) };
        }

        public static IEnumerable<object[]> InvalidRefreshTokenTestCases()
        {
            yield return new object[] { new RefreshTokenTestCase("invalidRefreshToken", null, null, null, false) };
            yield return new object[] { new RefreshTokenTestCase("", null, null, null, false) };
            yield return new object[] { new RefreshTokenTestCase(null, null, null, null, false) };
            yield return new object[] { new RefreshTokenTestCase(new string('a', 10000), null, null, null, false) };
        }

        public class RefreshTokenTestCase
        {
            public string RefreshToken { get; }
            public string UserId { get; }
            public string NewToken { get; }
            public string NewRefreshToken { get; }
            public bool ExpectedSuccess { get; }

            public RefreshTokenTestCase(string refreshToken, string userId, string newToken, string newRefreshToken, bool expectedSuccess)
            {
                RefreshToken = refreshToken;
                UserId = userId;
                NewToken = newToken;
                NewRefreshToken = newRefreshToken;
                ExpectedSuccess = expectedSuccess;
            }
        }

        // LoginAsync Test Cases
        public static IEnumerable<object[]> LoginTestCases()
        {
            
            yield return new object[] {
                new LoginTestCase(
                    "validuser@example.com",
                    "ValidPassword",
                    ServiceResult<(string, string)>.Success(("expectedToken", "expectedRefreshToken"), "Login successful")
                )
            };
            yield return new object[] {
                new LoginTestCase(
                    email: "nonexistent@example.com",
                    password: "ValidPassword",
                    expectedResult: ServiceResult<(string, string)>.Failure("User not found", ServiceErrorType.NotFound)
                )
            };
            yield return new object[] {
                new LoginTestCase(
                    email: "validuser@example.com",
                    password: "InvalidPassword",
                    expectedResult: ServiceResult<(string, string)>.Failure("Authentication failed", ServiceErrorType.Unauthorized)
                )
            };
            //yield return new object[] { new LoginTestCase("invaliduser@example.com", "InvalidPassword", ServiceResult<(string, string)>.Failure("Authentication failed", ServiceErrorType.Unauthorized)) };
            yield return new object[] { new LoginTestCase(null, "ValidPassword", ServiceResult<(string, string)>.Failure("Invalid input", ServiceErrorType.BadRequest)) };
            yield return new object[] { new LoginTestCase("", "ValidPassword", ServiceResult<(string, string)>.Failure("Invalid input", ServiceErrorType.BadRequest)) };
            yield return new object[] { new LoginTestCase("validuser@example.com", null, ServiceResult<(string, string)>.Failure("Invalid input", ServiceErrorType.BadRequest)) };
            yield return new object[] { new LoginTestCase("validuser@example.com", "", ServiceResult<(string, string)>.Failure("Invalid input", ServiceErrorType.BadRequest)) };
            //yield return new object[] { new LoginTestCase(new string('a', 10000), "ValidPassword", ServiceResult<(string, string)>.Failure("Invalid input", ServiceErrorType.BadRequest)) };
        }

        public class LoginTestCase
        {
            public string Email { get; }
            public string Password { get; }
            public ServiceResult<(string, string)> ExpectedResult { get; }

            public LoginTestCase(string email, string password, ServiceResult<(string, string)> expectedResult)
            {
                Email = email;
                Password = password;
                ExpectedResult = expectedResult;
            }
        }

        // DeleteUserAuthDetailsAsync Test Cases
        public static IEnumerable<object[]> DeleteUserAuthDetailsTestCases()
        {
            yield return new object[] { new DeleteUserAuthDetailsTestCase(new UserDeletionInitEvent { Email = "existinguser@example.com" }, true) };
            yield return new object[] { new DeleteUserAuthDetailsTestCase(new UserDeletionInitEvent { Email = "nonexistentuser@example.com" }, false) };
            yield return new object[] { new DeleteUserAuthDetailsTestCase(new UserDeletionInitEvent { Email = null }, false) };
            yield return new object[] { new DeleteUserAuthDetailsTestCase(new UserDeletionInitEvent { Email = "" }, false) };
            yield return new object[] { new DeleteUserAuthDetailsTestCase(new UserDeletionInitEvent { Email = new string('a', 10000) }, false) };
        }

        public class DeleteUserAuthDetailsTestCase
        {
            public UserDeletionInitEvent DeletionEvent { get; }
            public bool ExpectedSuccess { get; }

            public DeleteUserAuthDetailsTestCase(UserDeletionInitEvent deletionEvent, bool expectedSuccess)
            {
                DeletionEvent = deletionEvent;
                ExpectedSuccess = expectedSuccess;
            }
        }
    }
}
