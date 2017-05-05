using UpendService;
using UpendService.Controllers;
using Xunit;
using Task = UpendService.Models.Task;
using System.Linq;
namespace UpendServiceTest
{
	public class TaskControllerTest
	{
		private const string NAME = "Test";

		[Fact]
		public void AddUserToTable()
		{
			// Arrange 
			var tasks = new FakeTable<Task> { };
			var tableFactory = new TestTableFactory { Tasks = tasks };
			var model = new ModelContext(tableFactory);
			var controller = new TaskController(model, new TestIdentity());

			// Act
			controller.Post(new Task { Name = NAME });

			// Assert
			Assert.NotEmpty(tasks.Data);
			Assert.Equal(NAME, tasks.Data.First().Data.Name);
		}
	}
}
