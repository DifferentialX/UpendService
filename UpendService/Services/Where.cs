using System.Collections.Generic;

namespace UpendService.Services
{
	public class Where : Dictionary<string, string>
	{
		public const string PARTITIONKEY = "PartitionKey";
		public const string ROWKEY = "RowKey";

		public string Partition => this[PARTITIONKEY];
		public string Row => this[ROWKEY];

		public bool HasRow => ContainsKey(ROWKEY);

		private Where(string partitionKey, string rowKey = null)
		{
			Add(PARTITIONKEY, partitionKey);
			if(rowKey != null)
				Add(ROWKEY, rowKey);
		}

		public static Where Query(string partitionKey, string rowKey = null)
		{
			return new Where(partitionKey, rowKey);
		}
	}
}
