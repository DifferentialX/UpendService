using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using UpendService.Models;
using UpendService.Services;

namespace UpendService.Controllers
{
	[Route("api/[controller]")]
	//[Authorize]
	public abstract class BaseController<T> : Controller
		where T : Data<T>
	{
		protected readonly ModelContext _model;
		protected readonly ITable _table;
		protected readonly ICurrentIdentity _identity;

		public BaseController(ModelContext model, ICurrentIdentity identity)
		{
			
			_model = model;
			_table = model.GetTable<T>();
			_identity = identity;
		}
		#region Actions

		[HttpGet]
		public virtual IEnumerable<T> Get() =>
			_table.Find<T>(Where.Query(Partition));

		[HttpGet("{id}")]
		public IEnumerable<T> Get(Guid id) =>
			_table.Find<T>(Where.Query(Partition, id.ToString()));

		[HttpPost]
		public virtual Guid? Post([FromBody]T data)
		{
			if (!IsValid(data)) return null;
			_table.Insert(data, Partition);
			return CreateResponse(data);
		}

		[HttpPut("{id}")]
		public void Put(Guid id, [FromBody] T data)
		{
			if (!IsValid(data)) return;

			_table.Update(data, Partition);
		}

		[HttpDelete("{id}")]
		public virtual void Delete(Guid id) =>
			_table.Delete<T>(Where.Query(Partition, id.ToString()));
		#endregion

		protected string Partition => _identity.UniqueId;
		
		[NonAction]
		public virtual Guid? CreateResponse(T data) { return null; }

		[NonAction]
		public abstract bool IsValid(T data);
	}
}
