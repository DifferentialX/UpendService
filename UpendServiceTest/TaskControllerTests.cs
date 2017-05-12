using System;
using System.Collections.Generic;
using UpendService.Models;
using Task = UpendService.Models.Task;

namespace UpendServiceTest
{
	public class TaskControllerTest : ControllerTest<Task>
	{
		private const string NAME = "Test";
		private readonly Guid VALIDGUID = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
		public override Task InvalidData => new Task { Name = null, Size = -1, Color = -1, TaskGuid = Guid.Empty, LastUpdate = DateTimeOffset.MaxValue };

		public override bool Equal(Task actual, Task expected)
		{
			return
				actual.LastUpdate == expected.LastUpdate &&
				actual.Name == expected.Name &&
				actual.Size == expected.Size &&
				actual.SnoozeUntil == expected.SnoozeUntil &&
				actual.TaskGuid == expected.TaskGuid;
		}

		public override Task ValidData => new Task { Name = NAME };

		public override IList<Task> InvalidItems => new List<Task>
		{
			new Task(),
			new Task{Name = null},
			new Task{Name = ""},
			new Task{Name = NAME},
			new Task{Name = null, TaskGuid = VALIDGUID },
			new Task{Name = "",   TaskGuid = VALIDGUID },
			new Task{Name = NAME, TaskGuid = VALIDGUID, Color = -1},
			new Task{Name = NAME, TaskGuid = VALIDGUID, Color = 6},
			new Task{Name = NAME, TaskGuid = VALIDGUID, Size = -1},
			new Task{Name = NAME, TaskGuid = VALIDGUID, Size = 3},
		};

		public override IList<Task> ValidItems => new List<Task>
		{
			new Task{Name = NAME, TaskGuid = VALIDGUID}
		};
	}
}
