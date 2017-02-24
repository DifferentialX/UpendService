using System.Collections.Generic;

namespace UpendService.Models
{
	public class User : Data
	{
		public string Name { get; set; }
		public Dictionary<string, string> Settings { get; set; }
		public string Email { get; set; }
	}
}