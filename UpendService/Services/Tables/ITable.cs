using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UpendService.Models;

namespace UpendService.Services
{
	public interface ITable
	{
		IEnumerable<T> Find<T>(Where where) where T : Data<T>;
		void Insert<T>(T data, string partitionKey) where T : Data<T>;
		void Update<T>(T data, string partitionKey) where T : Data<T>;
		void Delete<T>(Where where) where T : Data<T>;
	}
}
