using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalonRAG.Domain.Models
{
	public class UserModel
	{
		public int Id { get; set; }

		public string Email { get; set; } = string.Empty;

		public DateTime CreateDate { get; set; }
	}
}
