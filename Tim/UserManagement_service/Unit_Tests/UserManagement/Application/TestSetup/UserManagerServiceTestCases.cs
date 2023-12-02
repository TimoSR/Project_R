using Application.DTO.UserManagement;

namespace Unit_Tests.UserManagement.Application.TestSetup;

public class UserManagerServiceTestCases
{
    public static IEnumerable<object[]> RegistrationTestCases()
    {
        // Successful case
        yield return new object[] { new UserRegisterDto { Email = "validemail@example.com", UserName = "validUser", Password = "ValidPassword1" }, true, 1 };

        // Invalid email
        yield return new object[] { new UserRegisterDto { Email = "invalidemail", UserName = "user", Password = "password" }, false, 0 };

        // User already exists
        yield return new object[] { new UserRegisterDto { Email = "existingemail@example.com", UserName = "existingUser", Password = "ExistingPassword" }, false, 0 };

        // Null values
        yield return new object[] { new UserRegisterDto { Email = null, UserName = "user", Password = "password" }, false, 0 };
        yield return new object[] { new UserRegisterDto { Email = "email@example.com", UserName = null, Password = "password" }, false, 0 };
        yield return new object[] { new UserRegisterDto { Email = "email@example.com", UserName = "user", Password = null }, false, 0 };

        // Empty strings
        yield return new object[] { new UserRegisterDto { Email = "", UserName = "user", Password = "password" }, false, 0 };
        yield return new object[] { new UserRegisterDto { Email = "email@example.com", UserName = "", Password = "password" }, false, 0 };
        yield return new object[] { new UserRegisterDto { Email = "email@example.com", UserName = "user", Password = "" }, false, 0 };

        // Excessively large input
        var longString = new string('a', 10000);
        yield return new object[] { new UserRegisterDto { Email = longString, UserName = "user", Password = "password" }, false, 0 };
        yield return new object[] { new UserRegisterDto { Email = "email@example.com", UserName = longString, Password = "password" }, false, 0 };
        yield return new object[] { new UserRegisterDto { Email = "email@example.com", UserName = "user", Password = longString }, false, 0 };
        
        // Simulating negative value in string context
        yield return new object[] { new UserRegisterDto { Email = "negativevalue@example.com", UserName = "-1", Password = "password" }, false, 0 };
        yield return new object[] { new UserRegisterDto { Email = "-1@example.com", UserName = "user", Password = "password" }, false, 0 };

        // Simulating int.MaxValue + 1 in a string context
        yield return new object[] { new UserRegisterDto { Email = $"{int.MaxValue + 1L}@example.com", UserName = "user", Password = "password" }, false, 0 };
        yield return new object[] { new UserRegisterDto { Email = "email@example.com", UserName = $"User{int.MaxValue + 1L}", Password = "password" }, false, 0 };
    }
}