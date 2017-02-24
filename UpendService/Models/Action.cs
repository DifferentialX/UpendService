using System;

namespace UpendService.Models
{
	public class Action : Data
	{
		public Guid TaskGuid { get; set; }
		public DateTimeOffset Time { get; set; }
	}

	public class ActionDataEntity : DataEntity<Action>
	{
		public string TaskGuid { get; set; }

		public ActionDataEntity(Action data, string partitionKey, string rowKey)
			: base(data, partitionKey, rowKey)
		{
			TaskGuid = data.TaskGuid.ToString();
		}

		public ActionDataEntity() { }
	}
}