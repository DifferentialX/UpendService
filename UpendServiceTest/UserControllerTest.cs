using System;
using System.Collections.Generic;
using System.Text;
using UpendService.Models;

namespace UpendServiceTest
{
	public class UserControllerTest : ControllerTest<User>
	{
		public const string EMAIL = "email";
		public const string NAME = "name";
		public readonly Dictionary<string, string> DICT = new Dictionary<string, string>();

		public override IList<User> InvalidItems => new List<User>
		{
			new User (),
			new User { Email = null },
			new User { Email = EMAIL },
			new User { Name = null },
			new User { Name = NAME },
			new User { Settings = DICT },
			new User { Email = "", Name = "" },
			new User { Email = null, Settings = DICT },
			new User { Name = null, Settings = DICT },
			new User { Email = EMAIL, Settings = DICT },
			new User { Name = NAME, Settings = DICT },
			new User { Email = "", Name = "", Settings = DICT },
		};

		public override IList<User> ValidItems => new List<User>
		{
			ValidData,
			new User { Email = EMAIL, Name = NAME, Settings = DICT},
		};

		public override User ValidData => new User { Email = EMAIL, Name = NAME };

		public override User InvalidData => new User();

		public override bool Equal(User actual, User expected)
		{
			return actual == expected;
		}
	}
}
