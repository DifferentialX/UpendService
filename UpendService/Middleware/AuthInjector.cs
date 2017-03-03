using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
				CreateDevelopmentUser(context);
			}
			else
			{
				CreateProductionUser(context);
			}
			await next(context);
		}

		private static void CreateProductionUser(HttpContext context)
		{
			if (context.Request.Headers["X-MS-CLIENT-PRINCIPAL-ID"] == StringValues.Empty)
				return;
			
			var idp = context.Request.Headers["X-MS-CLIENT-PRINCIPAL-IDP"];
			var idHeader = "X-MS-TOKEN-" + idp.ToString().ToUpperInvariant() + "-ID-TOKEN";

			if (context.Request.Headers[idHeader] == StringValues.Empty)
				return;

			JwtSecurityToken token = new JwtSecurityToken(context.Request.Headers[idHeader]);
			var identity = new ClaimsIdentity(token.Claims, "Automatic");

			// Add a few custom claims to the identity that we use internally
			identity.AddClaim(new Claim("http://schemas.microsoft.com/identity/claims/identityprovider", context.Request.Headers["X-MS-CLIENT-PRINCIPAL-IDP"]));
			identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, context.Request.Headers["X-MS-CLIENT-PRINCIPAL-ID"]));

			context.User = new ClaimsPrincipal(identity);
		}

		private static void CreateDevelopmentUser(HttpContext context)
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
	}
}