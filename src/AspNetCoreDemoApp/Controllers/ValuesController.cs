using System.Collections.Generic;
using System.IO;
using System.Net;
using AspNetCoreDemoApp.Classes;
using AspNetCoreDemoApp.Models;
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
        public object Post([FromBody]Skill data)
        {
            if (data.session.user.accessToken == null)
            {
                var skill = new Skill()
                {
                    version = "1.0",
                    response = new Response()
                    {
                        outputSpeech = new OutputSpeech()
                        {
                            type = "PlainText",
                            text = "Please use the companion app to authenticate on Amazon to start using this skill"
                        },
                        card = new Card()
                        {
                            type = "LinkAccount"
                        },
                        shouldEndSession = false
                    },
                    sessionAttributes = null
                };
                return skill;
            }
            else
            {
                var amznProfileURL = "https://api.amazon.com/user/profile?access_token=";
                amznProfileURL += data.session.user.accessToken;
                var rq = WebRequest.Create(amznProfileURL);
                var html = string.Empty;
                using (var response = rq.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    html = reader.ReadToEnd();
                }

                return new { version = "1.0", response = new { outputSpeech = new { type = "PlainText", text = "The status of your tax return is awaiting for client response" } } };
            }
		}

        // POST: api/values/updateStatus
        [Route("updateStatus")]
        [HttpPost]
        public object UpdateState(string taxyear, string email, int status, decimal taxdue)
        {
            Helper.WriteState(taxyear, email, status, taxdue);
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
