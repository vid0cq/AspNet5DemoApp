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

        [Route("privacyPolicy")]
        [HttpGet]
        public string PrivacyPolicy()
        {
            return "This is the Personal TAleXa private policy";
        }

        // POST: api/values
        [HttpPost]
        public object Post()
		{
            return new { version = "1.0", response = new { outputSpeech = new { type = "PlainText", text = "The status of your tax return is awaiting for client response" } } };
		}

        // POST: api/values/updateStatus
        [Route("updateStatus")]
        [HttpPost]
        public object UpdateState(string taxyear, string email, int status, decimal taxdue)
        {
            return new { response = "Status updated to " + status + " and tax due updated to " + taxdue + " for " + email + " for taxyear " + taxyear };
        }

        // GET api/values/5
        [HttpGet("{id}")]
		public string Get(int id)
		{
			return "value" + id + "change(s)";
		}
	}
}
