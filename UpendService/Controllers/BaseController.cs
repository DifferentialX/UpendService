using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using UpendService.Models;
using System.Reflection;
using UpendService.Services;

namespace UpendService.Controllers
{
	[Route("api/[controller]")]
	//[Authorize]
	public abstract class BaseController<T> : Controller
		where T : Data
	{
		protected readonly ModelContext Model;
		protected CloudTable Table { get; set; }
		protected readonly ITable Table2;
		public BaseController(ModelContext model)
		{
			
			Model = model;
			Table = model.GetTable<T>();
			Table2 = model.GetTable2<T>();
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
			return Table2.Find<T>(GetCurrentUniqueIdentifier());
		}

		[NonAction]
		private IEnumerable<DataEntity<T>> GetEntities(Guid id)
		{
			return Table2.FindEntities<T>(id.ToString());
		}

		[NonAction]
		protected IEnumerable<DataEntity<T>> GetDataForRowKey(Guid id)
		{
			return Table2.FindEntities<T>(GetCurrentUniqueIdentifier(), id.ToString());
		}

		[HttpGet("{id}")]
		public IEnumerable<T> Get(Guid id)
		{
			return Table2.Find<T>(GetCurrentUniqueIdentifier(), id.ToString());
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
			Table2.Delete<T>(GetCurrentUniqueIdentifier(), id.ToString());
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
