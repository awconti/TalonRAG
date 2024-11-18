namespace TalonRAG.Domain.Entity
{
    /// <summary>
    /// Represents persisted embedded article content.
    /// </summary>
    public class ArticleEmbedding : Microsoft.Extensions.AI.Embedding
    {
        /// <summary>
        /// Float array representing <see cref="Article.Description"/>.
        /// </summary>
        public float[]? Embedding { get; set; }

        /// <summary>
        /// <see cref="Article.Description"/>
        /// </summary>
        public string? Content { get; set; }
    }
}
