using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace UpendService
{
	public static class ClaimsPrincipalExtensions
	{
		public static string Id(this ClaimsPrincipal identity)
		{
			return identity.FindFirst("http://schemas.microsoft.com/identity/claims/identityprovider")?.Value;
		}

		public static string Source(this ClaimsPrincipal identity)
		{
			return identity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		}
	}
}
