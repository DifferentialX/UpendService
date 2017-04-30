using Microsoft.WindowsAzure.Storage;
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

		public AzureStorageTable(Type type, string connection)
		{
			CloudStorageAccount account = CloudStorageAccount.Parse(connection);
			_table = account.CreateCloudTableClient().GetTableReference(type.Name + "s");
			_table.CreateIfNotExistsAsync().Wait();
		}

		#region Table Storage Specific Methods
		private IEnumerable<DataEntity<T>> FindEntities<T>(Where query) where T : Data<T>
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
		#endregion

		#region ITable
		public IEnumerable<T> Find<T>(Where where) where T : Data<T> =>
			FindEntities<T>(where).Select(d => d.Data);
		
		private void Delete<T>(IEnumerable<DataEntity<T>> data) where T : Data<T> =>
			Parallel.ForEach(data, dataEntity => _table.Execute(TableOperation.Delete(dataEntity)));

		public void Delete<T>(Where where) where T : Data<T> => 
			Delete<T>(FindEntities<T>(where));

		public void Insert<T>(T data, string partitionKey) where T : Data<T> =>
			_table.Execute(TableOperation.Insert(data.Entity(partitionKey)));

		public void Update<T>(T data, string partitionKey) where T : Data<T> =>
			_table.Execute(TableOperation.InsertOrReplace(data.Entity(partitionKey)));
#endregion
	}
}
