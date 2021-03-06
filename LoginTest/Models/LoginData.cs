using System.ComponentModel.DataAnnotations;

namespace LoginTest.Models
{
    public class LoginData
    {
        [Required(ErrorMessage = "ユーザIDを入力してください。")]
        [StringLength(16, ErrorMessage = "ユーザIDが長すぎます。")]
        public string UserID { get; set; }

        [Required(ErrorMessage = "パスワードを入力してください。")]
        [StringLength(32, ErrorMessage = "パスワードが長すぎます。")]
        public string Password { get; set; }
    }
}
