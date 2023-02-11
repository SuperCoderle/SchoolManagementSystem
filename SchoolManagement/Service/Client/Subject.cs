using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using SchoolDTOS;
using SchoolManagement.Data;
using System.Text.Json;

namespace SchoolManagement.Service.Client
{
    public class Subject : ComponentBase
    {
        [Inject]
        private IHttpClientFactory httpClient { get; set; }

        [Inject]
        private IConfiguration config { get; set; }

        [Inject]
        private SweetAlertService swal { get; set; }

        public Subject(IHttpClientFactory httpClient, IConfiguration config, SweetAlertService swal)
        {
            this.httpClient = httpClient;
            this.config = config;
            this.swal = swal;
        }

        public Subject()
        {

        }

        public IEnumerable<SubjectDTO> subjects = Array.Empty<SubjectDTO>();
        public IEnumerable<SubjectDTO> subjectSorted = Array.Empty<SubjectDTO>();
        public SubjectData sub = new SubjectData();
        private string errorMessage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await RefreshList();
        }

        protected async Task RefreshList()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, config["API_URL"] + "subject");
                var client = httpClient.CreateClient();
                var response = await client.SendAsync(request);
                using var responseStream = await response.Content.ReadAsStreamAsync();
                subjects = await JsonSerializer.DeserializeAsync<IEnumerable<SubjectDTO>>(responseStream);
                subjectSorted = subjects.OrderBy(s => Enum.Parse(typeof(DayOfWeek), s.SchoolDay));
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                await swal.FireAsync("Error!", "From Subject.cs: " + errorMessage, SweetAlertIcon.Error);
            }
        }
    }
}
