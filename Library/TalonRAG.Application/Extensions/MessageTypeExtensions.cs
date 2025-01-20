using System.ComponentModel;
using System.Reflection;
using TalonRAG.Domain.Enums;

namespace TalonRAG.Application.Extensions
{
	/// <summary>
	/// Extension class used to perform additional on operations on enums of <see cref="MessageType" />.
	/// </summary>
	public static class MessageTypeExtensions
	{
		/// <summary>
		/// Converts the enum value of <see cref="MessageType"/> to its corresponding descritption string if defined.
		/// </summary>
		/// <param name="messageType">
		/// <see cref="MessageType"/>.
		/// </param>
		public static string ToDescription(this MessageType messageType)
		{
			var type = messageType.GetType();
			var fieldInfo = type.GetField(messageType.ToString());
			var descriptionAttribute = (DescriptionAttribute?) fieldInfo?.GetCustomAttribute(typeof(DescriptionAttribute), false);
			return descriptionAttribute is not null ? descriptionAttribute.Description : string.Empty;
		}
	}
}
