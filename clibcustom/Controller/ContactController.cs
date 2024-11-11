using clibCustom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
 

namespace clibCustom.Controller
{
   public class ContactController : ApiController
    {
        [HttpPost]
        public async Task<HttpResponseMessage>  Send(ContactModel data)
        {
            var httpClient = new HttpClient();
            var PK = "YOUR_PRIVATE_KEY_HERE";
            var userIP = ((HttpContextBase)this.Request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            var uri = "http://www.google.com/recaptcha/api/verify";
 
            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("privatekey", PK));
            postData.Add(new KeyValuePair<string, string>("remoteip", userIP));
            postData.Add(new KeyValuePair<string, string>("challenge", data.Challenge));
            postData.Add(new KeyValuePair<string, string>("response", data.Response));
 
            HttpContent content = new FormUrlEncodedContent(postData);
 
            string responseFromServer = await httpClient.PostAsync(uri, content)
                    .ContinueWith((postTask) => postTask.Result.EnsureSuccessStatusCode())
                    .ContinueWith((readTask) => readTask.Result.Content.ReadAsStringAsync().Result);
 
            if (responseFromServer.StartsWith("true"))
            {
                // TODO: send an email blah blah
 
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Sorry mate, wrong captcha response. Are you a bot?");
            }
 
        }
    }
}
