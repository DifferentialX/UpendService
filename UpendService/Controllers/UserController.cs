using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using UpendService.Models;

namespace UpendService.Controllers
{
	public class UserController : BaseController<User>
	{
		public UserController(ModelContext model) : base(model) { }

		internal override bool IsValid(User data)
		{
			if (string.IsNullOrEmpty(data.Email) || string.IsNullOrEmpty(data.Name))
				return false;
			return true;
		}
	}
}
