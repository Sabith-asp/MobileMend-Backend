namespace MobileMend.Domain.Entities
{
    public class User
    {
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public string PasswordHash { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsBlocked { get; set; }

        public string RefreshToken { get; set; }
        public bool EmailConfirmed { get; set; }
        public string EmailVerificationToken { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }


    }
}
