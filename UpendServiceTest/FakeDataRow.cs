using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UpendService.Models;

namespace UpendServiceTest
{
	public class FakeDataRow<T> where T : Data<T>
	{
		public string PartitionKey { get; set; }
		public string RowKey { get; set; }
		public T Data { get; set; }
	}
}
