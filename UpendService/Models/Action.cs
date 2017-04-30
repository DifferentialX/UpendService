using System;

namespace UpendService.Models
{
	public class Action : Data<Action>
	{
		public Guid TaskGuid { get; set; }
		public DateTimeOffset Time { get; set; }

		public override DataEntity<Action> Entity(string partition)
		{
			return new ActionDataEntity(this, partition, Time.Ticks.ToString());
		}
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