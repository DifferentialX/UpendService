using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace UpendService.Models
{
	public class Data
	{
		public Guid UserGuid { get; set; }
	}

	public class DataEntity<T> : TableEntity where T : Data
	{
		public string DataString { get; set; }
		public DataEntity()
		{

		}
		public DataEntity(T data, string partitionKey, string rowKey = "")
		{
			if (partitionKey == null)
				partitionKey = data.UserGuid.ToString();

			PartitionKey = partitionKey;
			RowKey = rowKey;
			var jSet = new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore };
			DataString = JsonConvert.SerializeObject(data, jSet);
		}

		public T Data => JsonConvert.DeserializeObject<T>(DataString);
	}
}