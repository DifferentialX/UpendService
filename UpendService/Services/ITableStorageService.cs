using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UpendService.Models;

namespace UpendService.Services
{
	public interface ITable
	{
		[Obsolete("We should use the `WhereQuery` method for this.")]
		IEnumerable<T> Find<T>(string partitionKey, string rowKey = null) where T : Data;

		IEnumerable<T> Find<T>(WhereQuery where) where T : Data;

		[Obsolete("Should not be public because we should not rely on TableEntities")]
		IEnumerable<DataEntity<T>> FindEntities<T>(string partitionKey, string rowKey = null) where T : Data;

		void Delete<T>(string partitionKey, string rowKey = null) where T : Data;

		void Insert<T>(T data) where T : Data;
	}
}
