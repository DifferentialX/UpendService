using System;
using System.Collections.Generic;

namespace UpendService.Models
{
	public class User : Data<User>
	{
		public string Name { get; set; }
		public Dictionary<string, string> Settings { get; set; }
		public string Email { get; set; }

		public override DataEntity<User> Entity(string partition)
		{
			return new DataEntity<User>(this, partition);
		}
	}
}