using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using UpendService.Models;
using System.Collections.Generic;

namespace UpendService
{
	public class ModelContext
	{
		public CloudTable Actions { get; set; }
		public CloudTable Tasks { get; set; }
		public CloudTable Users { get; set; }

		private Dictionary<System.Type, CloudTable> tables;

		public ModelContext(IConfigurationRoot configuration)
		{
			var connection = configuration.GetConnectionString("UpendStorageConnection");
			CloudStorageAccount account = CloudStorageAccount.Parse(connection);

			tables = new Dictionary<System.Type, CloudTable>();

			Actions = GetTable<Action>(account);
			Tasks = GetTable<Task>(account);
			Users = GetTable<User>(account);
		}

		public CloudTable GetTable<T>()
		{
			return tables[typeof(T)];
		}
			

		private CloudTable GetTable<T>(CloudStorageAccount account)
		{
			CloudTable table = account.CreateCloudTableClient().GetTableReference(typeof(T).Name + "s");
			table.CreateIfNotExistsAsync().Wait();
			tables.Add(typeof(T), table);
			return table;
		}
	}
}