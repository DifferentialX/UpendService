using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using UpendService;
using UpendService.Models;
using Entity = UpendService.Models.DataEntity<UpendService.Models.Task>;
using Action = UpendService.Models.Action;
using Microsoft.Extensions.Configuration;

namespace UpendService.Controllers
{
	public class TaskController : BaseController<Task>
	{
		public override Guid Post([FromBody]Task value)
		{
			value.TaskGuid = Guid.NewGuid();
			return base.Post(value);
		}

		public override string RowKey(Task data)
		{
			return data.TaskGuid.ToString();
		}

		public override void Delete(Guid id)
		{
			base.Delete(id);

			// Delete all actions whose task guid is this one
			var filter = TableQuery.GenerateFilterCondition("TaskGuid", QueryComparisons.Equal, id.ToString());
			var query = new TableQuery<ActionDataEntity>().Where(filter);
			var actions = ActionController.Table.ExecuteQuery(query).ToList();

			foreach (var action in actions)
			{
				ActionController.Table.Execute(TableOperation.Delete(action));
			}


			//// Delete all actions whose task guid is this one
			//var filter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, GetCurrentUniqueIdentifier());
			//var query = new TableQuery<ActionDataEntity>().Where(filter);
			//var actions = Table.ExecuteQuery(query).ToList().Where(action => action.TaskGuid == id.ToString());

			//foreach (var action in actions)
			//{
			//	ActionController.Table.Execute(TableOperation.Delete(action));
			//}

			//ActionController.Table.DeleteAsync
		}

		public override Guid DataToReturnUponCreation(Task data)
		{
			return data.TaskGuid;
		}

		public override IEnumerable<Entity> DataToDelete(Guid id)
		{
			return GetDataForRowKey(id);
		}

		internal override bool IsValid(Task data)
		{
			if (data.Color < 0 || data.Color > 5 || string.IsNullOrEmpty(data.Name) || data.Size < 0 || data.Size > 2 || data.TaskGuid == Guid.Empty)
				return false;
			return true;
		}
	}
}
