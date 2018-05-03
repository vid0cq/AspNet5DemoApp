using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using AspNetCoreDemoApp.Classes;
using AspNetCoreDemoApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
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
                        shouldEndSession = true
                    },
                    sessionAttributes = null
                };
                return skill;
            }
            else
            {
                try
                {
                    State state = GetState(data);
                    switch (data.request.type)
                    {
                        case "LaunchRequest":
                        case "SessionEndedRequest":
                            return FlowResponse(state, data.request.type);
                        default:
                            return IntentResponse(data.request.intent, state);
                    }
                }
                catch(Exception e)
                {
                    var skill = IntentResponse(null, null);
                    skill.response.outputSpeech.text = e.ToString();
                    return skill;
                }
            }
        }

        private Skill IntentResponse(Intent intent, State state)
        {
            var skill = new Skill
            {
                version = "1.0",
                response = new Response()
                {
                    outputSpeech = new OutputSpeech()
                    {
                        type = "PlainText",
                    },
                    shouldEndSession = false
                },
                sessionAttributes = null
            };

            if(intent==null) skill.response.outputSpeech.text = "I can't find that information";

            switch (intent.name)
            {
                case "ReturnStatus":
                    skill.response.outputSpeech.text = "Your tax return status is " + state.Status;
                    break;
                case "ReturnTaxDue":
                    skill.response.outputSpeech.text = "Your tax due is " + state.TaxDue + " pounds";
                    break;
                case "ReturnIncomeSources":
                    var incomeSources = state.IncomeSource.Remove(state.IncomeSource.Length - 1).Split("_").Aggregate<String>((phrase, next) => phrase + "," + next);
                    skill.response.outputSpeech.text = "Your tax return income sources are: " + incomeSources;
                    break;
                case "ReturnFormEmail":
                    string dir = Environment.CurrentDirectory;
                    skill.response.outputSpeech.text = SendEmail(Path.Combine(dir, state.TaxYear + "_" + state.Email.Split("@")[0]),state.Email);
                    
                    break;
                default:
                    skill.response.outputSpeech.text = "I can't find the tax information you requested";
                    break;
            }

            return skill;
            //return new { version = "1.0", response = new { outputSpeech = new { type = "PlainText", text = state.Status } } };
            //return new { version = "1.0", response = new { outputSpeech = new { type = "PlainText", text = state.TaxDue } } };
        }

        private Skill FlowResponse(State state, string requestType)
        {
            //return new { version = "1.0", response = new { outputSpeech = new { type = "PlainText", text = "Welcome to Personal tax " + state.Name.Split(" ")[0] } } };
            var skill = new Skill()
            {
                version = "1.0",
                response = new Response()
                {
                    outputSpeech = new OutputSpeech()
                    {
                        type = "PlainText",
                    },
                },
                sessionAttributes = null
            };

            if (requestType == "LaunchRequest")
            {
                skill.response.outputSpeech.text = "Welcome to Personal tax ";
                skill.response.shouldEndSession = false;
            }
            else
                skill.response.outputSpeech.text = "Bye ";

            skill.response.outputSpeech.text += state.Name.Split(" ")[0];

            return skill;
        }

        // POST: api/values/updateStatus
        [Route("updateStatus")]
        [HttpPost]
        public object UpdateState(string taxyear, string email, string status, decimal taxdue, string name, string incomeSource)
        {
            try
            {
                Helper.WriteState(taxyear, email, status, taxdue, name, incomeSource);
                return new { response = "Status updated to " + status + " and tax due updated to " + taxdue + " for " + email + " for taxyear " + taxyear + " for name" + name + " for income source" + incomeSource};
            }
            catch(Exception e)
            {
                return e.ToString();
            }
                
        }

        private string SendEmail(string taxReturnPath, string toEmail)
        {
            taxReturnPath += ".pdf";
            if (Helper.ReturnExists(taxReturnPath))
            {
                try
                {
                    SmtpClient client = new SmtpClient("smtp.gmail.com");
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("ptax2018codegames@gmail.com", "ilab2018games");
                    client.EnableSsl = true;
                    client.Port = 587;

                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress("ptax2018codegames@gmail.com");
                    mailMessage.To.Add(toEmail);
                    mailMessage.Body = "Your tax return is attached";
                    mailMessage.Subject = "Tax return";
                    mailMessage.Attachments.Add(new Attachment(taxReturnPath));
                    client.Send(mailMessage);

                    return "Tax Return was sent";
                }
                catch (Exception ex)
                {
                    return "Error on sending the Tax Return";
                }
            }
            else
            {
                return "Tax Return does not exist on the server";
            }
            
        }

        [Route("savereturn")]
        [HttpPost]
        public object SaveReturn([FromBody] UpdatePDF updatePDF)
        {
            //byte[] buffer = new byte[Request.ContentLength.Value];
            //Request.Body.Read(buffer, 0, buffer.Length);
            //string content = Encoding.UTF8.GetString(buffer);
            byte[] byteContent = System.Convert.FromBase64String(updatePDF.base64PDF);

            Helper.SaveReturn(Path.Combine(Environment.CurrentDirectory, updatePDF.TaxYear + "_" + updatePDF.email.Split("@")[0]), byteContent);
            return true;
        }

        private static State GetState(Skill data)
        {
            string html = GetUserProfile(data);
            State state = new State();
            String email = ((State)JsonConvert.DeserializeObject(html, state.GetType())).Email;
            string taxyear = "2018";
            if (data.request.intent != null && data.request.intent.slots != null && data.request.intent.slots.TaxYear != null && data.request.intent.slots.TaxYear.value != null)
                taxyear = data.request.intent.slots.TaxYear.value;
            state = Helper.ReadState(taxyear, email);
            return state;
        }

        private static string GetUserProfile(Skill data)
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

            return html;
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
