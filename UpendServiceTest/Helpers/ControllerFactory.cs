using UpendService;
using UpendService.Controllers;
using UpendService.Models;
using UpendService.Services;

namespace UpendServiceTest
{
	internal static class ControllerFactory
	{
		public static BaseController<T> Create<T>(ModelContext context, ICurrentIdentity identity) 
			where T : Data<T> 
		{
			var type = typeof(T);
			if (type == typeof(Task))
			{
				return new TaskController(context, identity) as BaseController<T>;
			}
			else if (type == typeof(Action))
			{
				return new ActionController(context, identity) as BaseController<T>;
			}
			else if (type == typeof(User))
			{
				return new UserController(context, identity) as BaseController<T>;
			}
			throw new System.Exception("Should never be hit.");
		}
	}
}
