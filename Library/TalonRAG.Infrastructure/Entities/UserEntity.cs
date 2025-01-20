namespace TalonRAG.Infrastructure.Entities
{
	/// <summary>
	/// Represents a single persisted user.
	/// </summary>
	public class UserEntity
	{
		/// <summary>
		/// The unique identifier of the user.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// The email of the user.
		/// </summary>
		public string Email { get; set; } = string.Empty;

		/// <summary>
		/// The date and time for which the user was created.
		/// </summary>
		public DateTime CreateDate { get; set; }
	}
}
