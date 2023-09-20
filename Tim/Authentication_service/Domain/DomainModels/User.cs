namespace Domain.DomainModels
{
    public class User
    {
        public string Id { get; set; } // The unique identifier for this user.
        public string Email { get; set; } // The user's email address.
        public string GoogleId { get; set; } // The user's Google ID, if they have logged in with Google.
        public string HashedPassword { get; set; } // The user's hashed password.
        public string RefreshToken { get; set; } // The refresh token for this user.
        public DateTime RefreshTokenExpiryTime { get; set; } // The expiry time for the refresh token.
    }
}