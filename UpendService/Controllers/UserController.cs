using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using UpendService.Models;

namespace UpendService.Controllers
{
	public class UserController : BaseController<User>
	{
		public UserController(ModelContext model) : base(model) { }

		public override IEnumerable<User> Get()
		{
			var filter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, GetCurrentUniqueIdentifier());
			var query = new TableQuery<DataEntity<User>>().Where(filter);

			return Table.ExecuteQuery(query).ToList().Select(x => x.Data);
		}

		// POST: api/User
		public override Guid Post([FromBody]User value)
		{
			value.UserGuid = Guid.NewGuid();
			return base.Post(value);
		}

		// DELETE: api/User/5
		public override void Delete(Guid id)
		{
			base.Delete(id);
			new TaskController(Model).Delete(id);
			new ActionController(Model).Delete(id);
		}

		public override string RowKey(User u)
		{
			return u.UserGuid.ToString();
		}

		public override Guid DataToReturnUponCreation(User data)
		{
			return data.UserGuid;
		}

		internal override bool IsValid(User data)
		{
			if (string.IsNullOrEmpty(data.Email) || string.IsNullOrEmpty(data.Name))
				return false;
			return true;
		}
	}
}
