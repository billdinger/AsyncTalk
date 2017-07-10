using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AsyncDemo
{
    public class AsyncExamples
    {
        private Workload Load { get; }

        public AsyncExamples()
        {
            Load = new Workload();
        }

        public Task<int> StartATaskAsync()
        {
            var aTask = new Task<int>(() => 17 + 23);
            return aTask;
        }

        public async Task<string> GetAWebsiteAsync()
        {
            using (var httpclient = new HttpClient())
            {
                using (var result =
                    await httpclient.GetAsync("https://detroitcode.amegala.com"))
                {
                    var content =
                        await result.Content.ReadAsStringAsync();
                    return content;
                }
            }
        }

        public async Task GetAWebsiteContentAsync()
        {
            using (var httpclient = new HttpClient())
            {
                using (var result =
                    await httpclient.GetAsync("https://detroitcode.amegala.com"))
                {
                    // implicitly returned:
                    await result.Content.ReadAsStringAsync();
                }
            }
        }

        public async void GetAWebsiteOnClick(object sender, EventArgs e)
        {
            await GetAWebsiteAsync();
        }

        public async Task<int> GetAReturnCodeAsync()
        {
            using (var httpclient = new HttpClient())
            {
                using (var result =
                    await httpclient.GetAsync("https://detroitcode.amegala.com"))
                {
                    return (int)result.StatusCode;
                }
            }
        }
    }
}