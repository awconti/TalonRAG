﻿using System.Text.Json.Serialization;

namespace TalonRAG.Application.DataTransferObjects.External
{
    /// <summary>
    /// DTO representing an article retrieved from <a href="https://newsapi.org">News API</a>.
    /// </summary>
    public class NewsApiV2ArticleDto
    {
        /// <summary>
        /// Represents description field in JSON response.
        /// </summary>
        [JsonPropertyName("description")]
        public required string Description { get; set; }
    }
}
