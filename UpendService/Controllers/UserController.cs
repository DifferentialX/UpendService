﻿using UpendService.Models;
using UpendService.Services;

namespace UpendService.Controllers
{
	public class UserController : BaseController<User>
	{
		public UserController(ModelContext model, ICurrentIdentity identity) : base(model, identity) { }

		public override bool IsValid(User data)
		{
			if (string.IsNullOrEmpty(data.Email) || string.IsNullOrEmpty(data.Name))
				return false;
			return true;
		}
	}
}
