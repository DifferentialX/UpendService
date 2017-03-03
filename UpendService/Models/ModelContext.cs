using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using UpendService.Models;

namespace UpendService
{
	public class ModelContext
	{
		public CloudTable Actions;
		public CloudTable Tasks;
		public CloudTable Users;

		public ModelContext(IConfigurationRoot configuration)
		{
			var connection = configuration.GetConnectionString("UpendStorageConnection");
			CloudStorageAccount account = CloudStorageAccount.Parse(connection);

			Actions = GetTable<Action>(account);
			Tasks = GetTable<Task>(account);
			Users = GetTable<User>(account);
		}

		private CloudTable GetTable<T>(CloudStorageAccount account)
		{
			CloudTable table = account.CreateCloudTableClient().GetTableReference(typeof(T).Name + "s");
			table.CreateIfNotExistsAsync().Wait();
			return table;
		}
	}
}