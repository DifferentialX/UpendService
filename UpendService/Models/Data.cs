using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;

namespace UpendService.Models
{
	public abstract class Data<T> where T : Data<T>
	{
		public abstract DataEntity<T> Entity(string partition);
	}

	public class DataEntity<T> : TableEntity where T : Data<T>
	{
		public string DataString { get; set; }
		public DataEntity()
		{

		}
		public DataEntity(T data, string partitionKey, string rowKey = "")
		{
			PartitionKey = partitionKey ?? throw new ArgumentNullException(nameof(partitionKey), nameof(partitionKey) + " must be the user's SID");
			RowKey = rowKey;
			var jSet = new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore };
			DataString = JsonConvert.SerializeObject(data, jSet);
		}

		public T Data => JsonConvert.DeserializeObject<T>(DataString);
	}
}