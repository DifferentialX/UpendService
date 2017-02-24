using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace UpendService.Middleware
{
    public class AuthInjector
    {
		private readonly RequestDelegate next;
		private readonly IHostingEnvironment env;

		public AuthInjector(RequestDelegate next, IHostingEnvironment env)
		{
			this.next = next;
			this.env = env;
		}

		public async Task Invoke(HttpContext context)
		{
			if(env.IsDevelopment())
			{
				var identity = new ClaimsIdentity();
				var claims = new Claim[]
				{
					new Claim("http://schemas.microsoft.com/identity/claims/identityprovider", "fake"),
					new Claim(ClaimTypes.NameIdentifier, "testuser")
				};
				identity.AddClaims(claims);
				context.User = new ClaimsPrincipal(identity);
			}

			await next(context);
		}
	}
}