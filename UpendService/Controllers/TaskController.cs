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
		public TaskController(ModelContext model) : base(model) { }

		public override Guid? Post([FromBody]Task value)
		{
			value.TaskGuid = Guid.NewGuid();
			return base.Post(value);
		}

		public override void Delete(Guid id)
		{
			base.Delete(id);

			//// Delete all actions whose task guid is this one
			//var filter = TableQuery.GenerateFilterCondition("TaskGuid", QueryComparisons.Equal, id.ToString()); //TaskGuid eq [id.tostring()]
			//var query = new TableQuery<ActionDataEntity>().Where(filter);
			//var actions = Model.Actions.ExecuteQuery(query).ToList();

			//foreach (var action in actions)
			//{
			//	Model.Actions.Execute(TableOperation.Delete(action));
			//}
		}

		public override Guid? CreateResponse(Task data)
		{
			return data.TaskGuid;
		}

		internal override bool IsValid(Task data)
		{
			if (data.Color < 0 || data.Color > 5 || string.IsNullOrEmpty(data.Name) || data.Size < 0 || data.Size > 2 || data.TaskGuid == Guid.Empty)
				return false;
			return true;
		}
	}
}
