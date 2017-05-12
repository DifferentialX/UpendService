using System;
using System.Collections.Generic;
using UpendService.Models;
using Xunit;
using Action = UpendService.Models.Action;
namespace UpendServiceTest
{
	public class ActionControllerTest : ControllerTest<Action>
	{
		private readonly Guid VALIDGUID = new Guid(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 0);

		public override bool Equal(Action actual, Action expected)
		{
			return actual == expected;
		}

		public override Action ValidData => new Action { Time = DateTimeOffset.UtcNow, TaskGuid = VALIDGUID};
		public override Action InvalidData => new Action { Time = DateTimeOffset.MinValue, TaskGuid = Guid.Empty };

		public override IList<Action> InvalidItems => new List<Action>
		{
			new Action(),
			new Action{TaskGuid = Guid.Empty, Time = DateTimeOffset.MaxValue}, //Invalid TaskGuid
			new Action{TaskGuid = VALIDGUID, Time = new DateTimeOffset(2012, 04, 30, 0, 0, 0, TimeSpan.Zero)}, //Time before graduation
		};

		public override IList<Action> ValidItems => new List<Action>
		{
			ValidData,
		};
	}
}
