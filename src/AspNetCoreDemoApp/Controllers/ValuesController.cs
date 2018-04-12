using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreDemoApp.Controllers
{
	[Route("api/[controller]")]
	public class ValuesController : ControllerBase
	{
		// GET: api/values
		[HttpGet]
		public IEnumerable<string> Get()
		{
			return new[] { "The status of your tax return is awaiting for client response" };
		}
                
                // POST: api/values
                [HttpPost]
                public string Post()
		{
                        return 
                        "{ \"version\": \"1.0\", \"response\": { \"outputSpeech\": { \"type\": \"PlainText\", \"text\": \"The status of your tax return is awaiting for client response\" } } }";
		}
                
		// GET api/values/5
		[HttpGet("{id}")]
		public string Get(int id)
		{
			return "value" + id + "change(s)";
		}
	}
}
