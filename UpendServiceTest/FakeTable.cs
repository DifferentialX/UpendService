using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UpendService.Models;
using UpendService.Services;

namespace UpendServiceTest
{
	public class FakeTable<U> : ITable where U : Data<U>
	{
		public IList<FakeDataRow<U>> Data { get; set; } = new List<FakeDataRow<U>>();

		public void Delete<T>(Where where) where T : Data<T>
		{
			throw new NotImplementedException();
		}

		public IEnumerable<T> Find<T>(Where where) where T : Data<T>
		{
			throw new NotImplementedException();
		}

		public void Insert<T>(T data, string partitionKey) where T : Data<T>
		{
			Data.Add(new FakeDataRow<U>
			{
				PartitionKey = partitionKey,
				RowKey = data.Entity(partitionKey).RowKey,
				Data = data as U
			});
		}

		public void Update<T>(T data, string partitionKey) where T : Data<T>
		{
			throw new NotImplementedException();
		}
	}
}
