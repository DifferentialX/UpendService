using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UpendService.Models;


namespace UpendService.Services.Tables
{
	public class FakeTable<U> : ITable where U : Data<U>
	{
		List<FakeDataRow<U>> data = new List<FakeDataRow<U>>();

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
			throw new NotImplementedException();
		}

		public void Update<T>(T data, string partitionKey) where T : Data<T>
		{
			throw new NotImplementedException();
		}
	}
}
