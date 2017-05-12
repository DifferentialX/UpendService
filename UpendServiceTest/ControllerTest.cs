using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UpendService;
using UpendService.Controllers;
using UpendService.Models;
using Xunit;

namespace UpendServiceTest
{
	public abstract class ControllerTest<T> where T : Data<T>
	{
		protected readonly FakeTable<T> data;
		protected readonly BaseController<T> controller;

		public ControllerTest()
		{
			var tableFactory = new TestTableFactory {};
			data = tableFactory.CreateTable<T>() as FakeTable<T>;

			var model = new ModelContext(tableFactory);
			controller = ControllerFactory.Create<T>(model, new TestIdentity());
		}

		// Add Data to Table Test
		[Fact]
		public void AddDataToTable()
		{
			Assert.Empty(data.Data);
			T dataObject = ValidData;

			// Act
			controller.Post(dataObject);

			// Assert
			Assert.NotEmpty(data.Data);
			Assert.True(Equal(data.Data.First().Data, dataObject));
		}

		// Add no Data because the Data was invalid
		[Fact]
		public void FailToAddInvalidData()
		{
			Assert.Empty(data.Data);
			T dataObject = InvalidData;

			// Act
			controller.Post(dataObject);

			// Assert
			Assert.Empty(data.Data);
		}

		[Fact]
		public void ValidityTest()
		{
			foreach(T t in InvalidItems)
				Assert.False(controller.IsValid(t));
			foreach(T t in ValidItems)
				Assert.True(controller.IsValid(t));
		}

		public abstract IList<T> InvalidItems { get; }
		public abstract IList<T> ValidItems { get; }

		public abstract T ValidData { get; }
		public abstract T InvalidData { get; }
		public abstract bool Equal(T actual, T expected);
	}
}
