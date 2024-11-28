using System.Text.Json.Serialization;

namespace TalonRAG.Application.DataTransferObjects
{
    /// <summary>
    /// DTO representing an article retrieved from <a href="https://newsapi.org">News API</a>.
    /// </summary>
    public class NewsApiV2Article
    {
        /// <summary>
        /// Represents description field in JSON response.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }
}
