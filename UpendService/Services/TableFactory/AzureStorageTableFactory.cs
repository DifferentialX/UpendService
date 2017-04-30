using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UpendService.Models;

namespace UpendService.Services.TableFactory
{
	public class AzureStorageTableFactory : ITableFactory
	{
		private readonly CloudStorageAccount _account;

		public AzureStorageTableFactory(IConfiguration config)
		{
			var connection = config.GetConnectionString("UpendStorageConnection");
			_account = CloudStorageAccount.Parse(connection);
		}

		public ITable CreateTable<T>() where T : Data<T>
		{
			CloudTable table = _account.CreateCloudTableClient().GetTableReference(typeof(T).Name + "s");
			table.CreateIfNotExistsAsync().Wait();

			return new AzureStorageTable(table);
		}
	}
}
