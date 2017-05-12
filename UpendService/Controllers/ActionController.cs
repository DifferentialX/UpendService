using System;
using UpendService.Services;
using Action = UpendService.Models.Action;

namespace UpendService.Controllers
{
	public class ActionController : BaseController<Action>
	{
		public ActionController(ModelContext model, ICurrentIdentity identity) : base(model, identity) { }

		public override bool IsValid(Action data)
		{
			if (data.TaskGuid == Guid.Empty || data.Time < new DateTimeOffset(2012, 05, 01, 00, 00, 00, new TimeSpan(0)))
				return false;
			return true;
		}
	}
}
