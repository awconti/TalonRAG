using System.Text.Json.Serialization;

namespace TalonRAG.Common.Persistence.DTO
{
	/// <summary>
	/// Represents an article retrieved from <a href="https://newsapi.org">News API</a>.
	/// </summary>
	public class Article
	{
		/// <summary>
		/// Represents description field in JSON response.
		/// </summary>
		[JsonPropertyName("description")]
		public string? Description { get; set; }
	}
}
