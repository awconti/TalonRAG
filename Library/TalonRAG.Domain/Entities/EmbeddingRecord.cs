namespace TalonRAG.Domain.Entities
{
    /// <summary>
    /// Represents persisted embedded article desription content.
    /// </summary>
    public class EmbeddingRecord
    {
        /// <summary>
        /// The unique database identifier for the article embedding.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Embedding representing description of article.
        /// </summary>
        public float[]? VectorEmbedding { get; set; }

        /// <summary>
        /// Description of article.
        /// </summary>
        public string? Content { get; set; }
    }
}
