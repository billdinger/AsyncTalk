using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApiAsyncExample.Controllers
{
    [Route("/home")]
    public class HomeController : ApiController
    {
        [Route("dontlock")]
        [HttpGet]
        public async Task<string> DontLock()
        {

            var tasks = new List<Task<string>>();
            for (int i = 0; i < 100; i++)
            {
                tasks.Add(await Task.Factory.StartNew(GetHttpResultAsync));
            }
            await Task.WhenAll(tasks);

            return DateTime.Now.ToString();
        }

        [Route("deadlock")]
        public string Deadlock()
        {
            // The following code WILL deadlock.
            var result = GetHttpResultAsync().Result;
            return result;
        }

        [Route("dolock")]
        [HttpGet]
        public string DoLock()
        {
            for (int i = 0; i < 100; i++)
            {
                var result = GetHttpResult();
            }

            return DateTime.Now.ToString();
        }

        private string GetHttpResult()
        {
            using (var httpclient = new HttpClient())
            {
                using (var result =
                    httpclient.GetAsync("https://www.sprint.com/api/digital/devices/v1/lookup/devices?defaultSKUPrice=SINGLE_PRICING&deviceType=PHONES&flow=GROSS_ADD"))
                {
                    using (var content = result.Result.Content.ReadAsStringAsync())
                    {
                        return content.Result;
                    }
                }
            }
        }

        private async Task<string> GetHttpResultAsync()
        {
            using (var httpclient = new HttpClient())
            {
                using (var result =
                    await httpclient.GetAsync("http://localhost:7385/solr/sitecore_web_index/select?q=*%3A*&wt=json&indent=true"))
                {
                    var content = await result.Content.ReadAsStringAsync();
                    return content;
                }
            }
        }
    }
}
