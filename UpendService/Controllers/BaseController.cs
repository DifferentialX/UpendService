using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using UpendService.Models;


namespace UpendService.Controllers
{
	[Route("api/[controller]")]
	//[Authorize]
	public abstract class BaseController<T> : Controller
		where T : Data
	{
		private static CloudTable table = default(CloudTable);
		protected static CloudTable Table
		{
			get
			{
				if (table == default(CloudTable))
				{
					CloudStorageAccount account = CloudStorageAccount.Parse("Connection String"); //TODO JAF 20170221: AAAAAAAACK will fail
					table = account.CreateCloudTableClient().GetTableReference(typeof(T).Name + "s");
					table.CreateIfNotExistsAsync();
				}
				return table;
			}
		}

		[NonAction]
		protected string GetCurrentUniqueIdentifier()
		{
			var claimsIdentity = (ClaimsPrincipal)User;

			string provider = claimsIdentity.FindFirst("http://schemas.microsoft.com/identity/claims/identityprovider").Value;
			string nameIdentifier = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
			return nameIdentifier + "_" + provider;
		}

		[HttpGet]
		public virtual IEnumerable<T> Get()
		{
			var filter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, GetCurrentUniqueIdentifier());
			var query = new TableQuery<DataEntity<T>>().Where(filter);
			return Table.ExecuteQuery(query).ToList().Select(x => x.Data);
		}

		[NonAction]
		private IEnumerable<DataEntity<T>> GetEntities(Guid id)
		{
			var filter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, id.ToString());
			var query = new TableQuery<DataEntity<T>>().Where(filter);

			return Table.ExecuteQuery(query).ToList();
		}

		[NonAction]
		protected IEnumerable<DataEntity<T>> GetDataForRowKey(Guid id)
		{
			var filter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, GetCurrentUniqueIdentifier());
			var filter2 = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, id.ToString());
			var combined = TableQuery.CombineFilters(filter, TableOperators.And, filter2);
			var query = new TableQuery<DataEntity<T>>().Where(combined);

			return Table.ExecuteQuery(query).ToList();
		}

		[HttpGet("{id}")]
		public IEnumerable<T> Get(Guid id)
		{
			return GetEntities(id).Select(x => x.Data);
		}

		//public virtual Guid Post([FromBody]string value)
		//{
		//          var data = JsonConvert.DeserializeObject<T>(value);
		//	Table.Execute(TableOperation.Insert(Entity(data)));
		//	return DataToReturnUponCreation(data);
		//}

		[HttpPost]
		public virtual Guid Post([FromBody]T value)
		{
			if (!IsValid(value))
				return Guid.Empty;
			Table.Execute(TableOperation.Insert(Entity(value)));
			return DataToReturnUponCreation(value);
		}

		[HttpPut("{id}")]
		public void Put(Guid id, [FromBody] T value)
		{
			T data = value;
			if (!IsValid(data))
				return;
			Table.Execute(TableOperation.InsertOrReplace(Entity(data)));
		}

		[HttpDelete("{id}")]
		public virtual void Delete(Guid id)
		{
			var dataEntities = DataToDelete(id);
			foreach (var dataEntity in dataEntities)
			{
				Table.Execute(TableOperation.Delete(dataEntity));
			}
		}

		[NonAction]
		public virtual IEnumerable<DataEntity<T>> DataToDelete(Guid id)
		{
			return GetEntities(id);
		}

		[NonAction]
		public abstract string RowKey(T data);

		[NonAction]
		public abstract Guid DataToReturnUponCreation(T data);

		[NonAction]
		public virtual string PartitionKey(T data)
		{
			return GetCurrentUniqueIdentifier();
		}

		[NonAction]
		protected virtual DataEntity<T> Entity(T data)
		{
			return new DataEntity<T>(data, PartitionKey(data), RowKey(data));
		}

		[NonAction]
		internal virtual bool IsValid(T data)
		{
			return true;
		}
	}
}
