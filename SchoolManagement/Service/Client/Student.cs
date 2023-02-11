using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using SchoolDTOS;
using System.Text.Json;

namespace SchoolManagement.Service.Client
{
    public class Student : ComponentBase
    {
        [Inject]
        private IConfiguration config { get; set; }

        [Inject]
        private IHttpClientFactory httpClient { get; set; }

        public Student(IConfiguration config, IHttpClientFactory httpClient)
        {
            this.config = config;
            this.httpClient = httpClient;
        }

        public Student()
        {

        }

        public IEnumerable<StudentDTO> students = Array.Empty<StudentDTO>();

        protected override async Task OnInitializedAsync()
        {
            await RefreshList();
        }

        private async Task RefreshList()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, config["API_URL"] + "student");
            var client = httpClient.CreateClient();
            var response = await client.SendAsync(request);
            using var responseStream = await response.Content.ReadAsStreamAsync();
            students = await JsonSerializer.DeserializeAsync<IEnumerable<StudentDTO>>(responseStream);
        }
    }
}
