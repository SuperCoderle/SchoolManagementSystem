using Microsoft.AspNetCore.Components;
using SchoolDTOS;
using System.Text.Json;

namespace SchoolManagement.Service.Client
{
    public class Report : ComponentBase
    {
        [Inject]
        private IConfiguration config { get; set; }

        [Inject]
        private IHttpClientFactory httpClient { get; set; }

        public Report(IConfiguration config, IHttpClientFactory httpClient)
        {
            this.config = config;
            this.httpClient = httpClient;
        }

        public Report()
        {

        }

        public IEnumerable<ReportDTO> reports = Array.Empty<ReportDTO>();
        public IEnumerable<StudentDTO> students = Array.Empty<StudentDTO>();

        [Parameter]
        public int ID { get; set; }

        public string Path { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Path = config["PHOTOS_URL"] + "/Students/";
            await RefreshList();
        }

        protected async Task RefreshList()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, config["API_URL"] + "student/" + ID);
            var client = httpClient.CreateClient();
            var response = await client.SendAsync(request);
            using var responseStream = await response.Content.ReadAsStreamAsync();
            students = await JsonSerializer.DeserializeAsync<IEnumerable<StudentDTO>>(responseStream);

            var request2 = new HttpRequestMessage(HttpMethod.Get, config["API_URL"] + "report/" + ID);
            var client2 = httpClient.CreateClient();
            var response2 = await client2.SendAsync(request2);
            using var responseStream2 = await response2.Content.ReadAsStreamAsync();
            reports = await JsonSerializer.DeserializeAsync<IEnumerable<ReportDTO>>(responseStream2);
        }


        public int total = 0;

        protected int Total()
        {
            int sum = 0;
            foreach(var obj in reports)
            {
                total += 50;
                sum += obj.Point;
            }
            return sum;
        }

        protected double Grade()
        {
            if (total == 0)
                return 1;
            return Total() * 4 / total;
        }
    }
}
