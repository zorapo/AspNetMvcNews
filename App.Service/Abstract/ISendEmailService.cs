namespace App.Service.Abstract
{
	public interface ISendEmailService
	{
		Task SendResetPasswordEmail(string resetPasswordEmailLink, string ToEmail);
	}
}
