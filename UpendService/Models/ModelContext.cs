using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using UpendService.Models;
using System.Collections.Generic;
using UpendService.Services;

namespace UpendService
{
	public class ModelContext
	{
		public ITable Actions2 { get; set; }
		public ITable Tasks2 { get; set; }
		public ITable Users2 { get; set; }


		public CloudTable Actions { get; set; }
		public CloudTable Tasks { get; set; }
		public CloudTable Users { get; set; }

		private IDictionary<System.Type, CloudTable> tables;
		private IDictionary<System.Type, ITable> tables2;
		private ITable GetAStoredTable<T>() where T : Data<T>
		{
			if (!tables2.ContainsKey(typeof(T)))
				return null;
			return tables2[typeof(T)];

		}


		public ModelContext(IConfigurationRoot configuration)
		{
			var connection = configuration.GetConnectionString("UpendStorageConnection");
			CloudStorageAccount account = CloudStorageAccount.Parse(connection);

			tables = new Dictionary<System.Type, CloudTable>();
			tables2 = new Dictionary<System.Type, ITable>();

			Actions = GetTable<Action>(account);
			Tasks = GetTable<Task>(account);
			Users = GetTable<User>(account);

			Actions2 = new AzureStorageTable(Actions);
			Tasks2 = new AzureStorageTable(Tasks);
			Users2 = new AzureStorageTable(Users);
		}

		public CloudTable GetTable<T>()
		{
			return tables[typeof(T)];
		}

		public ITable GetTable2<T>() where T: Data<T>
		{
			return GetAStoredTable<T>();
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