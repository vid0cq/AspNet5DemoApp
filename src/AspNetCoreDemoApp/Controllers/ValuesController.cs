using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using AspNetCoreDemoApp.Classes;
using AspNetCoreDemoApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
//using Npgsql;

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
                if(data.request.type== "LaunchRequest")
                {
                    return new { version = "1.0", response = new { outputSpeech = new { type = "PlainText", text = "Welcome to Personal tax" } } };
                }

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

                var msg = String.Empty;
                State state = new State();
                String email = ((State)JsonConvert.DeserializeObject(html, state.GetType())).Email;
                msg += email + " ";
                state = Helper.ReadState("2018", email);
                msg += state == null ? "email not found " : state.Email;

                if(data.request.intent.name== "ReturnStatus")
                    return new { version = "1.0", response = new { outputSpeech = new { type = "PlainText", text = state.Status } } };
                else if(data.request.intent.name == "ReturnTaxDue")
                    return new { version = "1.0", response = new { outputSpeech = new { type = "PlainText", text = state.TaxDue} } };

                return new { version = "1.0", response = new { outputSpeech = new { type = "PlainText", text = "I can't find the tax information you requested"} } };
            }
		}

        // POST: api/values/updateStatus
        [Route("updateStatus")]
        [HttpPost]
        public object UpdateState(string taxyear, string email, string status, decimal taxdue, string name)
        {
            var a = Helper.WriteState(taxyear, email, status, taxdue, name);
            if (a != null)
                return new { response = a };
            else
                return new { response = "Status updated to " + status + " and tax due updated to " + taxdue + " for " + email + " for taxyear " + taxyear + " for name" + name};
        }

        // GET api/values/5
        [HttpGet("{id}")]
		public string Get(int id)
		{
			return "value" + id + "change(s)";
		}

        [Route("testDatabaseConnection")]
        [HttpGet]
        // GET api/values/TestDatabaseConnection
        public object testDatabaseConnection()
        {
            string msg = String.Empty;
            //try
            //{
                
            //    NpgsqlConnection conn = new NpgsqlConnection("Server=ec2-79-125-14-195.eu-west-1.compute.amazonaws.com; Port=5432; User Id=jakalnwdyiotao; Password=aea0bebd0295f72e075e6b0da710f4c150898d7d608933d772bf9b2f330e7202; Database=d1gbb0gjgjner9; SSL Mode=Require;Trust Server Certificate=true;"); //<ip> is an actual ip address
            //    conn.Open();
            //    NpgsqlCommand cmd = new NpgsqlCommand();
            //    NpgsqlDataReader dr = cmd.ExecuteReader();
            //    if (dr.Read())
            //    {
            //        boolfound = true;
            //        msg = "connection established";
            //    }
            //    if (boolfound == false)
            //    {
            //        msg = "Data does not exist";
            //    }
            //    dr.Close();
            //    conn.Close();

                return msg;
            //}
            //catch(Exception e)
            //{
            //    return e.ToString();
            //}
        }
	}
}
