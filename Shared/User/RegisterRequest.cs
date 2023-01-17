using System.ComponentModel.DataAnnotations;

namespace TakraonlineCRM.Shared.User
{
    public class RegisterRequest
    {
        [Required( ErrorMessage = "กรุณาระบุบัญชีผู้ใช้" )]
        public string UserName { get; set; }

        [Required( ErrorMessage = "กรุณาระบุชื่อผู้ใช้" )]
        public string DisplayName { get; set; }
        [Required( ErrorMessage = "กรุณาระบุอีเมล" )]
        [DataType( DataType.EmailAddress )]
        [EmailAddress( ErrorMessage = "รูปแบบอีเมลไม่ถูกต้อง" )]
        public string Email { get; set; }
        [Required( ErrorMessage = "กรุณาระบุ Role" )]
        public string Role { get; set; }

    }

    public class UserRequest : RegisterRequest
    {
        public string Id { get; set; }
        [Required( ErrorMessage = "กรุณาระบุ Password" )]
        public string Password { get; set; }
        [Required( ErrorMessage = "กรุณาระบุ PasswordConfirm" )]
        [Compare( nameof( Password ), ErrorMessage = "Passwords ไม่ตรงกัน" )]
        public string PasswordConfirm { get; set; }
    }

    public class UserEditRequest : RegisterRequest
    {
        public string Id { get; set; }
        public UserEditRequest() { }
        public UserEditRequest( User user )
        {
            DisplayName = user.DisplayName;
            UserName = user.UserName;
            Email = user.Email;
            Role = user.Role;
            Id = user.Id;
        }

        public string NewPassword { get; set; }
        [Compare( nameof( NewPassword ), ErrorMessage = "Passwords ไม่ตรงกัน" )]
        public string NewPasswordConfirm { get; set; }
    }
}
