using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace UpendService.Controllers
{
	[Route("api/[controller]")]
	public class ValuesController : Controller
	{
		protected readonly string Connection;
		public ValuesController(IConfigurationRoot config)
		{
			Connection = config.GetConnectionString("UpendStorageConnection");
		}

		// GET api/values
		[HttpGet]
		public IEnumerable<string> Get()
		{
			return new string[] { "value1", "value2", Connection };
		}

		// GET api/values/5
		[HttpGet("{id}")]
		public int Get(int id)
		{
			return id;
		}

		// POST api/values
		[HttpPost]
		public void Post([FromBody]string value)
		{
		}

		// PUT api/values/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody]string value)
		{
		}

		// DELETE api/values/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
