using System;
using UpendService.Services;

namespace UpendServiceTest
{
	class TestIdentity : ICurrentIdentity
	{
		public override string Id { get; } = "FAKEID";

		public override string Name { get; } = "Test Tester";

		public override string Email { get; } = "test@nowdo.xyz";

		public override string Source { get; } = "TEST";
	}
}
