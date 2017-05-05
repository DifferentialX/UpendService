using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace UpendService.Services.CurrentIdentity
{
	public class CurrentIdentity : ICurrentIdentity
	{
		private readonly ClaimsPrincipal identity;

		public CurrentIdentity(IHttpContextAccessor accessor)
		{
			identity = accessor.HttpContext.User;
		}

		public override string Id => identity.Id();

		public override string Name { get; }
		public override string Email { get;  }
		public override string Source => identity.Source();
	}
}
