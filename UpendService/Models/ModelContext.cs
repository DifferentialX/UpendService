using System;
using System.Collections.Generic;
using UpendService.Models;
using UpendService.Services;
using UpendService.Services.TableFactory;
using Action = UpendService.Models.Action;

namespace UpendService
{
	public class ModelContext
	{
		public IDictionary<Type, ITable> tables;

		public ITable Actions => tables[typeof(Action)];
		public ITable Tasks => tables[typeof(Task)];
		public ITable Users => tables[typeof(User)];

		public ModelContext(ITableFactory factory)
		{
			tables = new Dictionary<Type, ITable>();

			InitializeTable<Action>(factory);
			InitializeTable<Task>(factory);
			InitializeTable<User>(factory);
		}

		internal ITable GetTable<T>()
		{
			if (!tables.ContainsKey(typeof(T)))
				return null;
			return tables[typeof(T)];
		}

		private void InitializeTable<T> (ITableFactory factory) where T : Data<T>
		{
			var table = factory.CreateTable<T>();
			tables.Add(typeof(T), table);
		}
	}
}