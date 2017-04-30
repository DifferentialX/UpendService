using Microsoft.Extensions.Configuration;
using UpendService.Models;
using System.Collections.Generic;
using UpendService.Services;

namespace UpendService
{
	public class ModelContext
	{
		public IDictionary<System.Type, ITable> tables;

		public ITable Actions => tables[typeof(Action)];
		public ITable Tasks => tables[typeof(Task)];
		public ITable Users => tables[typeof(User)];


		public ModelContext(IConfigurationRoot configuration)
		{
			var connection = configuration.GetConnectionString("UpendStorageConnection");

			tables = new Dictionary<System.Type, ITable>();
			StoreITable<Action>(connection);
			StoreITable<Task>(connection);
			StoreITable<User>(connection);
		}

		internal ITable GetTable<T>()
		{
			if (!tables.ContainsKey(typeof(T)))
				return null;
			return tables[typeof(T)];
		}

		private void StoreITable<T> (string connection) where T : Data<T>
		{
			var table = new AzureStorageTable(typeof(T), connection);
			tables.Add(typeof(T), table);
		}
	}
}