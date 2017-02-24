using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading.Tasks;
using UpendService.Models;

namespace UpendService
{
	public static class TableExtensions
	{
		public static async Task<IEnumerable<T>> ExecuteQueryAsync<T>(this CloudTable table, TableQuery<T> query) where T : ITableEntity, new()
		{
			var entities = new List<T>();
			TableContinuationToken token = null;
			do
			{
				var segment = await table.ExecuteQuerySegmentedAsync(query, token);
				token = segment.ContinuationToken;

				entities.AddRange(segment.Results);
			} while (token != null);
			return entities;
		}

		public static IEnumerable<T> ExecuteQuery<T>(this CloudTable table, TableQuery<T> query) where T : ITableEntity, new()
		{
			return table.ExecuteQueryAsync(query).Result;
		}

		public static TableResult Execute(this CloudTable table, TableOperation operation)
		{
			return table.ExecuteAsync(operation).Result;
		}
	}
}
