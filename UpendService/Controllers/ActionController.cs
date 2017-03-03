using System;
using Microsoft.WindowsAzure.Storage.Table;
using UpendService.Models;
using Action = UpendService.Models.Action;
using Microsoft.Extensions.Configuration;

namespace UpendService.Controllers
{
	public class ActionController : BaseController<Action>
	{
		public ActionController(ModelContext model) : base(model) { }
		public override string RowKey(Action a)
		{
			return a.Time.Ticks.ToString();
		}

		public override Guid DataToReturnUponCreation(Action data)
		{
			return Guid.Empty;
		}

		internal override bool IsValid(Action data)
		{
			if (data.TaskGuid == Guid.Empty || data.Time < new DateTimeOffset(2012, 05, 01, 00, 00, 00, new TimeSpan(0)))
				return false;
			return true;
		}

		protected override DataEntity<Action> Entity(Action data)
		{
			return new ActionDataEntity(data, PartitionKey(data), RowKey(data));
		}
	}
}
