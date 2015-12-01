using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Web.Script.Serialization;

namespace LOBConsole
{
    using System.Configuration;
    using System.Web.UI;

    class Program
    {
        static void Main(string[] args)
        {
            // All the below variable values are for developer sandbox environment. In case of a different environment, get these values from deployment team.
            //Pick user id and password from a configuration file and store in encrypted format in the config file using encryption algorithms like DPAPI.
            var parendId = "1966b6aa-7ade-45e1-91dd-99e73099e869";
            var parendId = "1966b6aa-7ade-45e1-91dd-99e73099e869";
            //////old
            

            #region authenticate service account against Azure AD
            var authContext = new AuthenticationContext(authority);
            var uc = new UserCredential(userid, pswd);
            var result = Task.Factory.StartNew(() => authContext.AcquireTokenAsync(
                resourceId,
                parendId,
                uc)).Result.Result;
            #endregion

           AddExpenseActivity(baseAddress, result.AccessToken);
            // AddActivityWithOGDetails(baseAddress, result.AccessToken);
           //UpdateExpenseActivity(baseAddress, result.AccessToken);
        }
       /// <summary>
       /// Add new activity to the IW feed. This sample also sends information to create Open Graph object and is required in case Yammer embed is being used on web pages. 
       /// If this is not the case, refer to AddActivity instead
       /// </summary>
       /// <param name="baseAddress">base address of Activity API Url</param>
       /// <param name="accessToken">Acess tokem for service account</param>
        static void AddActivityWithOGDetails(string baseAddress, string accessToken)
        {
            var activity = new ActivityMessage();
            activity.ActivityDate = DateTime.Now;
            activity.AppData = "{\"Type\": \"" + "News\"}";
            activity.AppId = "intranet";
            activity.Body = "It's time for open enrollment again!  Don't forget to elect your benefits by Dec 1.  Also there are some new benefits this year.";
            activity.RenderingTemplateVersion = "1.0";
            activity.SourceId = "2343353545"; // the unique identifier of the article on the intranet
            activity.OpenGraphItem = new OpenGraphItemMessage
            {
                Name = "Open Enrollment 2014",
                Description = "The period of time during which individuals who are eligible to enroll in a Qualified Health Plan can enroll in a plan. For coverage starting in 2015, the Open Enrollment Period is Nov 1 to Dec 1. Individuals may also qualify for Special Enrollment Periods outside of Open Enrollment if they experience certain events",
                Image = "http://www.worldbank.org/sites/default/files/images/WBG-logo-footer.png", // the url of a thumbnail image
                Url = "http://wbgintranet/news/2343353545" // the URL of the source page on the intranet
            };

            // serialize as json
            // note we have created an object and serializing it with Json.NET but could be a string
            // serialize as json
            var json = JsonConvert.SerializeObject(activity);
            bool isSuccess = ActivityAPISend(baseAddress, accessToken, "/api/Activity/Send", json);
            if (isSuccess)
            {
                Console.WriteLine("Success");
            }
            else
            {
                Console.WriteLine("Failure");
            }

            Console.Write("Press enter to finish");
            Console.ReadLine();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseAddress">base address of Activity API Url</param>
        /// <param name="accessToken">Acess tokem for service account</param>
        static void AddExpenseActivity(string baseAddress, string accessToken)
        {
            // expense report example
            // this is a private message between Katie Jordan and Garth Fort
            var activity = new ActivityMessage();
            activity.ActivityDate = DateTime.Now;
            activity.ActorEmail = "wbgonespace-jobs@devad.worldbank.org";
            activity.AppId = "expense";
            activity.Body = "Travel to Africa for project initiation";
            activity.DirectedToUserEmails = new[] { "wbgonespace-jobs@devad.worldbank.org" }; // when a message is directed to one or more users, it is private
            activity.RenderingTemplateVersion = "1.0";
            activity.SourceId = "7777"; 
            // serialize as json
            var json = JsonConvert.SerializeObject(activity);
            bool isSuccess = ActivityAPISend(baseAddress, accessToken, "/api/Activity/Send", json);
            if (isSuccess)
            {
                Console.WriteLine("Success");
            }
            else
            {
                Console.WriteLine("Failure");
            }

            Console.Write("Press enter to finish");
            Console.ReadLine();
        }

        /// <summary>
        /// Send updates for an existing activity to IW. When users take action in IW like approving expense report, the action details are sent to MSMQ by a Router service that will be listening to IW updates.
        /// The below code reads from MSMQ to get details about IW activity and sends approved status back to IW. Refer to SDK documentation for Router service.
        /// </summary>
        /// <param name="baseAddress">base address of Activity API Url</param>
        /// <param name="accessToken"></param>
        static void UpdateExpenseActivity(string baseAddress, string accessToken)
        {

           //Get details from latest message in MSMQ. Id and ActivityId are the two parametes that needs to be read from latest MSMQ message and sent to IW to identify the activity being 
           //updated and the corresponding action (like approve expense report)
            Dictionary<string, string> dict = GetUpdate();
            activityUpdate.AppId = "expensereport";
            activityUpdate.ActivityActionId = dict["Id"];//This is required to be passed to IW
            activityUpdate.SourceId = "7777";
            activityUpdate.ActivityId = dict["ActivityId"];//This is required to be passed to IW
           activityUpdate.TaskItem = new ActivityTaskMessage
            {
                ActorEmail = "wbgonespace-jobs@devad.worldbank.org",
                Description = "Approve Expense report",
                Title = "Test Task 124",
                DueDate = DateTime.Now.AddDays(3),
                StartDate = DateTime.Now
            };

            var json = JsonConvert.SerializeObject(activityUpdate);
            bool isSuccess = ActivityAPISend(baseAddress, accessToken, "/api/Activity/Update", json);
            if (isSuccess)
            {
                Console.WriteLine("Success");
            }
            else
            {
                Console.WriteLine("Failure");
            }

            Console.Write("Press enter to finish");
            Console.ReadLine();

        }

        /// <summary>
        /// Retirieves latest message from MSMQ.
        /// </summary>
        /// <returns>IW message details</returns>
        private static Dictionary<string, string> GetUpdate()
        {
           //ExpenseReport is the name of private queue in MSMQ where the messages will be published by router service. Seperate queues to be available for sepereate LOB's. Replace with 
           //queueu name specific to your application.
           MSMQHelper helper = new MSMQHelper("ExpenseReport");
            var actQ = helper.Dequeue().ToString();
            var jss = new JavaScriptSerializer();
            var dict = jss.Deserialize<Dictionary<string, string>>(actQ);
            //helper.Enqueue(actQ);
            return dict;
        }

        /// <summary>
        /// Send data to IW Activity API
        /// </summary>
       /// <param name="baseAddress">base address of Activity API Url</param>
       /// <param name="accessToken">Acess tokem for service account</param>
        /// <param name="action">Activity API send action</param>
        /// <param name="json">JSON to be passed to Activity API</param>
        /// <returns></returns>
        private static bool ActivityAPISend(string baseAddress, string accessToken, string action, string json)
        {
            using (StreamReader sr = new StreamReader(@"D:\Ram\LOBConsole\message.json"))
            {
                // Read the stream to a string, and write the string to the console.
                json = sr.ReadToEnd();
         
            }
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // send to IW activity API
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, baseAddress + action);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Content = content;

            try
            {
                var response = Task.Factory.StartNew(() => client.SendAsync(request)).Result.Result;
                if (response.IsSuccessStatusCode)
                    return true;
            }
            catch (Exception ex)
            {

               Console.Write(ex.Message);
            }

            return false;
        }
    }
}

