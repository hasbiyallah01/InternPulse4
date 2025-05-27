namespace InternPulse4.Models.UserModel;
    public class UserResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
    }

public class LoginResponseModel
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public string RoleName { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}

