using UpendService.Models;
using UpendService.Services;
using UpendService.Services.TableFactory;
using Task = UpendService.Models.Task;

namespace UpendServiceTest
{
	public class TestTableFactory : ITableFactory
	{
		public ITable Tasks { get; set; } = new FakeTable<Task>();
		public ITable Actions { get; set; } = new FakeTable<Action>();
		public ITable Users { get; set; } = new FakeTable<User>();

		public ITable CreateTable<T>() where T : Data<T>
		{
			var type = typeof(T);

			if(type == typeof(Task))
			{
				return Tasks;
			}
			else if(type == typeof(Action))
			{
				return Actions;
			}
			else if(type == typeof(User))
			{
				return Users;
			}
			else
			{
				throw new System.Exception("Unexpected type");
			}
		}
	}
}
