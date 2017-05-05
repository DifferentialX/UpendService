using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UpendService.Services
{
	public abstract class ICurrentIdentity
	{
		public abstract string Id { get; }
		public abstract string Name { get; }
		public abstract string Email { get; }
		public abstract string Source { get; }

		public string UniqueId => Source + "_" + Id;
	}
}
