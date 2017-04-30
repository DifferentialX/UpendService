using System;

namespace UpendService.Models
{
	public class Task : Data<Task>
	{
		public Guid TaskGuid { get; set; }
		public string Name { get; set; }
		public int Size { get; set; }
		public int Color { get; set; }
		public DateTimeOffset SnoozeUntil { get; set; }
		public bool Active { get; set; }
		public DateTimeOffset LastUpdate { get; set; }

		public override DataEntity<Task> Entity(string partition)
		{
			return new DataEntity<Task>(this, partition, TaskGuid.ToString());
		}
	}
}