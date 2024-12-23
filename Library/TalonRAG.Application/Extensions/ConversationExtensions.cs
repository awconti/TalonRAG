using TalonRAG.Application.DataTransferObjects;
using TalonRAG.Domain.Models;

namespace TalonRAG.Application.Extensions
{
    /// <summary>
    /// Mapper class responsible for constructing <see cref="ConversationDto" /> instances on behalf of the application.
    /// </summary>
    public static class ConversationExtensions
    {
        /// <summary>
        /// Creates a new <see cref="ConversationDto" /> instance based on a <see cref="Conversation" /> model instance 
        /// and an optional collection of <see cref="Message" /> model instances.
        /// </summary>
        /// <param name="conversation">
        /// <see cref="Conversation" />.
        /// </param>
        public static ConversationDto ToDto(this Conversation conversation)
        {
            return new ConversationDto
            {
                Id = conversation.Id,
                UserId = conversation.UserId,
                CreateDate = conversation.CreateDate,
                Messages = conversation.Messages.Select(message => message.ToDto()).ToList()
            };
        }
    }
}
