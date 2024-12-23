using System.Text.Json.Serialization;

namespace TalonRAG.Application.DataTransferObjects
{
	/// <summary>
	/// DTO representing HTTP response from <a href="https://newsapi.org">News API</a>.
	/// </summary>
	public class NewsApiV2Response
	{
		/// <summary>
		/// List collection of <see cref="IList{NewsApiArticle?}"/>
		/// </summary>
		[JsonPropertyName("articles")]
		public IList<NewsApiV2ArticleDto>? Articles { get; set; }
	}
}
