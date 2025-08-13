namespace CrmProject.Application.Dtos.AuthDtos
{
    public class RegisterRequestDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string? PhoneNumber { get; set; } // opsiyonel
    }
}
