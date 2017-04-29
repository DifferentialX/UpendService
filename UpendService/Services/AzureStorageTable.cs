using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UpendService.Models;

namespace UpendService.Services
{
	public class AzureStorageTable : ITable
	{
		private readonly CloudTable _table;

		public AzureStorageTable(CloudTable table)
		{
			_table = table;
		}

		public IEnumerable<DataEntity<T>> FindEntities<T>(string partitionKey, string rowKey = null) where T : Data
		{
			return FindEntities<T>(new WhereQuery(partitionKey, rowKey));
		}

		public IEnumerable<DataEntity<T>> FindEntities<T>(WhereQuery query) where T : Data
		{
			string filter = null;
			foreach (var que in query)
			{
				var condition = TableQuery.GenerateFilterCondition(que.Key, QueryComparisons.Equal, que.Value);
				filter = CombineConditions(filter, condition);
			}

			var tableQuery = new TableQuery<DataEntity<T>>().Where(filter);
			return _table.ExecuteQuery(tableQuery).ToList();
		}

		private static string CombineConditions(string filter, string condition)
		{
			if (filter == null)
				filter = condition;
			else
				filter = TableQuery.CombineFilters(filter, TableOperators.And, condition);
			return filter;
		}

		public IEnumerable<T> Find<T>(string partitionKey, string rowKey = null) where T : Data
		{
			return Find<T>(new WhereQuery(partitionKey, rowKey));
		}

		public IEnumerable<T> Find<T>(WhereQuery where) where T : Data
		{
			return FindEntities<T>(where).Select(d => d.Data);
		}
		
		private void Delete<T>(IEnumerable<DataEntity<T>> data) where T : Data =>
			Parallel.ForEach(data, dataEntity => _table.Execute(TableOperation.Delete(dataEntity)));

		public void Delete<T>(string partitionKey, string rowKey = null) where T : Data => 
			Delete<T>(FindEntities<T>(new WhereQuery(partitionKey, rowKey)));

		public void Insert<T>(T data) where T : Data
		{
			_table.Execute(TableOperation.Insert(Entity(data)));
		}
	}
}
