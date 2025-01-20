using TalonRAG.Application.DataTransferObjects;
using TalonRAG.Domain.Models;

namespace TalonRAG.Application.Extensions
{
    /// <summary>
    /// Mapper class responsible for constructing <see cref="MessageDto" /> instances on behalf of the application.
    /// </summary>
    public static class MessageModelExtensions
    {
        /// <summary>
        /// Creates a new <see cref="MessageDto" /> based on a <see cref="MessageModel" /> model instance.
        /// </summary>
        /// <param name="message">
        /// <see cref="MessageModel" />.
        /// </param>
        public static MessageDto ToDto(this MessageModel message)
        {
            return new MessageDto
            {
                MessageType = message.MessageType.ToDescription(),
                Content = message.Content,
                CreateDate = message.CreateDate
            };
        }
    }
}
