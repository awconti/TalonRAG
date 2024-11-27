using System.Text.Json.Serialization;

namespace TalonRAG.Infrastructure.DataTransferObjects
{
	/// <summary>
	/// DTO representing HTTP response from <a href="https://newsapi.org">News API</a>.
	/// </summary>
	public class NewsApiResponse
	{
		/// <summary>
		/// List collection of <see cref="IList{NewsApiArticle?}"/>
		/// </summary>
		[JsonPropertyName("articles")]
		public IList<NewsApiArticle>? Articles { get; set; }
	}
}
