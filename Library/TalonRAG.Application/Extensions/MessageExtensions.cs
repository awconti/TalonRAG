using TalonRAG.Application.DataTransferObjects;
using TalonRAG.Domain.Models;

namespace TalonRAG.Application.Extensions
{
    /// <summary>
    /// Mapper class responsible for constructing <see cref="MessageDto" /> instances on behalf of the application.
    /// </summary>
    public static class MessageExtensions
    {
        /// <summary>
        /// Creates a new <see cref="MessageDto" /> based on a <see cref="Message" /> model instance.
        /// </summary>
        /// <param name="message">
        /// <see cref="Message" />.
        /// </param>
        public static MessageDto ToDto(this Message message)
        {
            return new MessageDto
            {
                AuthorRole = message.AuthorRole,
                Content = message.Content,
                CreateDate = message.CreateDate
            };
        }
    }
}
