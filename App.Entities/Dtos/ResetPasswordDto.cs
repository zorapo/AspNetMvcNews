using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.Entities.Dtos
{
    public class ResetPasswordDto
    {

        [DisplayName("E-Posta Adresi")]
        [Required(ErrorMessage = "{0} boş geçilmemelidir.")]
        [MaxLength(100, ErrorMessage = "{0} {1} karakterden fazla olmamalıdır.")]
        [MinLength(10, ErrorMessage = "{0} {1} karakterden az olmamalıdır.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DisplayName("Yeni Şifre")]
        [Required(ErrorMessage = "{0} boş geçilmemelidir.")]
        [MaxLength(30, ErrorMessage = "{0} {1} karakterden fazla olmamalıdır.")]
        [MinLength(8, ErrorMessage = "{0} {1} karakterden az olmamalıdır.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Yeni Şifre Tekrar")]
        [Required(ErrorMessage = "{0} boş geçilmemelidir.")]
        [MaxLength(30, ErrorMessage = "{0} {1} karakterden fazla olmamalıdır.")]
        [MinLength(8, ErrorMessage = "{0} {1} karakterden az olmamalıdır.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Girmiş olduğunuz Şifre ile Şifre Tekrarı eşleşmelidir")]
        public string RepeatPassword { get; set; }
        public string TokenCode { get; set; } // TokenCode email ile iletilecek. Bu istekleri ne zaman yaparsan yap bir token alacaksın ve bu token query string ile iletilecek. Bu da biz modelimize ilettiğimiz zaman o değeri tutacak.
    }
}
