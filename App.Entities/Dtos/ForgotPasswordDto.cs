using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.Entities.Dtos
{
	public class ForgotPasswordDto
	{
		[DisplayName("E-Posta Adresi")]
		[Required(ErrorMessage = "{0} boş geçilmemelidir.")]
		[EmailAddress(ErrorMessage ="E-posta formatını yanlış girdiniz.")]
		public string Email { get; set; }
	}
}
