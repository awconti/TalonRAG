namespace TalonRAG.Domain.Models
{
	/// <summary>
	/// Model representing a user on behalf of the domain.
	/// </summary>
	public class UserModel
	{
		/// <summary>
		/// The unique identifier of the user.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// The email address of the user.
		/// </summary>
		public string Email { get; set; } = string.Empty;

		/// <summary>
		/// The date and time the user was created.
		/// </summary>
		public DateTime CreateDate { get; set; }
	}
}
