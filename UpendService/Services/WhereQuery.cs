using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UpendService.Services
{
    public class WhereQuery : Dictionary<string, string>
    {
		public const string PARTITIONKEY = "PartitionKey";
		public const string ROWKEY = "RowKey";

		public string Partition => this[PARTITIONKEY];
		public string Row => this[ROWKEY];

		public bool HasRow => ContainsKey(ROWKEY);

		public WhereQuery(string partitionKey, string rowKey = null)
		{
			Add(PARTITIONKEY, partitionKey);
			if(rowKey != null)
				Add(ROWKEY, rowKey);
		}
    }
}
